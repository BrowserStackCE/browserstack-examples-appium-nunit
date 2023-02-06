using browserstack_examples_appium_nunit.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System;

namespace browserstack_examples_appium_nunit
{
    [TestFixture]
    public class MobileDriverTestRunner
    {

        protected AppiumDriver<AppiumWebElement> App;

        [SetUp]
        public void Init()
        {
            AppiumOptions appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Samsung Galaxy S21");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "11");
            object Os = appiumOptions.ToCapabilities().GetCapability("platformName");
            if (Os is not null && Os.ToString().ToLower().Equals(DeviceType.Android.ToString().ToLower()))
            {
                App = new AndroidDriver<AppiumWebElement>(new Uri("https://hub.browserstack.com/wd/hub"), appiumOptions);
            }
            else if (Os is not null && Os.ToString().ToLower().Equals(DeviceType.Ios.ToString().ToLower()))
            {
                App = new IOSDriver<AppiumWebElement>(new Uri("https://hub.browserstack.com/wd/hub"), appiumOptions);
            }
        }

        protected void SetTestName(string name)
        {
            ((IJavaScriptExecutor)App).ExecuteScript("browserstack_executor: {\"action\": \"setSessionName\", \"arguments\": {\"name\":\"" + name + "\" }}");
        }

        protected void MarkTestStatus()
        {
            if (TestContext.CurrentContext.Result.FailCount == 0)
            {
                ((IJavaScriptExecutor)App).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"All " + TestContext.CurrentContext.Result.PassCount + " test(s) completed successfully.\"}}");
            }
            else
            {
                ((IJavaScriptExecutor)App).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"" + TestContext.CurrentContext.Result.Message + ". Failed: " + TestContext.CurrentContext.Result.FailCount +
                    ", Passed: " + TestContext.CurrentContext.Result.PassCount +
                    ". \"}}");
            }
        }

        [TearDown]
        public void Cleanup()
        {
            App.Quit();
        }

    }

}
