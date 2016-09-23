using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telemetry;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedPreviewablePhotos : IPreviewablePhotos
    {
        ILogger _Logger;
        IPreviewablePhotos _PreviewablePhotos;

        public LoggedPreviewablePhotos(
            ILogger logger,
            IPreviewablePhotos previewablePhotos
        )
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (previewablePhotos == null)
                throw new ArgumentNullException(nameof(previewablePhotos));

            _Logger = logger;
            _PreviewablePhotos = previewablePhotos;
        }

        public List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreview errorGeneratingPreviewHandler = null)
        {
            _Logger.Information($"Retrieving photos.");            
            var result = _PreviewablePhotos.Retrieve(errorGeneratingPreviewHandler);

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