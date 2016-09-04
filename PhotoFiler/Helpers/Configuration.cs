using System;
using System.IO;
using PhotoFiler.Models;
using PhotoFiler.Helpers.Hasher;

namespace PhotoFiler.Helpers
{
    public class Configuration : IConfiguration
    {
        private const string ROOTH_PATH = "RoothPath";
        private const string HASH_LENGTH = "HashLength";
        private const string CREATE_PREVIEW = "CreatePreview";

        public DirectoryInfo RootPath { get; set; }

        public int HashLength { get; set; } = 5;

        public bool CreatePreview { get; set; } = false;

        public DirectoryInfo PreviewLocation { get; set; }

        public IHasher HashingFunction { get; set; }

        public Configuration()
        {
            var settings = System.Configuration.ConfigurationManager.AppSettings;

            if ((settings[ROOTH_PATH] != null) && (Directory.Exists(settings[ROOTH_PATH])))
                RootPath = new DirectoryInfo(settings[ROOTH_PATH]);
            else
                throw new DirectoryNotFoundException("Root path for photos not found!");

            HashLength = int.Parse(settings[HASH_LENGTH]);
            CreatePreview = bool.Parse(settings[CREATE_PREVIEW]);

            var previewPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            if (Directory.Exists(previewPath))
                PreviewLocation = new DirectoryInfo(previewPath);
            else
                throw new DirectoryNotFoundException("Preview location path not found!");

            HashingFunction = new MD5();
        }
    }
}