using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedAlbumRepository : LoggedBase, IAlbumRepository
    {
        IAlbumRepository _AlbumRepository;

        public LoggedAlbumRepository(
            ILogger logger,
            IAlbumRepository albumRepository
        ) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (albumRepository == null)
                throw new ArgumentNullException(nameof(albumRepository));

            _AlbumRepository = albumRepository;
        }

        public IHashedAlbum Create(List<IPreviewablePhoto> photos)
        {
            Logger.Information($"Creating album with { photos.Count()} photos.");
            Logger
                .Verbose(
                    photos
                        .Select(photo => photo.FileInfo.ToString())
                        .ToArray()
                );

            return
                new LoggedAlbum(
                    Logger,
                    _AlbumRepository.Create(photos)
                );            
        }
    }
}