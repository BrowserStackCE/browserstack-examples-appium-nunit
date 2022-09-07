using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    [TestFixtureSource(typeof(MobileDriverTestRunner))]
    public class LoginTest : MobileDriverTestRunner
    {
        public LoginTest(AppiumOptions appiumOptions){
            this.appiumOptions = appiumOptions;
        }
        
        [SetUp]
        public void init(){
            GetDriver(appiumOptions);
            SetTestName(SessionName);
        }

        [TestCase("hellouser","hellopassword")]
        [TestCase("hellouser2","hellopassword")]
        [TestCase("hellouser3","hellopassword")]
        public void PerformLoginValidCreds(string user,string password)
        {
            LoginPage loginPage = new(App);
            loginPage.PerformLogin(user, password);
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
            MarkTestStatus();
            Shutdown();
        }
    }
}
