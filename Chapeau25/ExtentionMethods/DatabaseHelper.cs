using Microsoft.Data.SqlClient;

namespace Chapeau25.ExtentionMethods
{
    public static class DatabaseHelper
    {
        private static string _connectionString;

        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeau2025Database");
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}