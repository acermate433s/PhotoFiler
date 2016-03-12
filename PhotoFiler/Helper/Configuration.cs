using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoFiler.Helper
{
    public class Configuration
    {
        const string ROOTH_PATH = "RoothPath";
        const string HASH_LENGTH = "HashLength";

        public string RootPath { get; set; }

        public int HashLength { get; set; } = 5;

        public Configuration()
        {
            var settings = System.Configuration.ConfigurationManager.AppSettings;
            RootPath = settings[ROOTH_PATH];
            HashLength = int.Parse(settings[HASH_LENGTH]);
        }
    }
}