using System;

using Microsoft.Extensions.Logging;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedPhoto : LoggedBase, IPhoto
    {
        private readonly IPhoto photo;

        public LoggedPhoto(
            ILogger logger,
            IPhoto photo
        ) : base(logger)
        {
            this.photo = photo ?? throw new ArgumentNullException(nameof(photo));

            this.Logger.LogInformation($"Photo \"{photo.Location}\" with hash \"{Hash}\"");
        }

        public DateTime? CreationDateTime
        {
            get
            {
                return this.photo.CreationDateTime;
            }
        }

        public string Location
        {
            get
            {
                return this.photo.Location;
            }
        }

        public Hash Hash
        {
            get
            {
                return this.photo.Hash;
            }
        }

        public string Name
        {
            get
            {
                return this.photo.Name;
            }
        }

        public string Resolution
        {
            get
            {
                return this.photo.Resolution;
            }
        }

        public int Width {
            get
            {
                return this.photo.Width;
            }
        }

        public int Height
        {
            get
            {
                return this.photo.Height;
            }
        }

        public string Size
        {
            get
            {
                return this.photo.Size;
            }
        }
    }
}