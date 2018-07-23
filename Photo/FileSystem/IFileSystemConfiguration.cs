using Photo.Models;
using System.IO;

namespace Photo.FileSystem
{
    public interface IFileSystemConfiguration
    {
        /// <summary>
        /// Flag to indicate if previews are created on startup
        /// </summary>
        bool CreatePreview { get; }

        /// <summary>
        /// Length of the hash 
        /// </summary>
        int HashLength { get; }

        /// <summary>
        /// Rooth directory where the photos are stored
        /// </summary>
        DirectoryInfo RootPathDirectory { get; }

        /// <summary>
        /// Location of the directory where the generated previews are stored
        /// </summary>
        DirectoryInfo PreviewLocationDirectory { get; }

        /// <summary>
        /// Hashing function to use
        /// </summary>
        IHashFunction HashingFunction { get; }
    }
}
