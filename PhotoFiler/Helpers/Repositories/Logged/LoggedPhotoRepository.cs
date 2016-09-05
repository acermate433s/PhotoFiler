using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoFiler.Models;
using PhotoFiler.Helpers.Photos.Logged;
using System.IO;
using Telemetry;

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

        public  IPreviewablePhoto Create(FileInfo file)
        {
            return
                new LoggedPreviewablePhoto(
                    _Logger,
                    _PhotoRepository.Create(file)
                );
        }
    }
}