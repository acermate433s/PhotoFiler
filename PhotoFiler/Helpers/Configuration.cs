namespace PhotoFiler.Helper
{
    public class Configuration
    {
        private const string ROOTH_PATH = "RoothPath";
        private const string HASH_LENGTH = "HashLength";
        private const string CREATE_PREVIEW = "CreatePreview";

        public string RootPath { get; set; }

        public int HashLength { get; set; } = 5;

        public bool CreatePreview { get; set; } = false;

        public Configuration()
        {
            var settings = System.Configuration.ConfigurationManager.AppSettings;
            RootPath = settings[ROOTH_PATH];
            HashLength = int.Parse(settings[HASH_LENGTH]);
            CreatePreview = bool.Parse(settings[CREATE_PREVIEW]);
        }
    }
}