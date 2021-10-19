using System.Collections.Generic;

namespace BrowserStack.WebDriver.Config
{
    public class LocalTunnelConfig
    {
        public Dictionary<string, string> LocalOptions { get; private set; }
        public bool IsEnabled { get; private set; }
    }
}