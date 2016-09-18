﻿using System;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedRepository : IRepository
    {
        ILogger _Logger;
        IRepository _Repository;

        public LoggedRepository(
            ILogger logger,
            IRepository repository
        )
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _Logger = logger;
            _Repository = repository;
        }

        public IAlbumRepository CreateAlbumRepository()
        {
            _Logger.Information("Creating IAlbumRepository instance");

            return
                new LoggedAlbumRepository(
                    _Logger,
                    _Repository.CreateAlbumRepository()
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            _Logger.Information("Creating IPhotoRepository instance");

            return
                new LoggedPhotoRepository(
                    _Logger,
                    _Repository.CreatePhotoRepository()
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            _Logger.Information("Creating IPhotosRepository instance");

            return
                new LoggedPhotosRepository(
                    _Logger,
                    _Repository.CreatePhotosRepository()
                );
        }
    }
}