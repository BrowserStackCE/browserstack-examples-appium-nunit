using System.Collections.Generic;
using BrowserStack.App.Common;
using BrowserStack.WebDriver.Config;
using NUnit.Framework;
using OpenQA.Selenium.Appium;


namespace BrowserStack.App.Pages
{
    public class AboutPage
    {

        readonly Dictionary<DeviceType, PageObject> ButtonAbout = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "About") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "About") },
        };

        readonly Dictionary<DeviceType, PageObject> ButtonLearnMore = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "LearnMore") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "LearnMore") },
        };
        

        readonly TechnicalActions Actions;


        public AboutPage(AppiumDriver<AppiumWebElement> app)
        {
            Actions = new(app);
        }

        public void GoToAboutTab()
        {
            Actions.PerformClick(ButtonAbout);
            StringAssert.AreEqualIgnoringCase("Learn More", Actions.GetText(ButtonLearnMore));
        }

    }
}