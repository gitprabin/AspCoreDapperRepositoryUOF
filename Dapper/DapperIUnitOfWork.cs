using DapperRepositoryUowPattern.Repositories;

namespace DapperRepositoryUowPattern.Dapper
{
    public interface DapperIUnitOfWork
    {
        IContactsRepository ContactsRepository { get; }
        void Dispose();
        void BeginTransaction();
        void CommitChanges();
        void RollbackChanges();
    }
}