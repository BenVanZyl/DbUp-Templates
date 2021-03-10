using CommandLine;
using Microsoft.Extensions.Configuration;
using System;
using static NetCore.DbMigrations.ScriptTokens;

namespace NetCore.DbMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" # Performing Db Upgrade ... ");
            string connectionString = "";
            DeploymentEnvironments deployTo = DeploymentEnvironments.LocalDev;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    Console.WriteLine($" ** Current Arguments: -e '{o.Environment}' -c '{OutputCnnString(o.ConnectionString)}'");

                    if (!string.IsNullOrWhiteSpace(o.ConnectionString))
                        connectionString = o.ConnectionString;
                    else
                        connectionString = DefaultConnectionString;

                    if (!string.IsNullOrWhiteSpace(o.Environment))
                    {
                        o.Environment = o.Environment.Trim().ToLower();

                        if (o.Environment == "localdev")
                            deployTo = DeploymentEnvironments.LocalDev;
                        else if (o.Environment == "testing" || o.Environment == "test")
                            deployTo = DeploymentEnvironments.Testing;
                        else if (o.Environment == "uat")
                            deployTo = DeploymentEnvironments.Uat;
                        else if (o.Environment == "production" || o.Environment == "prod")
                            deployTo = DeploymentEnvironments.Production;

                        else
                            throw new Exception("!!! ERROR: Environment not defined!");
                    }

                    Console.WriteLine($" ** ScriptExecutor.PerformUpgrade('{deployTo}', '{OutputCnnString(connectionString)}')");
                    ScriptExecutor.PerformUpgrade(deployTo, connectionString, true);

                });

            Console.WriteLine(" # Db Upgrade Completed. Done. ");

#if DEBUG
            Console.ReadKey();
#endif
        }

        private static string DefaultConnectionString => Configuration.GetConnectionString("DefaultConnection"); //.ConnectionString;
        private static string OutputCnnString(string cnnStr) => !string.IsNullOrWhiteSpace(cnnStr) && cnnStr.Length > 32 ? cnnStr.Substring(0, 32) : cnnStr;
        public class Options
        {
            [Option('e', "environment", Required = false, HelpText = "Deployment Environment")]
            public string Environment { get; set; }
            [Option('c', "connectionstring", Required = false, HelpText = "Connection string of database where scripts are being deployed to.")]
            public string ConnectionString { get; set; }
        }


        private static IConfigurationRoot _configuration = null;
        private static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var fileName = "appsettings.json";
#if DEBUG
                    Console.WriteLine("Debug version");
                    fileName = "appsettings.Development.json";
#endif

                    var builder = new ConfigurationBuilder()
                       .SetBasePath(AppContext.BaseDirectory)
                       .AddJsonFile(fileName, optional: true, reloadOnChange: true);

                    _configuration = builder.Build();
                }
                return _configuration;
            }
            set { _configuration = value; }
        }
    }
}