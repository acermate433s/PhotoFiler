using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedPreviewableHashedPhotos : IPreviewableHashedPhotos
    {
        ILogger _Logger;
        IPreviewableHashedPhotos _PreviewableHashedPhotos;

        public LoggedPreviewableHashedPhotos(
            ILogger logger,
            IPreviewableHashedPhotos previewableHashedPhotos
        )
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (previewableHashedPhotos == null)
                throw new ArgumentNullException("previewableHashedPhotos");

            _Logger = logger;
            _PreviewableHashedPhotos = previewableHashedPhotos;
        }

        public List<IPreviewableHashedPhoto> Retrieve()
        {
            var result =  _PreviewableHashedPhotos.Retrieve();

            if (result.Count() == 0)
                _Logger.Warning("No photos retrieved!");

            _Logger.Information($"Retrieved {result.Count()} photos");
            _Logger.Verbose(result.Select(item => item.FileInfo.FullName).ToArray());

            return result;
        }
    }
}