using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.Models
{
    /// <summary>
    /// IPhotos Repository
    /// </summary>
    public interface IPhotosRepository
    {
        IPreviewablePhotos Create();
    }
}
