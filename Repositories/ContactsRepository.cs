using Dapper;
using DapperRepositoryUowPattern.Models;
using DapperRepositoryUowPattern.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperRepositoryUowPattern.Repositories
{
    public class ContactsRepository : BaseRepository<Contact>, IContactsRepository
    {
        public ContactsRepository(IDbConnection connection, Func<IDbTransaction> transaction, string tableName)
            : base(connection, transaction, tableName)
        { }

        #region IContactsRepository

        public void Add(Contact contact)
        {
            const string queryContact = "insert into contact(contact_first_name, contact_last_name) values (@fn, @ln);" +
                                        "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = Connection.Query<int>(queryContact, new { fn = contact.FirstName, ln = contact.LastName }, this.Transaction ?? this.Transaction);
        }
        #endregion
    }
}
