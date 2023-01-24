using System;
using browserstack_examples_appium_nunit.Pages;
using NUnit.Framework;

namespace browserstack_examples_appium_nunit.Tests
{
    [TestFixture]
    public class LoginTest : MobileDriverTestRunner
    {

        public LoginTest():base() { }

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
    }
}