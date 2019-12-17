using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperRepositoryUowPattern.Models;
using DapperRepositoryUowPattern.Dapper;

namespace DapperRepositoryUowPattern.Controllers
{
    public class HomeController : Controller
    {
        private readonly DapperIUnitOfWork _uow = null;
        public HomeController(DapperIUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                //_uow.BeginTransaction();
                //@test
                //_uow.ContactsRepository.Add(new Contact()
                //{
                //    FirstName = "Prabin",
                //    LastName = "Siwakoti"
                //});

                // _uow.CommitChanges();

                // _uow.RollbackChanges();

                //@test base generic function

                //var datas = await _uow.ContactsRepository.GetAllAsync();

                //var contact = new Contact()
                //{
                //    FirstName = "Prabin",
                //    LastName = "Siwakoti",
                //};

                //await _uow.ContactsRepository.InsertAsync(contact);


                //UPDATE
                //var user = await _uow.ContactsRepository.GetAsync(1);
                //user.FirstName = "Prabesh";
                //user.LastName = "Siwakoti";

                //await _uow.ContactsRepository.UpdateAsync(user);

                //SAVE RANGE
                List<Contact> contacts = new List<Contact>();
                for (int i = 0; i < 1000; i++)
                {
                    contacts.Add(new Contact()
                    {
                        FirstName = "Hari " + i,
                        LastName = "Rai" + i
                    }); ;
                }

                await _uow.ContactsRepository.SaveRangeAsync(contacts);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
