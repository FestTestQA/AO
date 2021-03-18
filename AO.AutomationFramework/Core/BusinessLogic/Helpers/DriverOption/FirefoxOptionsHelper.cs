using OpenQA.Selenium.Firefox;

namespace AO.AutomationFramework.Core.BusinessLogic.Helpers.DriverOption
{
    public class FirefoxOptionsHelper
    {
        public static FirefoxOptions GetMobileEmulationOption(string deviceName)
        {
            var profile = new FirefoxProfile();
            int pixelRatio = 1;
            int width;
            string user_agent;
            int height;
            switch (deviceName)
            {
                case "Sony Xperia XZ1 Compact":
                    user_agent = "Mozilla/5.0 (Linux; Android 9.0.0; Xperia XZ: 41.3.A.2.192) Gecko/20100101 Firefox/73.0";
                    width = 720;
                    height = 1280;
                    break;

                case "Apple iPhone X":
                    user_agent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1";
                    width = 375;
                    height = 812;
                    pixelRatio = 3;
                    break;

                case "Samsung Galaxy S9":
                    user_agent = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.84 Mobile Safari/537.36";
                    width = 360;
                    height = 740;
                    pixelRatio = 4;
                    break;

                case "Pixel 2":
                    user_agent = "Mozilla/5.0 (Linux; Android 7.0; Pixel C Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/52.0.2743.98 Safari/537.36";
                    width = 411;
                    height = 731;
                    pixelRatio = 2;
                    break;

                case "Samsung Galaxy Tab S3":
                    user_agent = "Mozilla/5.0 (Linux; Android 7.0; SM-T827R4 Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.116 Safari/537.36";
                    width = 2048;
                    height = 1563;
                    break;

                default:
                    user_agent = "Mozilla/5.0 (iPad; CPU OS 11_0 like Mac OS X) AppleWebKit/604.1.34 (KHTML, like Gecko) Version/11.0 Mobile/15A5341f Safari/604.1";
                    width = 768;
                    height = 1024;
                    pixelRatio = 2;
                    break;
            }
            profile.SetPreference("general.useragent.override", user_agent);
            profile.SetPreference("devtools.responsive.viewport.height", height);
            profile.SetPreference("devtools.responsive.viewport.width", width);
            profile.SetPreference("devtools.responsive.viewport.pixelRatio", pixelRatio);
            return new FirefoxOptions { Profile = profile };
        }
    }
}