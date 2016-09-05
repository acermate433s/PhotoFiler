using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedPhotosRepository : PhotosRepository
    {
        ILogger _Logger;

        public LoggedPhotosRepository(
            ILogger logger,
            DirectoryInfo roothPath, 
            IPhotoRepository photoRepository
        ) : base(roothPath, photoRepository)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _Logger = logger;
        }

        public new LoggedPreviewablePhotos Create()
        {
            return
                new LoggedPreviewablePhotos(
                    _Logger,
                    base.Create()
                );
        }
    }
}