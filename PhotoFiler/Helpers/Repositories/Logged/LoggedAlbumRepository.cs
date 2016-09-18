using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _Logger.Information($"Creating album with { photos.Count()} photos.");
            _Logger
                .Verbose(
                    photos
                        .Select(photo => photo.FileInfo.ToString())
                        .ToArray()
                );

            return
                new LoggedHashedAlbum(
                    _Logger,
                    _AlbumRepository.Create(photos)
                );            
        }
    }
}