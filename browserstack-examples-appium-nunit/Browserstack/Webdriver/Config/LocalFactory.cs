using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace BrowserStack.WebDriver.Config
{
    public class LocalFactory
    {
        private static LocalFactory Instance { get; set; }
        private readonly Local local = new();

        private readonly int LocalIdentifier = new Random().Next(0, 10000);
        static readonly object Lock = new();
        private static readonly ILog log = LogManager.GetLogger(typeof(LocalFactory));

        private LocalFactory(Dictionary<String, String> args)
        {
            try
            {
                args.Add("localIdentifier", LocalIdentifier.ToString());
                local.start(args.ToList());
                log.Info(("Started BrowserStack Local with identifier {}.", LocalIdentifier,
                    LocalIdentifier.ToString()));
            }
            catch (Exception e)
            {
                log.Error("Initialization of BrowserStack Local with identifier {} failed.", e);
                throw new Exception("Initialization of BrowserStack Local failed.", e);
            }
        }

        public static void CreateInstance(Dictionary<String, String> args)
        {
            if (Instance == null)
            {
                lock (LocalFactory.Lock)
                {
                    if (Instance == null)
                    {
                        Instance = new LocalFactory(args);
                    }
                }
            }
        }

        public static LocalFactory GetInstance()
        {
            return Instance;
        }

        public int GetLocalIdentifier()
        {
            return Instance.LocalIdentifier;
        }
    }

}