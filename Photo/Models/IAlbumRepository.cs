using Photo.Models;
using System.Collections.Generic;

namespace Photo.Models
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
