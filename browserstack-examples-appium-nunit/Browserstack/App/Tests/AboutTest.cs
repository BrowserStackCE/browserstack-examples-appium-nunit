using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    public class AboutTest : MobileDriverTestRunner
    {

        [Test]
        [TestCaseSource(typeof(MobileDriverTestRunner))]
        public void GoToAboutTab(AppiumOptions appiumOptions)
        {
            AboutPage aboutPage = new(GetDriver(appiumOptions));
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