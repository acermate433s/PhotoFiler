using Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telemetry;
using static Photo.Helpers.Helpers;

namespace Photo.Logged
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

        public List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreview errorGeneratingPreviewHandler = null)
        {
            Logger.Information($"Retrieving photos.");            
            var result = _PreviewablePhotos.Retrieve(errorGeneratingPreviewHandler);

            if (result.Count() == 0)
            {
                Logger.Warning("No photos retrieved!");
            }
            else
            {
                Logger.Information($"Retrieved {result.Count()} photos.");
                Logger.Verbose(result.Select(item => item.Location).ToArray());
            }

            return result;
        }
    }
}