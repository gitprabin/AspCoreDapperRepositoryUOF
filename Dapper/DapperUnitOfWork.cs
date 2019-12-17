using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using static DapperRepositoryUowPattern.Startup;
using DapperRepositoryUowPattern.Repositories;

namespace DapperRepositoryUowPattern.Dapper
{
    public class DapperUnitOfWork : DapperIUnitOfWork
    {
        private IContactsRepository _contactsRepository = null;
        private readonly IDbConnection _connection = null;
        private IDbTransaction _transaction = null;

        public DapperUnitOfWork()
        {
            IConfiguration configuration = ConfigBuilder.Build();
            _connection = new SqlConnection(configuration.GetConnectionString("DbContextString"));
            _connection.Open();
        }
      
        public IContactsRepository ContactsRepository => _contactsRepository ?? (_contactsRepository = new ContactsRepository(_connection, () => _transaction,"Contact"));
        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void CommitChanges()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void RollbackChanges()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

    }
}
