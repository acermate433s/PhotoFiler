using Microsoft.Extensions.Logging;
using PhotoFiler.Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoFiler.Photo.Logged
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
            Logger.LogInformation($"Creating album with { photos.Count()} photos.");
            Logger.LogInformation(
                String.Join(
                    Environment.NewLine,
                    photos
                        .Select(photo => photo.Location)
                        .ToArray()
                ));

            return
                new LoggedAlbum(
                    Logger,
                    _AlbumRepository.Create(photos)
                );            
        }
    }
}