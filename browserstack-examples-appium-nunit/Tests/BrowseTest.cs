using System;
using browserstack_examples_appium_nunit.Pages;
using NUnit.Framework;

namespace browserstack_examples_appium_nunit.Tests
{
    [TestFixture]
    public class BrowseTest : MobileDriverTestRunner
    {

        public BrowseTest() : base() { }


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

    }
}
