using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperRepositoryUowPattern.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private Func<IDbTransaction> _transaction = null;
        private readonly string _tableName;
        public BaseRepository(IDbConnection connection, Func<IDbTransaction> transaction, string tableName)
        {
            Connection = connection;
            _transaction = transaction;
            _tableName = tableName;
        }
        protected IDbConnection Connection { get; private set; }
        protected IDbTransaction Transaction => _transaction();

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Connection.QueryAsync<T>($"SELECT * FROM {_tableName}", null, Transaction ?? Transaction);
        }

        public async Task DeleteRowAsync(int id)
        {
            await Connection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id }, Transaction ?? Transaction);

        }

        public async Task<T> GetAsync(int id)
        {

            var result = await Connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id }, Transaction ?? Transaction);
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");

            return result;
        }

        public async Task<int> SaveRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery();

            inserted += await Connection.ExecuteAsync(query, list, Transaction ?? Transaction);

            return inserted;
        }

        public async Task InsertAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();
            await Connection.ExecuteAsync(insertQuery, t, Transaction ?? Transaction);
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties).Where(s => !s.Contains("Id")).ToList();

            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        public async Task UpdateAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery();

            await Connection.ExecuteAsync(updateQuery, t, Transaction ?? Transaction);
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }
    }
}
