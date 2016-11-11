using PhotoFiler.Models;
using System.IO;

namespace PhotoFiler.Models
{
    /// <summary>
    /// Application configuration settings
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Flag to indicate if previews are created on startup
        /// </summary>
        bool CreatePreview { get; set; }

        /// <summary>
        /// Length of the hash 
        /// </summary>
        int HashLength { get; set; }

        /// <summary>
        /// Rooth directory where the photos are stored
        /// </summary>
        DirectoryInfo RootPath { get; set; }

        /// <summary>
        /// Location of the directory where the generated previews are stored
        /// </summary>
        DirectoryInfo PreviewLocation { get; set; }

        /// <summary>
        /// Hashing function to use
        /// </summary>
        IHashFunction HashingFunction { get; set; }

        /// <summary>
        /// Flag to indicate if logging is generated
        /// </summary>
        bool EnableLogging { get; set; }
    }
}