using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DbMigrations
{
    /// <summary>
    /// String token to help determine what scripts needs to run when
    /// Can be changed to read values from the app seetings file.
    /// </summary>
    public static class Tokens
    {
        public static string Predeployment => "00_Predeployment";
        public static string Deployment => "01_Deployment";
        public static string DeployOnly => "02_Deploy_Only";
        public static string TestOnly => "03_Test_Only";
        
    }
}
