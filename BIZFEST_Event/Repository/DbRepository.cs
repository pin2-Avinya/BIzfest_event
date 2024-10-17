using BIZFEST_Event.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BIZFEST_Event.Repository
{
    public class DbRepository : IDbRepository
    {
        private readonly IConfiguration _configuration;
        public DbRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
       
        public IDbConnection CreateConnection(string connectionString) => new SqlConnection(connectionString);
       
    }
}
