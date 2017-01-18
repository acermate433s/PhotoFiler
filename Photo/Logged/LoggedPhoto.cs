using Photo.Models;
using System;
using System.IO;
using Telemetry;

namespace Photo.Logged
{
    public class LoggedPhoto : LoggedBase, IPhoto
    {
        IPhoto _Photo;

        public LoggedPhoto(
            ILogger logger,
            IPhoto photo
        ) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (photo == null)
                throw new ArgumentNullException(nameof(photo));

            _Photo = photo;

            Logger.Information($"Photo \"{photo.Location}\" with hash \"{Hash}\"");
        }

        public DateTime? CreationDateTime
        {
            get
            {
                return _Photo.CreationDateTime;
            }
        }

        public string Location
        {
            get
            {
                return _Photo.Location;
            }
        }

        public string Hash
        {
            get
            {
                return _Photo.Hash;
            }
        }

        public string Name
        {
            get
            {
                return _Photo.Name;
            }
        }

        public string Resolution
        {
            get
            {
                return _Photo.Resolution;
            }
        }

        public int Width {
            get
            {
                return _Photo.Width;
            }
        }

        public int Height
        {
            get
            {
                return _Photo.Height;
            }
        }

        public string Size
        {
            get
            {
                return _Photo.Size;
            }
        }
    }
}