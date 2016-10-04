using PhotoFiler.Models;
using System;
using System.IO;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedPhoto : LoggedBase, IHashedPhoto
    {
        IHashedPhoto _Photo;

        public LoggedHashedPhoto(
            ILogger logger, 
            IHashedPhoto photo
        ) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (photo == null)
                throw new ArgumentNullException(nameof(photo));

            _Photo = photo;

            Logger.Information($"Photo \"{photo.FileInfo.FullName}\" with hash \"{Hash}\"");
        }

        public DateTime? CreationDateTime
        {
            get
            {
                return _Photo.CreationDateTime;
            }
        }

        public FileInfo FileInfo
        {
            get
            {
                return _Photo.FileInfo;
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

        public string Size
        {
            get
            {
                return _Photo.Size;
            }
        }
    }
}