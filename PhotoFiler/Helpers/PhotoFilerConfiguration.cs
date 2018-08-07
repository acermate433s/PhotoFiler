using Photo.FileSystem;
using Photo.Hasher;
using Photo.Logged;
using Photo.Models;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace PhotoFiler.Helpers
{
    public class PhotoFilerConfiguration : ConfigurationSection, IFileSystemConfiguration, ILoggedConfiguration
    {
        private const string ROOT_PATH = "rootPath";
        private const string HASH_LENGTH = "hashLength";
        private const string CREATE_PREVIEW = "createPreview";
        private const string ENABLE_LOGGING = "enableLogging";

        /// <summary>
        /// Root path of the folder where the photos are stored
        /// </summary>
        [ConfigurationProperty(ROOT_PATH)]
        public string RoothPath { get => (string) this[ROOT_PATH]; set => this[ROOT_PATH] = value; }

        /// <summary>
        /// Root path of the folder where the photos are stored
        /// </summary>
        public DirectoryInfo RootPathDirectory { get => new DirectoryInfo(this.RoothPath); }

        /// <summary>
        /// Lenght of hash to generate per photo. Default is 5.
        /// </summary>
        [ConfigurationProperty(HASH_LENGTH)]
        public int HashLength { get => (int) this[HASH_LENGTH]; set => this[HASH_LENGTH] = value; }

        /// <summary>
        /// Flag to indicate if preview files for photos are generated when the application is started.
        /// </summary>
        [ConfigurationProperty(CREATE_PREVIEW)]
        public bool CreatePreview { get => (bool) this[CREATE_PREVIEW]; set => this[CREATE_PREVIEW] = value; }

        /// <summary>
        /// Location where the preview files are stored
        /// </summary>
        public DirectoryInfo PreviewLocationDirectory { get; }

        /// <summary>
        /// Hashing function to use
        /// </summary>
        public IHashFunction HashingFunction { get; set; }

        /// <summary>
        /// Flag to indicate if we want to enable logging
        /// </summary>
        [ConfigurationProperty(ENABLE_LOGGING)]
        public bool EnableLogging { get => (bool) this[ENABLE_LOGGING]; set => this[ENABLE_LOGGING] = value; }

        public PhotoFilerConfiguration()
        {
            var previewPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            if (Directory.Exists(previewPath))
                PreviewLocationDirectory = new DirectoryInfo(previewPath);
            else
                throw new DirectoryNotFoundException("Preview location path not found!");

            HashingFunction = new MD5(this.HashLength);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"RootPath: \"{RootPathDirectory}\"");
            builder.Append(System.Environment.NewLine);

            builder.Append($"HashLength: {HashLength}");
            builder.Append(System.Environment.NewLine);

            builder.Append($"CreatePreview: {CreatePreview}");
            builder.Append(System.Environment.NewLine);

            builder.Append($"PreviewLocation: \"{PreviewLocationDirectory}\"");
            builder.Append(System.Environment.NewLine);

            builder.Append($"HashingFunction: {HashingFunction}");
            builder.Append(System.Environment.NewLine);

            builder.Append($"EnableLogging: {EnableLogging}");

            return builder.ToString();
        }
    }
}