using System.Collections.Generic;

namespace PhotoFiler.Photo.Models
{
    /// <summary>
    /// IAlbum Repository
    /// </summary>
    public interface IAlbumRepository
    {
        IHashedAlbum Create(
            List<IPreviewablePhoto> photos
        );
    }
}
