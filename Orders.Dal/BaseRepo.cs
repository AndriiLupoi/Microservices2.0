using System.Data;
using Microsoft.Data.SqlClient;

namespace Orders.Dal
{
    public class BaseRepo
    {
        protected readonly string _connectionString;

        protected BaseRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
