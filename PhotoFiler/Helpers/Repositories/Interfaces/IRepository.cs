namespace PhotoFiler.Helpers.Repositories
{
    /// <summary>
    /// Creates IPhoto, IPhotos and IAlbum repositories
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Creates the photo repository.
        /// </summary>
        /// <returns></returns>
        IPhotoRepository CreatePhotoRepository();

        /// <summary>
        /// Creates the photos repository.
        /// </summary>
        /// <returns></returns>
        IPhotosRepository CreatePhotosRepository();

        /// <summary>
        /// Creates the album repository.
        /// </summary>
        /// <returns></returns>
        IAlbumRepository CreateAlbumRepository();
    }
}   