using PhotoFiler.Models;

namespace PhotoFiler.Helpers.Repositories
{
    /// <summary>
    /// IPhotos Repository
    /// </summary>
    public interface IPhotosRepository
    {
        IPreviewablePhotos Create();
    }
}
