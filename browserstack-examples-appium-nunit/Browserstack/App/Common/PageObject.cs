namespace BrowserStack.App.Common
{
    public class PageObject
    {

        public LocatorType LocatorType { get; private set; }
        public string LocatorValue { get; private set; }

        public PageObject(LocatorType type, string value)
        {
            LocatorType = type;
            LocatorValue = value;
        }
    }
}