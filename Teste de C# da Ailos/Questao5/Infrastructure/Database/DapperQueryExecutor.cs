// File: DapperQueryExecutor.cs
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Database
{
    public class DapperQueryExecutor : IQueryExecutor
    {
        private readonly IDbConnection _dbConnection;

        public DapperQueryExecutor(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters)
        {
            return _dbConnection.QueryFirstOrDefaultAsync<T>(query, parameters);
        }
    }

    public interface IQueryExecutor
    {
        Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters);
    }
}
