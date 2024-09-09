using System.Data;
using System.Data.SQLite;

namespace Questao5.Infrastructure.Sqlite
{
    public class SqliteConnectionFactory
    {
        private readonly string _connectionString;

        public SqliteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }

}
