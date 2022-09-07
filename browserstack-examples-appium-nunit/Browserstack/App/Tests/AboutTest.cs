using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    [TestFixtureSource(typeof(MobileDriverTestRunner))]
    public class AboutTest : MobileDriverTestRunner
    {
        public AboutTest(AppiumOptions appiumOptions){
            this.appiumOptions = appiumOptions;
        }

        [SetUp]
        public void init(){
            GetDriver(appiumOptions);
            SetTestName(SessionName);
        }

        [Test]
        public void GoToAboutTab()
        {
            AboutPage aboutPage = new(App);
            aboutPage.GoToAboutTab();
        }


        [TearDown]
        public void TearDown()
        {
            if (!TestContext.CurrentContext.Result.Outcome.Status.ToString().Equals("Passed"))
            {
                Console.WriteLine(App.PageSource);
            }
            SetTestName(SessionName);
            MarkTestStatus();
            Shutdown();
        }
    }
}
