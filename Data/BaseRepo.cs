using Microsoft.Data.SqlClient;

namespace Cargill.Reconc.Data
{
    public class BaseRepo
    {
        private readonly string _connectionString;
        protected readonly ILogger _logger;

        public BaseRepo(IConfiguration config, ILogger logger)
        {
            _connectionString = config.GetConnectionString("Reconc");
            _logger = logger;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}