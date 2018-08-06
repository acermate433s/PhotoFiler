using Microsoft.Extensions.Logging;
using Photo.Models;
using System;
using System.IO;

using static Photo.Helpers.Helpers;

namespace Photo.Logged
{
    public class LoggedPhotoRepository : LoggedBase, IPhotoRepository
    {
        IPhotoRepository _PhotoRepository;

        public LoggedPhotoRepository(
            ILogger logger,
            IPhotoRepository photoRepository
        ) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (logger == null)
                throw new ArgumentNullException(nameof(photoRepository));

            _PhotoRepository = photoRepository;
        }

        public  IPreviewablePhoto Create(
            FileInfo file, 
            ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null
        )
        {
            Logger.LogInformation($"Creating instance photo for \"{file}\"");
            
            return
                new LoggedPreviewablePhoto(
                    Logger,
                    _PhotoRepository.Create(file, errorGeneratingPreviewHandler)
                );            
        }
    }
}