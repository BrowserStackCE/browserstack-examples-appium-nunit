using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    [TestFixtureSource(typeof(MobileDriverTestRunner))]
    public class AboutTest : MobileDriverTestRunner
    {

        public AboutTest(AppiumOptions appiumOptions)
        {
           App = GetDriver(appiumOptions);
        }


        [OneTimeSetUp]
        public void Init()
        {
            SetTestName(this.GetType().Name);
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

            App.ResetApp();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            MarkTestStatus();
            Shutdown();
        }
    }
}