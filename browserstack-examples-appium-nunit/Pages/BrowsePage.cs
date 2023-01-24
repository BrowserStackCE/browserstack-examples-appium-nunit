using System;
using System.Collections.Generic;
using browserstack_examples_appium_nunit.Common;
using NUnit.Framework;
using OpenQA.Selenium.Appium;


namespace browserstack_examples_appium_nunit.Pages
{
    public class BrowsePage
    {
        readonly Dictionary<DeviceType, PageObject> ButtonBrowse = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "Browse") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "Browse") },
        };

        readonly Dictionary<DeviceType, PageObject> LabelItem = new()
        {
            { DeviceType.Android, new(LocatorType.AndroidUIAutomator2, "UiSelector().className(\"android.widget.TextView\").text(\"<dynamic>\")") },
            { DeviceType.Ios, new(LocatorType.IOSPredicateString, "label == \"<dynamic>\"") },
        };

        readonly Dictionary<DeviceType, PageObject> ButtonAddItem = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "AddToolbarItem") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "AddToolbarItem") },
        };


        readonly Dictionary<DeviceType, PageObject> EditNewItemText = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "ItemNameEntry") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "ItemNameEntry") },
        };

        readonly Dictionary<DeviceType, PageObject> EditNewItemDescription = new()
        {
            { DeviceType.Android, new(LocatorType.AccessibilityId, "ItemDescriptionEntry") },
            { DeviceType.Ios, new(LocatorType.AccessibilityId, "ItemDescriptionEntry") },
        };

        readonly Dictionary<DeviceType, PageObject> ButtonSave = new()
        {
            { DeviceType.Android, new(LocatorType.AndroidUIAutomator2, "UiSelector().className(\"android.widget.TextView\").text(\"SAVE\")") },
            { DeviceType.Ios, new(LocatorType.IOSPredicateString, "label == \"Save\"") },

        };

        readonly TechnicalActions Actions;

        public BrowsePage(AppiumDriver<AppiumWebElement> app)
        {
            Actions = new(app);
        }

        public void ValidateBrowseItem(string itemLabel)
        {
            Actions.PerformClick(ButtonBrowse);
            ValidateItemExists(LabelItem, itemLabel);

        }

        public void AddNewItem(string itemName, string itemDescription)
        {
            Actions.PerformClick(ButtonBrowse);
            Actions.PerformClick(ButtonAddItem);
            Actions.SetText(EditNewItemText, itemName);
            Actions.SetText(EditNewItemDescription, itemDescription);
            Actions.PerformClick(ButtonSave);
            ValidateItemExists(LabelItem, itemName);
        }

        public void ValidateItemExists(Dictionary<DeviceType, PageObject> pageElement, string itemLabel)
        {
            Assert.IsTrue(Actions.ElementExists(pageElement, itemLabel));
        }

    }
}