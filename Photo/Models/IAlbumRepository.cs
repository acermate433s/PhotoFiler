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
            IPhotosRepository repository,
            Helpers.Helpers.ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null
        );
    }
}
