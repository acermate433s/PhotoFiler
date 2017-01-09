using PhotoFiler.Models;

namespace PhotoFiler.Models
{
    /// <summary>
    /// IPhotos Repository
    /// </summary>
    public interface IPhotosRepository
    {
        IPreviewablePhotos Create();
    }
}
