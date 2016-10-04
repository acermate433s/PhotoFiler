﻿using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedPhotosRepository : LoggedBase, IPhotosRepository
    {
        IPhotosRepository _PhotosRepository;

        public LoggedPhotosRepository(
            ILogger logger,
            IPhotosRepository photosRepository
        ) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (photosRepository == null)
                throw new ArgumentNullException(nameof(photosRepository));

            _PhotosRepository = photosRepository;
        }

        public IPreviewablePhotos Create()
        {
            Logger.Information("Creating photo repository.");
            
            return
                new LoggedPreviewablePhotos(
                    Logger,
                    _PhotosRepository.Create()
                );
            
        }
    }
}