using DapperRepositoryUowPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperRepositoryUowPattern.Repositories
{
    public interface IContactsRepository : IBaseRepository<Contact>
    {
        void Add(Contact contact);
    }
}
