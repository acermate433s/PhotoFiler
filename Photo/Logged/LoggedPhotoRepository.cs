using System;
using System.IO;

using Microsoft.Extensions.Logging;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedPhotoRepository : LoggedBase, IPhotoRepository
    {
        private readonly IPhotoRepository photoRepository;

        public LoggedPhotoRepository(
            ILogger logger,
            IPhotoRepository photoRepository
        ) : base(logger)
        {
            this.photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
        }

        public  IPreviewablePhoto Create(
            FileInfo file, 
            IExifReaderService exifReader,
            ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null
        )
        {
            this.Logger.LogInformation($"Creating instance photo for \"{file}\"");
            
            return
                new LoggedPreviewablePhoto(
                    this.Logger,
                    this.photoRepository.Create(file, exifReader, errorGeneratingPreviewHandler)
                );            
        }
    }
}