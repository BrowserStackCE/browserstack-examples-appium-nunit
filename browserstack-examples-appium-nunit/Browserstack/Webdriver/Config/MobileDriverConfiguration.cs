using System.Collections.Generic;

namespace BrowserStack.WebDriver.Config
{
    public class MobileDriverConfiguration
    {

        public DriverType DriverType { get; private set; }

        public OnPremDriverConfig OnPremDriverConfig { get; private set; }

        public RemoteDriverConfig OnPremGridDriverConfig { get; private set; }

        public RemoteDriverConfig CloudDriverConfig { get; private set; }

        public List<Platform> GetActivePlatforms()
        {
            List<Platform> activePlatforms = new();
            switch (DriverType)
            {
                case DriverType.OnPremDriver:
                    activePlatforms = OnPremDriverConfig.Platforms;
                    break;
                case DriverType.OnPremGridDriver:
                    activePlatforms = OnPremGridDriverConfig.Platforms;
                    break;
                case DriverType.CloudDriver:
                    activePlatforms = CloudDriverConfig.Platforms;
                    break;
            }
            return activePlatforms;
        }
    }
}