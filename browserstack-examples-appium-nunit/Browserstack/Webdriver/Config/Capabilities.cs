using System.Collections.Generic;

namespace BrowserStack.WebDriver.Config
{
    public class Capabilities
    {
        public IDictionary<string, object> BStackOptions { get; private set; }
        public IDictionary<string, object> PlatformOptions { get; private set; }

    }
}