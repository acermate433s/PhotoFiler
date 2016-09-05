namespace PhotoFiler.Helpers.Repositories
{
    public interface IRepository
    {
        IPhotoRepository CreatePhotoRepository();

        IPhotosRepository CreatePhotosRepository();

        IAlbumRepository CreateAlbumRepository();
    }
}   