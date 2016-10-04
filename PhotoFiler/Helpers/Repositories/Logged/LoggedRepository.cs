using System;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
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
            Logger.Information("Creating IAlbumRepository instance");

            return
                new LoggedAlbumRepository(
                    Logger,
                    _Repository.CreateAlbumRepository()
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            Logger.Information("Creating IPhotoRepository instance");

            return
                new LoggedPhotoRepository(
                    Logger,
                    _Repository.CreatePhotoRepository()
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            Logger.Information("Creating IPhotosRepository instance");

            return
                new LoggedPhotosRepository(
                    Logger,
                    _Repository.CreatePhotosRepository()
                );
        }
    }
}