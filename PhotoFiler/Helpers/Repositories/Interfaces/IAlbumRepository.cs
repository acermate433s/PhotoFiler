using PhotoFiler.Models;
using System.Collections.Generic;

namespace PhotoFiler.Helpers.Repositories
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
