using PhotoFiler.Models;
using System;
using System.IO;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedPhotoPreviewer : IHashedPhotoPreviewer 
    {
        ILogger _Logger;
        IHashedPhotoPreviewer _HashedPhotoPreviewer;

        public LoggedHashedPhotoPreviewer(ILogger logger, IHashedPhotoPreviewer photoPreviewer)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (photoPreviewer == null)
                throw new ArgumentNullException("photoPreviewer");

            _Logger = logger;
            _HashedPhotoPreviewer = photoPreviewer;
        }

        public IHashedPhoto Photo
        {
            get
            {
                return _HashedPhotoPreviewer.Photo;
            }
            set
            {
                _Logger.Information($"Photo full name: \"{value.FileInfo.FullName}\"");
                _HashedPhotoPreviewer.Photo = value;
            }
        }
        public DirectoryInfo PreviewLocation
        {
            get
            {
                return _HashedPhotoPreviewer.PreviewLocation;
            }
            set
            {
                _Logger.Information($"Preview location is \"{value.FullName}\"");
                _HashedPhotoPreviewer.PreviewLocation = value;
            }
        }
        public bool Generate()
        {
            var result = _HashedPhotoPreviewer.Generate();
            if (!result)
                _Logger.Warning($"Cannot generate preview for photo \"{this.Photo.FileInfo.FullName}\"");

            return result;
        }
        public byte[] Preview()
        {
            var result = _HashedPhotoPreviewer.Preview();
            if(result == null)
                _Logger.Warning($"Cannot generate preview for photo \"{this.Photo.FileInfo.FullName}\"");
            else
                _Logger.Information($"Preview size for \"{this.Photo.FileInfo.FullName}\" is {result.Length} bytes");

            return result;
        }
        public byte[] View()
        {
            var result = _HashedPhotoPreviewer.View();
            if(result == null)
                _Logger.Warning($"Cannot generate full view for photo \"{this.Photo.FileInfo.FullName}\"");
            else
                _Logger.Information($"Full size for \"{this.Photo.FileInfo.FullName}\" is {result.Length} bytes");

            return result;
        }
    }
}