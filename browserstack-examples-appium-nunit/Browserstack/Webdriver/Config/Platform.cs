using System;
using System.Collections.Generic;

namespace BrowserStack.WebDriver.Config
{
    public class Platform
    {
        public String Name { get; private set; }

        public Capabilities Capabilities { get; private set; }
    }
}