using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperRepositoryUowPattern.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteRowAsync(int id);
        Task<T> GetAsync(int id);
        Task<int> SaveRangeAsync(IEnumerable<T> list);      
        Task InsertAsync(T t);
        Task UpdateAsync(T t);
    }
}
