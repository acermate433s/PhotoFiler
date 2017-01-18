using Photo.Models;

namespace Photo.Models
{
    /// <summary>
    /// IPhotos Repository
    /// </summary>
    public interface IPhotosRepository
    {
        IPreviewablePhotos Create();
    }
}
