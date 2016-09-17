using System;
using System.IO;
using PhotoFiler.Models;
using PhotoFiler.Helpers.Hasher;
using System.Text;

namespace PhotoFiler.Helpers
{
    public class Configuration : IConfiguration
    {
        private const string ROOTH_PATH = "RoothPath";
        private const string HASH_LENGTH = "HashLength";
        private const string CREATE_PREVIEW = "CreatePreview";
        private const string ENABLE_LOGGING = "EnableLogging";

        /// <summary>
        /// Root path of the folder where the photos are stored
        /// </summary>
        public DirectoryInfo RootPath { get; set; }

        /// <summary>
        /// Lenght of hash to generate per photo. Default is 5.
        /// </summary>
        public int HashLength { get; set; } = 5;

        /// <summary>
        /// Flag to indicate if preview files for photos are generated when the application is started.
        /// </summary>
        public bool CreatePreview { get; set; } = false;

        /// <summary>
        /// Location where the preview files are stored
        /// </summary>
        public DirectoryInfo PreviewLocation { get; set; }

        /// <summary>
        /// Hashing function to use
        /// </summary>
        public IHasher HashingFunction { get; set; }

        /// <summary>
        /// Flag to indicate if we want to enable logging
        /// </summary>
        public bool EnableLogging { get; set; }

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
            EnableLogging = bool.Parse(settings[ENABLE_LOGGING]);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"RootPath: \"{RootPath}\"");
            builder.Append(System.Environment.NewLine);

            builder.Append($"HashLength: {HashLength}");
            builder.Append(System.Environment.NewLine);

            builder.Append($"CreatePreview: {CreatePreview}");
            builder.Append(System.Environment.NewLine);

            builder.Append($"PreviewLocation: \"{PreviewLocation}\"");
            builder.Append(System.Environment.NewLine);

            builder.Append($"HashingFunction: {HashingFunction}");
            builder.Append(System.Environment.NewLine);

            builder.Append($"EnableLogging: {EnableLogging}");

            return builder.ToString();
        }
    }
}