using Microsoft.Extensions.Logging;
using PhotoFiler.Photo.Models;
using System;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedRepository : LoggedBase, IRepository
    {
        IRepository _Repository;

        public LoggedRepository(
            ILogger logger,
            IRepository repository
        ) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _Repository = repository;
        }

        public IAlbumRepository CreateAlbumRepository()
        {
            Logger.LogInformation("Creating IAlbumRepository instance");

            return
                new LoggedAlbumRepository(
                    Logger,
                    _Repository.CreateAlbumRepository()
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            Logger.LogInformation("Creating IPhotoRepository instance");

            return
                new LoggedPhotoRepository(
                    Logger,
                    _Repository.CreatePhotoRepository()
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            Logger.LogInformation("Creating IPhotosRepository instance");

            return
                new LoggedPhotosRepository(
                    Logger,
                    _Repository.CreatePhotosRepository()
                );
        }
    }
}