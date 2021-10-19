using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    [TestFixtureSource(typeof(MobileDriverTestRunner))]
    public class BrowseTest: MobileDriverTestRunner
    {

        public BrowseTest(AppiumOptions appiumOptions)
        {
            App = GetDriver(appiumOptions);
        }

        [OneTimeSetUp]
        public void Init()
        {
            SetTestName(this.GetType().Name);
        }    

        [Test]
        public void CheckItemCount()
        {

            BrowsePage browsePage = new(App);
            browsePage.ValidateBrowseItem("Sixth item");

        }

        [Test]
        public void AddNewItem()
        {
            BrowsePage browsePage = new(App);
            browsePage.AddNewItem("Seventh item", "This is an item description");
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