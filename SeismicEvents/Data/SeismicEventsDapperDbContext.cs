using System.Data;

namespace SeismicEventsFireEvents.Data
{
    public class SeismicEventsDapperDbContext
    {
        private readonly IConfiguration _configuration;
        public SeismicEventsDapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("SQLLiteConnection");
            return new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
        }
    }
}
