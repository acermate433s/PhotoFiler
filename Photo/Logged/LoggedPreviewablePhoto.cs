using System;

using Microsoft.Extensions.Logging;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedPreviewablePhoto : LoggedBase, IPreviewablePhoto
    {
        public event ErrorGeneratingPreviewEventHandler ErrorGeneratingPreviewHandler;

        private readonly IPreviewablePhoto previewablePhoto;

        public LoggedPreviewablePhoto(
            ILogger logger, 
            IPreviewablePhoto previewablePhoto
        ) : base(logger)
        {
            this.previewablePhoto = previewablePhoto ?? throw new ArgumentNullException(nameof(previewablePhoto));

            this.previewablePhoto.ErrorGeneratingPreviewHandler +=
                (sender, args) =>
                {
                    this.Logger.LogError(
                        args.Exception, 
                        "Error generating preview from \"{0}\"", 
                        args.Photo.Location
                    );

                    ErrorGeneratingPreviewHandler?.Invoke(sender, args);
                };
        }

        public DateTime? CreationDateTime
        {
            get
            {
                return this.previewablePhoto.CreationDateTime;
            }
        }

        public string Location
        {
            get
            {
                return this.previewablePhoto.Location;
            }
        }

        public Hash Hash
        {
            get
            {
                return this.previewablePhoto.Hash;
            }
        }

        public string Name
        {
            get
            {
                return this.previewablePhoto.Name;
            }
        }

        public string Resolution
        {
            get
            {
                return this.previewablePhoto.Resolution;
            }
        }

        public int Width
        {
            get
            {
                return this.previewablePhoto.Width;
            }
        }

        public int Height
        {
            get
            {
                return this.previewablePhoto.Height;
            }
        }

        public string Size
        {
            get
            {
                return this.previewablePhoto.Size;
            }
        }

        public byte[] Preview()
        {
            this.Logger.LogInformation($"Generating preview for \"{Location}\" with hash \"{Hash}\".");
            var result = this.previewablePhoto.Preview();

            if (result == null)
            {
                this.Logger.LogWarning($"Cannot generate preview for photo \"{Location}\" with hash \"{Hash}\".");
            }
            else
            {
                this.Logger.LogInformation($"Preview size for \"{Location}\" with hash \"{Hash}\" is {result.Length} bytes.");
            }

            return result;
        }
        public byte[] View()
        {
            this.Logger.LogInformation($"Generating view for \"{Location}\" with hash \"{Hash}\".");
            var result = this.previewablePhoto.View();

            if (result == null)
            {
                this.Logger.LogWarning($"Cannot generate full view for photo \"{Location}\" with hash \"{Hash}\".");
            }
            else
            {
                this.Logger.LogInformation($"Full size for \"{Location}\" with hash \"{Hash}\" is {result.Length} bytes.");
            }

            return result;            
        }
    }
}