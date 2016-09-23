using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.IO;
using Telemetry;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedPhotoRepository : IPhotoRepository
    {
        ILogger _Logger;
        IPhotoRepository _PhotoRepository;

        public LoggedPhotoRepository(
            ILogger logger,
            IPhotoRepository photoRepository
        ) 
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (logger == null)
                throw new ArgumentNullException(nameof(photoRepository));

            _Logger = logger;
            _PhotoRepository = photoRepository;
        }

        public  IPreviewablePhoto Create(
            FileInfo file, 
            ErrorGeneratingPreview errorGeneratingPreviewHandler = null
        )
        {
            _Logger.Information($"Creating instance photo for \"{file}\"");
            
            return
                new LoggedPreviewablePhoto(
                    _Logger,
                    _PhotoRepository.Create(file, errorGeneratingPreviewHandler)
                );            
        }
    }
}