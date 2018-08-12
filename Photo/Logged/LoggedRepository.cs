using System;

using Microsoft.Extensions.Logging;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedRepository : LoggedBase, IRepository
    {
        private readonly IRepository repository;

        public LoggedRepository(
            ILogger logger,
            IRepository repository
        ) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IAlbumRepository CreateAlbumRepository()
        {
            this.Logger.LogInformation("Creating IAlbumRepository instance");

            return
                new LoggedAlbumRepository(
                    this.Logger,
                    this.repository.CreateAlbumRepository()
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            this.Logger.LogInformation("Creating IPhotoRepository instance");

            return
                new LoggedPhotoRepository(
                    this.Logger,
                    this.repository.CreatePhotoRepository()
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            this.Logger.LogInformation("Creating IPhotosRepository instance");

            return
                new LoggedPhotosRepository(
                    this.Logger,
                    this.repository.CreatePhotosRepository()
                );
        }
    }
}