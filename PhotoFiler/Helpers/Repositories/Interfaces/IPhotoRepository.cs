using PhotoFiler.Models;
using System.IO;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Helpers.Repositories
{
    public interface IPhotoRepository
    {
        IPreviewablePhoto Create(FileInfo file, ErrorGeneratingPreview errorGeneratingPreviewHandler = null);
    }
}
