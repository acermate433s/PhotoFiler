using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedPreviewablePhotos : LoggedBase, IPreviewablePhotos
    {
        private readonly IPreviewablePhotos previewablePhotos;

        public LoggedPreviewablePhotos(
            ILogger logger,
            IPreviewablePhotos previewablePhotos
        ) : base(logger)
        {
            this.previewablePhotos = previewablePhotos ?? throw new ArgumentNullException(nameof(previewablePhotos));
        }

        public List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            this.Logger.LogInformation($"Retrieving photos.");            
            var result = this.previewablePhotos.Retrieve(errorGeneratingPreviewHandler);

            if (result.Count() == 0)
            {
                this.Logger.LogWarning("No photos retrieved!");
            }
            else
            {
                this.Logger.LogInformation($"Retrieved {result.Count()} photos.");
                this.Logger.LogTrace(String.Join(Environment.NewLine, result.Select(item => item.Location).ToArray()));
            }

            return result;
        }
    }
}