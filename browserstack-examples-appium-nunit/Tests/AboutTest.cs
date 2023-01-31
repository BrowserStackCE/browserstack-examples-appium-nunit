using System;
using browserstack_examples_appium_nunit.Pages;
using NUnit.Framework;

namespace browserstack_examples_appium_nunit.Tests
{
    [TestFixture]
    public class AboutTest : MobileDriverTestRunner
    {

        public AboutTest() : base() { }

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

    }
}
