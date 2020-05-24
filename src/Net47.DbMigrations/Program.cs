using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;

namespace Net47.DbMigrations
{
    class Program
    {
        private static string _connectionString;

        static void Main(string[] args)
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            DbMigration.PerformUpgrade(_connectionString);

            Console.ReadKey();
        }

    }
}
