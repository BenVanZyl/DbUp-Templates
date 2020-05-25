using Shouldly;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Net47.DbMigrations.Test
{
    public class DbMigrationTests
    {
        private const string SchemaScriptsPath = @"..\..\..\Net47.DbMigrations\Scripts";

        [Fact]
        public void VerifyAllScriptsEmbedded()
        {
            //var generalScriptsPath = Assembly.GetExecutingAssembly().RelativePath(SchemaScriptsPath);
            var scriptsOnDisk = Directory.GetFiles(SchemaScriptsPath, "*.sql", SearchOption.AllDirectories).Select(Path.GetFileName);
            var scriptsEmbedded = Assembly.GetAssembly(typeof(Net47.DbMigrations.DbMigration)).GetManifestResourceNames();

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
