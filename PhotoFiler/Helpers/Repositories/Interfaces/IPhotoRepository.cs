using PhotoFiler.Models;
using System.IO;

namespace PhotoFiler.Helpers.Repositories
{
    public interface IPhotoRepository
    {
        IPreviewablePhoto Create(FileInfo file);
    }
}
