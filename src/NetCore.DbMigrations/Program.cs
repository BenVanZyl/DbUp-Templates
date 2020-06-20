using Microsoft.Extensions.Configuration;
using System.IO;


namespace NetCore.DbMigrations
{
    class Program
    {
        private static string _connectionString;

        static void Main(string[] args)
        {
            _connectionString = Configuration.GetConnectionString("DefaultConnection");
            ScriptExecutor.PerformUpgrade(_connectionString);
            // return 0;
        }

        private static IConfigurationRoot _configuration = null;
        private static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    _configuration = builder.Build();
                }
                return _configuration;
            }
            set { _configuration = value; }
        }
    }
}
