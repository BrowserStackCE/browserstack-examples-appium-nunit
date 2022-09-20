using System;
using System.Collections;
using System.Collections.Generic;
using BrowserStack.WebDriver.Core;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App
{
    public class MobileDriverTestRunner: IEnumerable 
    {
        protected AppiumOptions appiumOptions;
        protected Dictionary<string,AppiumDriver<AppiumWebElement>> drivers = new Dictionary<string, AppiumDriver<AppiumWebElement>>();

        protected string TestID{
            get{
                return TestContext.CurrentContext.Test.ID;
            }
        }

        protected string SessionName{
            get{
                return TestContext.CurrentContext.Test.Name;
            }
        }

        protected AppiumDriver<AppiumWebElement> App{
            get{
                return drivers[TestID];
            }
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(MobileDriverTestRunner));
        public IEnumerator GetEnumerator()
        {
         
            MobileDriverFactory mobileDriverFactory = MobileDriverFactory.GetInstance();
            List<WebDriver.Config.Platform> list;
            list = mobileDriverFactory.GetPlatforms();
            foreach (WebDriver.Config.Platform platform in list)
            {
                object fixtureArgs = mobileDriverFactory.CreateRemoteMobileCapabilities(platform);
                log.Info(("Initialising driver with capabilities : {}", fixtureArgs));
                yield return fixtureArgs;
            }
        }

        protected void SetTestName(String name)
        {
            name = name.Replace("\""," ");
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

        protected void Shutdown()
        {
            App.Quit();
        }

        protected AppiumDriver<AppiumWebElement> GetDriver(AppiumOptions appiumOptions)
        {
            MobileDriverFactory mobileDriverFactory = MobileDriverFactory.GetInstance();
            var driver =  mobileDriverFactory.CreateRemoteMobileDriver(appiumOptions);
            drivers.Add(TestID,driver);
            return driver;
        }

    }

}
