using System;
using System.Linq;

namespace Net48.DbMigrations
{
    public static class RunningAs
    {
        private static bool? _unitTest = null;

        public static bool UnitTest
        {
            get
            {
                if (!_unitTest.HasValue)
                {   //no value assigned, check assemblies
                    if (AppDomain.CurrentDomain.GetAssemblies()
                        .Any(w => w.FullName.ToLowerInvariant().StartsWith("xunit"))
                        )
                        _unitTest = true;
                    else
                        _unitTest = false;
                }

                return _unitTest.Value;
            }
        }

    }
}
