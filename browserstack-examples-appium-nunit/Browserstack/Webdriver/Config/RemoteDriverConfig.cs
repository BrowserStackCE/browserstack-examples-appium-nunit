using System;
using System.Collections.Generic;

namespace BrowserStack.WebDriver.Config
{
    public class RemoteDriverConfig
    {
        public string HubUrl { get; private set; }

        public string User { get; private set; }

        public string Key { get; private set; }

        public Capabilities CommonCapabilities { get; private set; }

        public LocalTunnelConfig LocalTunnel { get; private set; }

        public List<Platform> Platforms { get; private set; }
    }
}