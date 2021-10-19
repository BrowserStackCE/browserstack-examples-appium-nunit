using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using YamlDotNet.Serialization.NamingConventions;
using BrowserStack.WebDriver.Config;
using Platform = BrowserStack.WebDriver.Config.Platform;
using OpenQA.Selenium.Appium.iOS;
using log4net;
using System.Linq;

namespace BrowserStack.WebDriver.Core
{

    public class MobileDriverFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MobileDriverFactory));
        private readonly string DEFAULT_CAPABILITIES_FILE = "capabilities.yml";
        private readonly string BROWSERSTACK_USERNAME = "BROWSERSTACK_USERNAME";
        private readonly string BROWSERSTACK_ACCESS_KEY = "BROWSERSTACK_ACCESS_KEY";
        private readonly string BROWSERSTACK_ANDROID_APP_ID = "BROWSERSTACK_ANDROID_APP_ID";
        private readonly string BROWSERSTACK_IOS_APP_ID = "BROWSERSTACK_IOS_APP_ID";
        private readonly string BUILD_ID = "BROWSERSTACK_BUILD_NAME";
        private readonly string DEFAULT_BUILD_NAME = "browserstack-examples-appium_nunit";
        public readonly string CAPABILITIES_DIR = "/Browserstack/Webdriver/Resources/";

        private readonly MobileDriverConfiguration MobileDriverConfiguration;
        private readonly string DefaultBuildSuffix;
        private readonly bool IsLocal;
        static readonly object Lock = new ();

        private static MobileDriverFactory instance;


        public MobileDriverFactory()
        {
            this.DefaultBuildSuffix = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            this.MobileDriverConfiguration = ParseMobileDriverConfig();
            List<Platform> Platforms = MobileDriverConfiguration.GetActivePlatforms();

            IsLocal = MobileDriverConfiguration.CloudDriverConfig != null &&
                    MobileDriverConfiguration.CloudDriverConfig.LocalTunnel.IsEnabled;

            if (IsLocal)
            {
                Dictionary<string, string> localOptions = MobileDriverConfiguration.CloudDriverConfig.LocalTunnel.LocalOptions;

                string accessKey = Environment.GetEnvironmentVariable(BROWSERSTACK_ACCESS_KEY) ?? MobileDriverConfiguration.CloudDriverConfig.Key;

                localOptions.Add("key", accessKey);

                LocalFactory.CreateInstance(MobileDriverConfiguration.CloudDriverConfig.LocalTunnel.LocalOptions);
            }
            Log.Debug(("Running tests on {} active platforms.", Platforms,
                  Platforms.Count().ToString()));
        }

        public static MobileDriverFactory GetInstance()
        {
            if (instance == null)
            {
                lock (MobileDriverFactory.Lock)
                {
                    if (instance == null)
                    {
                        instance = new MobileDriverFactory();
                    }
                }
            }
            return instance;
        }

        private MobileDriverConfiguration ParseMobileDriverConfig()
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

            string capabilitiesConfig =
                    Environment.GetEnvironmentVariable("CAPABILITIES_FILENAME");
            

            if (capabilitiesConfig == null)
            {
                capabilitiesConfig = DEFAULT_CAPABILITIES_FILE;
                
            }

            MobileDriverConfiguration mobileDriverConfiguration = deserializer.Deserialize<MobileDriverConfiguration>(File.ReadAllText(Directory.GetCurrentDirectory() + CAPABILITIES_DIR + capabilitiesConfig));

            return mobileDriverConfiguration;
        }

        public AppiumOptions CreateMobilePlatformCapabilities(Platform platform)
        {
            AppiumOptions mobileDriverOptions = null;

            switch (this.MobileDriverConfiguration.DriverType)
            {
                case DriverType.OnPremDriver:
                    mobileDriverOptions = CreateOnPremMobileCapabilities(platform);
                    break;
                case DriverType.CloudDriver:
                    mobileDriverOptions = CreateRemoteMobileCapabilities(platform);
                    break;
                case DriverType.OnPremGridDriver:
                    mobileDriverOptions = CreateOnPremGridMobileCapabilities(platform);
                    break;
                default:

                    break;

            }

            return mobileDriverOptions;
        }

        public List<Platform> GetPlatforms()
        {
            return this.MobileDriverConfiguration.GetActivePlatforms();
        }

        public AppiumOptions CreateRemoteMobileCapabilities(Platform platform)
        {
            RemoteDriverConfig remoteDriverConfig = this.MobileDriverConfiguration.CloudDriverConfig;
            CommonCapabilities commonCapabilities = remoteDriverConfig.CommonCapabilities;
            AppiumOptions platformCapabilities = new();

            if (commonCapabilities.Capabilities != null)
            {
                foreach (KeyValuePair<string, object> tuple in commonCapabilities.Capabilities.CapabilityMap)
                {

                    platformCapabilities.AddAdditionalCapability(tuple.Key.ToString(), tuple.Value);

                }
            }

            if (platform.Capabilities != null)
            {
                foreach (KeyValuePair<string, object> tuple in platform.Capabilities.CapabilityMap)
                {
                    platformCapabilities.AddAdditionalCapability(tuple.Key.ToString(), tuple.Value);

                }
            }

            string user = Environment.GetEnvironmentVariable(BROWSERSTACK_USERNAME) ?? remoteDriverConfig.User;
            string accessKey = Environment.GetEnvironmentVariable(BROWSERSTACK_ACCESS_KEY) ?? remoteDriverConfig.Key;


            platformCapabilities.AddAdditionalCapability("browserstack.user", user);
            platformCapabilities.AddAdditionalCapability("browserstack.key", accessKey);

            if (IsLocal)
            {
                platformCapabilities.AddAdditionalCapability("browserstack.localIdentifier", LocalFactory.GetInstance().GetLocalIdentifier());
            }

            object build = platformCapabilities.ToCapabilities().GetCapability("build");

            if (build is not null)
            {
                platformCapabilities.AddAdditionalCapability("build", CreateBuildName(build.ToString()));
            }

            return platformCapabilities;
        }

        public AppiumDriver<AppiumWebElement> CreateRemoteMobileDriver(AppiumOptions platformCapabilities)
        {
            AppiumDriver<AppiumWebElement> driver;
            object Os = platformCapabilities.ToCapabilities().GetCapability("os");
            if (Os is not null && Os.ToString().ToLower().Equals(DeviceType.Android.ToString().ToLower()))
            {
                if (Environment.GetEnvironmentVariable(BROWSERSTACK_ANDROID_APP_ID) != null)
                {
                    platformCapabilities.AddAdditionalCapability("app", Environment.GetEnvironmentVariable(BROWSERSTACK_ANDROID_APP_ID));
                }

                driver = new AndroidDriver<AppiumWebElement>(new Uri(this.MobileDriverConfiguration.CloudDriverConfig.HubUrl), platformCapabilities);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                return driver;
            }
            else if (Os is not null && Os.ToString().ToLower().Equals(DeviceType.Ios.ToString().ToLower()))
            {
                if (Environment.GetEnvironmentVariable(BROWSERSTACK_IOS_APP_ID) != null)
                {
                    platformCapabilities.AddAdditionalCapability("app", Environment.GetEnvironmentVariable(BROWSERSTACK_IOS_APP_ID));
                }

                driver = new IOSDriver<AppiumWebElement>(new Uri(this.MobileDriverConfiguration.CloudDriverConfig.HubUrl), platformCapabilities);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                return driver;
            }
            else
            {
                Log.Error(String.Format("Platform {0} is not supported.Please specify one among {1} ", Os,
                    Enum.GetValues(typeof(DeviceType))));
                throw new Exception(String.Format("Platform '{0}' is not supported. Please specify one among {1}", Os,
                    Enum.GetValues(typeof(DeviceType))));
            }
        }

        private String CreateBuildName(String buildPrefix)
        {
            if (String.IsNullOrEmpty(buildPrefix))
            {
                buildPrefix = DEFAULT_BUILD_NAME;
            }
            String buildName = buildPrefix;

            String buildSuffix = Environment.GetEnvironmentVariable(BUILD_ID);

            if (string.IsNullOrEmpty(buildSuffix))
            {
                buildSuffix = this.DefaultBuildSuffix;
            }

            return String.Format("{0}-{1}", buildName, buildSuffix);
        }


        private AppiumOptions CreateOnPremGridMobileCapabilities(Platform platform)
        {

            throw new NotImplementedException("On Prem Grid Appium driver is not yet implemented");
        }

        private AppiumOptions CreateOnPremGridMobileDriver(Platform platform)
        {

            throw new NotImplementedException("On Prem Grid Appium driver is not yet implemented");
        }

        private AppiumOptions CreateOnPremMobileCapabilities(Platform platform)
        {

            throw new NotImplementedException("On Prem Appium driver is not yet implemented");
        }

        private AppiumOptions CreateOnPremMobileDriver(Platform platform)
        {

            throw new NotImplementedException("On Prem Appium driver is not yet implemented");
        }
    }
}