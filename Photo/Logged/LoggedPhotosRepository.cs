using System;

using Microsoft.Extensions.Logging;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedPhotosRepository : LoggedBase, IPhotosRepository
    {
        private readonly IPhotosRepository photosRepository;

        public LoggedPhotosRepository(
            ILogger logger,
            IPhotosRepository photosRepository
        ) : base(logger)
        {
            this.photosRepository = photosRepository ?? throw new ArgumentNullException(nameof(photosRepository));
        }

        public IPreviewablePhotos Create()
        {
            this.Logger.LogInformation("Creating photo repository.");
            
            return
                new LoggedPreviewablePhotos(
                    this.Logger,
                    this.photosRepository.Create()
                );
            
        }
    }
}