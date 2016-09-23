using PhotoFiler.Models;
using System;
using System.IO;
using Telemetry;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedPreviewablePhoto : IPreviewablePhoto
    {
        public event ErrorGeneratingPreview ErrorGeneratingPreviewHandler;

        ILogger _Logger;
        IPreviewablePhoto _PreviewablePhoto;

        public LoggedPreviewablePhoto(
            ILogger logger, 
            IPreviewablePhoto previewablePhoto
        ) 
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (previewablePhoto == null)
                throw new ArgumentNullException(nameof(previewablePhoto));

            _Logger = logger;
            _PreviewablePhoto = previewablePhoto;

            _PreviewablePhoto.ErrorGeneratingPreviewHandler +=
                (photo, exception) =>
                {
                    _Logger.Error(
                        exception, 
                        "Error generating preview from \"{0}\"", 
                        photo.FileInfo.FullName
                    );

                    ErrorGeneratingPreviewHandler?.Invoke(photo, exception);
                };
        }

        public DateTime? CreationDateTime
        {
            get
            {
                return _PreviewablePhoto.CreationDateTime;
            }
        }

        public FileInfo FileInfo
        {
            get
            {
                return _PreviewablePhoto.FileInfo;
            }
        }

        public string Hash
        {
            get
            {
                return _PreviewablePhoto.Hash;
            }
        }

        public string Name
        {
            get
            {
                return _PreviewablePhoto.Name;
            }
        }

        public string Resolution
        {
            get
            {
                return _PreviewablePhoto.Resolution;
            }
        }

        public string Size
        {
            get
            {
                return _PreviewablePhoto.Size;
            }
        }

        public byte[] Preview()
        {
            _Logger.Information($"Generating preview for \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            var result = _PreviewablePhoto.Preview();

            if (result == null)
                _Logger.Warning($"Cannot generate preview for photo \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            else
                _Logger.Information($"Preview size for \"{FileInfo.FullName}\" with hash \"{Hash}\" is {result.Length} bytes.");

            return result;
        }
        public byte[] View()
        {
            _Logger.Information($"Generating view for \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            var result = _PreviewablePhoto.View();

            if (result == null)
                _Logger.Warning($"Cannot generate full view for photo \"{FileInfo.FullName}\" with hash \"{Hash}\".");
            else
                _Logger.Information($"Full size for \"{FileInfo.FullName}\" with hash \"{Hash}\" is {result.Length} bytes.");

            return result;
            
        }
    }
}