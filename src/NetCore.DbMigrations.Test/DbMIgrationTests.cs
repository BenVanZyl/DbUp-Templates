using Shouldly;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace NetCore.DbMigrations.Test
{
    public class DbMigrationTests
    {
        private const string SchemaScriptsPath = @"..\..\..\..\NetCore.DbMigrations\Scripts";

        /// <summary>
        /// Error in this test means there is a script that has not been marked as embedded resource.  Checck the error message and scripts.
        /// </summary>
        [Fact]
        public void VerifyAllScriptsEmbedded()
        {
            //var generalScriptsPath = Assembly.GetExecutingAssembly().RelativePath(SchemaScriptsPath);
            var scriptsOnDisk = Directory.GetFiles(SchemaScriptsPath, "*.sql", SearchOption.AllDirectories).Select(Path.GetFileName);
            var scriptsEmbedded = Assembly.GetAssembly(typeof(NetCore.DbMigrations.DbMigration)).GetManifestResourceNames();

            foreach (var f in scriptsOnDisk)
            {
                scriptsEmbedded.ShouldContain(e => e.EndsWith(f), "Script file: " + f);
            }
        }


        [Fact]
        public void VerifyIsRunningAsTest()
        {
            // code is running in unit test mode
            RunningAs.UnitTest.ShouldBeTrue();

        }
    }
}
