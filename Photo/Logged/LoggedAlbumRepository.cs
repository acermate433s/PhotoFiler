using Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Telemetry;

namespace Photo.Logged
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

        public IHashedAlbum Create(IPhotosRepository repository, Helpers.Helpers.ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            Logger.Information($"Creating album.");

            return
                new LoggedAlbum(
                    Logger,
                    _AlbumRepository.Create(repository, errorGeneratingPreviewHandler)
                );            
        }
    }
}