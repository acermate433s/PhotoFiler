using PhotoFiler.Models;
using System;
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
            return
                new LoggedAlbumRepository(
                    _Logger,
                    _Repository.CreateAlbumRepository()
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            return
                new LoggedPhotoRepository(
                    _Logger,
                    _Repository.CreatePhotoRepository()
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            return
                new LoggedPhotosRepository(
                    _Logger,
                    _Repository.CreatePhotosRepository()
                );
        }
    }
}