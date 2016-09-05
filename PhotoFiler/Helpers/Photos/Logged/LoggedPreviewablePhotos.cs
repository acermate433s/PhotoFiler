using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedPreviewablePhotos : IPreviewablePhotos
    {
        ILogger _Logger;
        IPreviewablePhotos _PreviewableHashedPhotos;

        public LoggedPreviewablePhotos(
            ILogger logger,
            IPreviewablePhotos previewableHashedPhotos
        )
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (previewableHashedPhotos == null)
                throw new ArgumentNullException(nameof(previewableHashedPhotos));

            _Logger = logger;
            _PreviewableHashedPhotos = previewableHashedPhotos;
        }

        public List<IPreviewablePhoto> Retrieve()
        {
            using (var logger = _Logger.Create("Retrieving photos."))
            {
                var result = _PreviewableHashedPhotos.Retrieve();

                if (result.Count() == 0)
                {
                    _Logger.Warning("No photos retrieved!");
                }
                else
                {
                    _Logger.Information($"Retrieved {result.Count()} photos.");
                    _Logger.Verbose(result.Select(item => item.FileInfo.FullName).ToArray());
                }

                return result;
            }
        }
    }
}