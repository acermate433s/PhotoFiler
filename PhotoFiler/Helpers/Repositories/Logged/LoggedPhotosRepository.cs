﻿using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedPhotosRepository : IPhotosRepository
    {
        ILogger _Logger;
        IPhotosRepository _PhotosRepository;

        public LoggedPhotosRepository(
            ILogger logger,
            IPhotosRepository photosRepository
        ) 
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (photosRepository == null)
                throw new ArgumentNullException(nameof(photosRepository));

            _Logger = logger;
            _PhotosRepository = photosRepository;
        }

        public IPreviewablePhotos Create()
        {
            _Logger.Information("Creating photo repository.");
            
            return
                new LoggedPreviewablePhotos(
                    _Logger,
                    _PhotosRepository.Create()
                );
            
        }
    }
}