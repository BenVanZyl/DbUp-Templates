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
            if (args.Length == 1)
                _connectionString = args[0];
            else
                _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            ScriptExecutor.PerformUpgrade(_connectionString);

            Console.ReadKey();
        }

    }
}
