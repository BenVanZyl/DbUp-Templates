using DbUp;
using System;
using System.Linq;
using System.Reflection;
using static Net48.DbMigrations.ScriptTokens;

namespace Net48.DbMigrations
{
    public static class ScriptExecutor
    {
        private static string _connectionString;

        public static bool PerformUpgrade(ScriptTokens.DeploymentEnvironments deployTo, string connectionString, bool ensureDbExists = false)
        {
            WriteTemplateInfo(connectionString, ensureDbExists);
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception(" !!! Error: No connectionstring provided!");
            _connectionString = connectionString;
            if (ensureDbExists)
                EnsureDatabase.For.SqlDatabase(_connectionString); //only needed when db must be created.

            // scripts that reset already run scripts in order to run updates on the speciifed object (table, proc, view, function, etc)
            DeployScripts(ScriptTokens.Predeployment);
            
            // actual deployment scripts
            DeployScripts(ScriptTokens.DeployDbObjects);
            
            // deploy data changes for all environments
            DeployScripts(ScriptTokens.DeployDataAllEnvironments);
            
            // deploy data changes for specified environment
            switch (deployTo)
            {
                case DeploymentEnvironments.LocalDev:
                    DeployScripts(ScriptTokens.DeployDataLocalDev);
                    break;
                case DeploymentEnvironments.Testing:
                    DeployScripts(ScriptTokens.DeployDataTesting);
                    break;
                case DeploymentEnvironments.Uat:
                    DeployScripts(ScriptTokens.DeployDataUat);
                    break;
                case DeploymentEnvironments.Production:
                    DeployScripts(ScriptTokens.DeployDataProduction);
                    break;
                default:
                    //nothing to do
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return true;
        }

        private static void DeployScripts(string stageToken)
        {
            //bool useAzureSqlIntegratedSecurity = true;

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_connectionString) //, null, useAzureSqlIntegratedSecurity)
                    .WithTransactionPerScript()
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.ToLower().Contains(stageToken.ToLower())) //, StringComparison.OrdinalIgnoreCase))
                    .LogToConsole()
                    .Build();

            if (!upgrader.TryConnect(out string errMsg))
                throw new Exception($" !!! Errors Occurred performing database update for stage token {stageToken} !!! /n {errMsg}");

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
                throw new Exception($" !!! Errors Occurred performing database update !!! /n{result.Error}");
            }
        }

        private static void WriteTemplateInfo(string connectionString, bool ensureDbExists = false)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("This code is based on the DbUp-Templates project as found on GitHub and is licensed to use and modify, free of charge, and without warranty.");
            Console.WriteLine("For more info and to get the code, please visit https://github.com/BenVanZyl/DbUp-Templates");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($" * Connectionstring = '{connectionString.Substring(0, 20)}'");
            Console.WriteLine($" * ensureDbExists = '{ensureDbExists}'");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
        }
    }
}
