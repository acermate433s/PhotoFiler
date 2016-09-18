using PhotoFiler.Models;
using System;
using System.IO;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedPreviewablePhoto : IPreviewablePhoto
    {
        ILogger _Logger;
        IPreviewablePhoto _PreviewableHashedPhoto;

        public LoggedPreviewablePhoto(
            ILogger logger, 
            IPreviewablePhoto photo
        ) 
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (photo == null)
                throw new ArgumentNullException(nameof(photo));

            _Logger = logger;
            _PreviewableHashedPhoto = photo;
        }

        public DateTime? CreationDateTime
        {
            get
            {
                return _PreviewableHashedPhoto.CreationDateTime;
            }
        }

        public FileInfo FileInfo
        {
            get
            {
                return _PreviewableHashedPhoto.FileInfo;
            }
        }

        public string Hash
        {
            get
            {
                return _PreviewableHashedPhoto.Hash;
            }
        }

        public string Name
        {
            get
            {
                return _PreviewableHashedPhoto.Name;
            }
        }

        public string Resolution
        {
            get
            {
                return _PreviewableHashedPhoto.Resolution;
            }
        }

        public string Size
        {
            get
            {
                return _PreviewableHashedPhoto.Size;
            }
        }

        public byte[] Preview()
        {
            _Logger.Information($"Generating preview for \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            var result = _PreviewableHashedPhoto.Preview();

            if (result == null)
                _Logger.Warning($"Cannot generate preview for photo \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            else
                _Logger.Information($"Preview size for \"{FileInfo.FullName}\" with hash \"{Hash}\" is {result.Length} bytes.");

            return result;
        }
        public byte[] View()
        {
            _Logger.Information($"Generating view for \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            var result = _PreviewableHashedPhoto.View();

            if (result == null)
                _Logger.Warning($"Cannot generate full view for photo \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            else
                _Logger.Information($"Full size for \"{FileInfo.FullName}\" with hash \"{Hash}\" is {result.Length} bytes.");

            return result;
            
        }
    }
}