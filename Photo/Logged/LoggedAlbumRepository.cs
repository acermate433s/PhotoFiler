using Microsoft.Extensions.Logging;
using PhotoFiler.Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedAlbumRepository : LoggedBase, IAlbumRepository
    {
        private readonly IAlbumRepository albumRepository;

        public LoggedAlbumRepository(
            ILogger logger,
            IAlbumRepository albumRepository
        ) : base(logger)
        {
            this.albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
        }

        public IHashedAlbum Create(List<IPreviewablePhoto> photos)
        {
            this.Logger.LogInformation($"Creating album with { photos.Count()} photos.");
            this.Logger.LogInformation(
                String.Join(
                    Environment.NewLine,
                    photos
                        .Select(photo => photo.Location)
                        .ToArray()
                ));

            return
                new LoggedAlbum(
                    this.Logger,
                    this.albumRepository.Create(photos)
                );            
        }
    }
}