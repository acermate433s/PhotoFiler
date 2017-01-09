using PhotoFiler.Models;
using System.Collections.Generic;

namespace PhotoFiler.Models
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
