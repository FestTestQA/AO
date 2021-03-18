using System.Collections.Generic;

namespace AO.AutomationFramework.Core.BusinessLogic.Variables
{
    public static class CommonVar
    {
        #region Settings

        public static Branch branch = Branch.Live;

        #endregion Settings

        #region PortalUsers

        public const string Anon = "";

        public const string TestRep1 = "royeje6596@hubopss.com";

        public static List<string> TestUsersEmails = new List<string>()
        {
            "royeje6596@hubopss.com",
        };

        #endregion PortalUsers

        #region TimeOuts

        public static int WaitTimeout = 10000;

        public static int WaitTimeStep = 100;

        public static int WaitTimeFactor = 1;

        public static int PdfFileOpenTimeout = 3000;

        #endregion TimeOuts

        #region Paths

        public static string ScreenshotPath = "C:\\Screenshots";

        public static string ResourcesFolderPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}Resources\\";

        #endregion Paths

        public enum Branch
        {
            Trunk,

            PreRelease,

            Live
        }

        public enum Browser
        {
            Chrome,

            IE,

            Edge,

            FireFox,

            Opera
        }

        public static string GetSiteURL()
        {
            return branch switch
            {
                Branch.Trunk => "https://test.ao.com",
                Branch.PreRelease => "https://pr.ao.com",
                Branch.Live => "https://www.ao.com",
                _ => "https://test.aqqord.com",
            };
        }
    }
}