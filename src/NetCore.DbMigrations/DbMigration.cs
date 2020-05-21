using DbUp;
using System;
using System.Reflection;

namespace NetCore.DbMigrations
{
    public static class DbMigration
    {
        private static string _connectionString;

        public static bool PerformUpgrade(string connectionString)
        {
            _connectionString = connectionString;

            EnsureDatabase.For.SqlDatabase(_connectionString);

            // scripts that reset already run scripts in order to run updates on the speciifed object (table, proc, view, function, etc)
            DeployScripts(Tokens.Predeployment);

            // actual deployment scripts
            DeployScripts(Tokens.Deployment);

            // only run these scripts when actually doing a deployment but not when doing testing.
            if (!RunningAs.UnitTest)
                DeployScripts(Tokens.DeployOnly);

            // only run these when testing
            if (RunningAs.UnitTest)
                DeployScripts(Tokens.TestOnly);

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
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains(stageToken, StringComparison.OrdinalIgnoreCase))
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
    }
}
