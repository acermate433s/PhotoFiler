using PhotoFiler.Models;
using System.Collections.Generic;

namespace PhotoFiler.Helpers.Repositories
{
    public interface IAlbumRepository
    {
        IHashedAlbum Create(
            List<IPreviewablePhoto> photos
        );
    }
}
