using PhotoFiler.Models;
using System.IO;

namespace PhotoFiler.Helpers
{
    public interface IConfiguration
    {
        bool CreatePreview { get; set; }

        int HashLength { get; set; }

        DirectoryInfo RootPath { get; set; }

        DirectoryInfo PreviewLocation { get; set; }

        IHasher HashingFunction { get; set; }
    }
}