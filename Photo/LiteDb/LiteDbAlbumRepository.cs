using Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Photo.LiteDb
{
    public class LiteDbAlbumRepository : IAlbumRepository
    {
        public LiteDbAlbumRepository(
            ILiteDbConfiguration configuration
        )
        {
            _Configuration = configuration;
        }

        private ILiteDbConfiguration _Configuration;

        public IHashedAlbum Create(IPhotosRepository repository, Helpers.Helpers.ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            return new LiteDbAlbum(repository);
        }
    }
}
