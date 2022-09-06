using System;
using BrowserStack.App.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Tests
{
    public class BrowseTest: MobileDriverTestRunner
    {   

        [Test]
        [TestCaseSource(typeof(MobileDriverTestRunner))]
        public void CheckItemCount(AppiumOptions appiumOptions)
        {

            BrowsePage browsePage = new(GetDriver(appiumOptions));
            browsePage.ValidateBrowseItem("Sixth item");

        }

        [Test]
        [TestCaseSource(typeof(MobileDriverTestRunner))]
        public void AddNewItem(AppiumOptions appiumOptions)
        {
            BrowsePage browsePage = new(GetDriver(appiumOptions));
            browsePage.AddNewItem("Seventh item", "This is an item description");
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