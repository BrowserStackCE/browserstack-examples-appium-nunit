using System;
using System.Collections.Generic;
using BrowserStack.WebDriver.Config;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace BrowserStack.App.Common
{
    public class TechnicalActions
    {
        protected AppiumDriver<AppiumWebElement> App;
        private static readonly ILog Log = LogManager.GetLogger(typeof(TechnicalActions));

        public TechnicalActions(AppiumDriver<AppiumWebElement> app)
        {
            App = app;
        }


        internal string GetText(Dictionary<DeviceType, PageObject> element)
        {
            AppiumWebElement appiumWebElement = FindElementByPlatform(element);
            var attributName = IsAndroid() ? "text" : "value";
            return appiumWebElement.GetAttribute(attributName);
        }

        internal void PerformClick(Dictionary<DeviceType, PageObject> element)
        {
            AppiumWebElement appiumWebElement = FindElementByPlatform(element);
            appiumWebElement.Click();
        }

        internal void SetText(Dictionary<DeviceType, PageObject> element, String text)
        {
            AppiumWebElement appiumWebElement = FindElementByPlatform(element);
            appiumWebElement.SendKeys(text);
        }

        internal bool ElementExists(Dictionary<DeviceType, PageObject> pageElement, string dynamicLocatorStr)
        {

            AppiumWebElement appiumWebElement = FindElementByPlatform(pageElement, dynamicLocatorStr);
            return appiumWebElement != null;
        }

        private bool IsAndroid()
        {
            return App
                .Capabilities
                .GetCapability("platformName")
                .ToString()
                .ToLower()
                .Equals(DeviceType.Android.ToString().ToLower());
        }


        private AppiumWebElement WaitForElement(By by, int duration = 30, bool forceFail = true)
        {
            AppiumWebElement webElement = null;
            try
            {
                WebDriverWait wait = new(App, TimeSpan.FromSeconds(duration));
                webElement = App.FindElement(by);
                wait.Until(c => webElement.Displayed);
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
                Log.Debug(App.PageSource);
                if(forceFail)
                {
                    throw new Exception("EXCEPTION: Could not find Web element - " +
                    e.Message);
                }
                
            }

            return webElement;
        }

        private AppiumWebElement FindElementByPlatform(Dictionary<DeviceType, PageObject> element, string dynamicLocatorValue = "")
        {
            AppiumWebElement appiumWebElement = null;
            string locatorValue;
            switch (IsAndroid())
            {
                case true:
                    PageObject androidLocator = element.GetValueOrDefault(DeviceType.Android);
                    locatorValue = androidLocator.LocatorValue.ToString().Replace("<dynamic>", dynamicLocatorValue);
                    appiumWebElement = FindElement(androidLocator.LocatorType, locatorValue);
                    break;
                case false:
                    PageObject iosLocator = element.GetValueOrDefault(DeviceType.Ios);
                    locatorValue = iosLocator.LocatorValue.ToString().Replace("<dynamic>", dynamicLocatorValue);
                    appiumWebElement = FindElement(iosLocator.LocatorType, locatorValue);
                    break;
            }

            return appiumWebElement;
        }

        private AppiumWebElement FindElement(LocatorType locatorType, string locatorValue)
        {
            switch(locatorType)
            {
                case LocatorType.AccessibilityId:
                    return WaitForElement(MobileBy.AccessibilityId(locatorValue));
                case LocatorType.ClassName:
                    return WaitForElement(MobileBy.ClassName(locatorValue));
                case LocatorType.XPath:
                    return WaitForElement(MobileBy.XPath(locatorValue));
                case LocatorType.Name:
                    return WaitForElement(MobileBy.Name(locatorValue));
                case LocatorType.Id:
                    return WaitForElement(MobileBy.Id(locatorValue));
                case LocatorType.AndroidUIAutomator2:
                    return WaitForElement(MobileBy.AndroidUIAutomator(locatorValue));
                case LocatorType.IOSPredicateString:
                    return WaitForElement(MobileBy.IosNSPredicate(locatorValue));
                case LocatorType.IOSClassChain:
                    return WaitForElement(MobileBy.IosClassChain(locatorValue));
                default:
                    return WaitForElement(MobileBy.AccessibilityId(locatorValue));
            }
        }
    }
}
