using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    [TestFixtureSource(typeof(MobileDriverTestRunner))]
    public class LoginTest : MobileDriverTestRunner
    {

        public LoginTest(AppiumOptions appiumOptions)
        {
            App = GetDriver(appiumOptions);
        }

        [OneTimeSetUp]
        public void Init()
        {
            SetTestName(this.GetType().Name);
        }


        [Test]
        public void PerformLoginValidCreds()
        {
            LoginPage loginPage = new(App);
            loginPage.PerformLogin("hellouser", "hellopassword");
            loginPage.ValidateLogin();
        }


        [Test]
        public void PerformLoginNoCreds()
        {
            LoginPage loginPage = new(App);
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