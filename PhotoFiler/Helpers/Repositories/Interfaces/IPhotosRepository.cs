using PhotoFiler.Models;

namespace PhotoFiler.Helpers.Repositories
{
    public interface IPhotosRepository
    {
        IPreviewablePhotos Create();
    }
}
