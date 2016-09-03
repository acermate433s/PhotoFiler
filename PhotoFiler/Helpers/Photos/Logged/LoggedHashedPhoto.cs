using PhotoFiler.Models;
using System;
using System.IO;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedPhoto : IHashedPhoto
    {
        ILogger _Logger;
        IHashedPhoto _Photo;

        public LoggedHashedPhoto(
            ILogger logger, 
            IHashedPhoto photo
        )
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (photo == null)
                throw new ArgumentNullException("photo");

            _Logger = logger;
            _Photo = photo;

            _Logger.Information($"Photo \"{photo.FileInfo.FullName}\" with hash \"{Hash}\"");
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