using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedAlbumRepository : IAlbumRepository
    {
        ILogger _Logger;
        IAlbumRepository _AlbumRepository;

        public LoggedAlbumRepository(
            ILogger logger,
            IAlbumRepository albumRepository
        ) 
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (albumRepository == null)
                throw new ArgumentNullException(nameof(albumRepository));

            _Logger = logger;
            _AlbumRepository = albumRepository;
        }

        public IHashedAlbum Create(List<IPreviewablePhoto> photos)
        {
            return
                new LoggedHashedAlbum(
                    _Logger,
                    _AlbumRepository.Create(photos)
                );
        }
    }
}