using Microsoft.Data.Sqlite;

namespace ESP.DAL
{
    public class AppDbContext
    {
        private readonly string _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=esp.db";
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
