using System.IO;

namespace PhotoFiler.Models
{
    public interface IHashedPhotoPreviewer
    {
        IHashedPhoto Photo { get; set; }

        DirectoryInfo PreviewLocation { get; set; }

        bool Generate();

        byte[] Preview();

        byte[] View();
    }
}