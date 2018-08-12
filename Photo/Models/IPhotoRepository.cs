using System.IO;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Models
{
    /// <summary>
    /// IPhoto Repository
    /// </summary>
    public interface IPhotoRepository
    {
        IPreviewablePhoto Create(
            FileInfo file, 
            IExifReaderService exifReader,
            ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null
        );
    }
}
