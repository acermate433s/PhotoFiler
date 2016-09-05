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
    public class LoggedPhotoRepository : PhotoRepository
    {
        ILogger _Logger;

        public LoggedPhotoRepository(
            ILogger logger,
            int hashLength,
            IHasher hashingFunction,
            DirectoryInfo previewLocation
        ) : base(hashLength, hashingFunction, previewLocation)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _Logger = logger;
        }

        public new LoggedPreviewablePhoto Create(FileInfo file)
        {
            return
                new LoggedPreviewablePhoto(
                    _Logger,
                    base.Create(file)
                );
        }
    }
}