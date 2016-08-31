using System.IO;

namespace PhotoFiler.Models
{
    public interface IPreviewableHashedPhoto : IHashedPhoto
    {
        byte[] Preview();

        byte[] View();
    }
}