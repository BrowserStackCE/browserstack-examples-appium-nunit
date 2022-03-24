using System;
using System.Collections.Generic;
using BrowserStack.App.Common;
using BrowserStack.WebDriver.Config;
using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace BrowserStack.App.Pages
{
    public class LoginPage
    {
        readonly Dictionary<DeviceType, PageObject> EditUserName = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "UserName") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "UserName") },
        };

        readonly Dictionary<DeviceType, PageObject> EditPassword = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "Password") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "Password") },
        };

        readonly Dictionary<DeviceType, PageObject> ButtonLogin = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "LoginButton") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "LoginButton") },
        };

        readonly Dictionary<DeviceType, PageObject> LabelLoginStatus = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "StatusLabel") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "StatusLabel") },
        };


        readonly TechnicalActions Actions;

        public LoginPage(AppiumDriver<AppiumWebElement> app)
        {
            Actions = new(app);
        }

        public void PerformLogin(String username, String password)
        {
            Actions.SetText(EditUserName, username);
            Actions.SetText(EditPassword, password);
            Actions.PerformClick(ButtonLogin);
        }

        public void PerformLogin()
        {
            Actions.PerformClick(ButtonLogin);
        }

        public void ValidateLogin()
        {
            StringAssert.Contains("Logging in", Actions.GetText(LabelLoginStatus));
        }

    }
}
