using DbUp;
using System;
using System.Linq;
using System.Reflection;

namespace Net47.DbMigrations
{
    public static class DbMigration
    {
        private static string _connectionString;

        public static bool PerformUpgrade(string connectionString)
        {
            WriteTemplateInfo();

            _connectionString = connectionString;

            EnsureDatabase.For.SqlDatabase(_connectionString);

            // scripts that reset already run scripts in order to run updates on the speciifed object (table, proc, view, function, etc)
            DeployScripts(ScriptTokens.Predeployment);

            // actual deployment scripts
            DeployScripts(ScriptTokens.Deployment);

            // only run these scripts when actually doing a deployment but not when doing testing.
            if (!RunningAs.UnitTest)
                DeployScripts(ScriptTokens.DeployOnly);

            // only run these when testing
            if (RunningAs.UnitTest)
                DeployScripts(ScriptTokens.TestOnly);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            return true;
        }

        private static void DeployScripts(string stageToken)
        {
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    .WithTransactionPerScript()
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains(stageToken)) //, StringComparison.OrdinalIgnoreCase))
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                //return -1;
                throw new Exception($"Errors Occurred performing database update!/n{result.Error}");
            }
        }

        private static void WriteTemplateInfo()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("This code is based on the DbUp-Templates project as found on GitHub and is licensed to use and modify, free of charge, and without warranty.");
            Console.WriteLine("For more info and to get the code, please visit https://github.com/BenVanZyl/DbUp-Templates");
            Console.WriteLine("----------------------------------------");
        }
    }
}
