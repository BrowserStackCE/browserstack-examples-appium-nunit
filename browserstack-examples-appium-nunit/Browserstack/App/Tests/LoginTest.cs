using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    public class LoginTest : MobileDriverTestRunner
    {

        [Test]
        [TestCaseSource(typeof(MobileDriverTestRunner))]
        public void PerformLoginValidCreds(AppiumOptions appiumOptions)
        {
            LoginPage loginPage = new(GetDriver(appiumOptions));
            loginPage.PerformLogin("hellouser", "hellopassword");
            loginPage.ValidateLogin();
        }


        [Test]
        [TestCaseSource(typeof(MobileDriverTestRunner))]
        public void PerformLoginNoCreds(AppiumOptions appiumOptions)
        {
            LoginPage loginPage = new(GetDriver(appiumOptions));
            loginPage.PerformLogin();
            loginPage.ValidateLogin();
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