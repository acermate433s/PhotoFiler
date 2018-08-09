using Microsoft.Extensions.Logging;
using PhotoFiler.Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedPreviewablePhotos : LoggedBase, IPreviewablePhotos
    {
        IPreviewablePhotos _PreviewablePhotos;

        public LoggedPreviewablePhotos(
            ILogger logger,
            IPreviewablePhotos previewablePhotos
        ) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (previewablePhotos == null)
                throw new ArgumentNullException(nameof(previewablePhotos));

            _PreviewablePhotos = previewablePhotos;
        }

        public List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            Logger.LogInformation($"Retrieving photos.");            
            var result = _PreviewablePhotos.Retrieve(errorGeneratingPreviewHandler);

            if (result.Count() == 0)
            {
                Logger.LogWarning("No photos retrieved!");
            }
            else
            {
                Logger.LogInformation($"Retrieved {result.Count()} photos.");
                Logger.LogTrace(String.Join(Environment.NewLine, result.Select(item => item.Location).ToArray()));
            }

            return result;
        }
    }
}