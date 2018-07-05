﻿using Photo.Models;
using System;
using System.IO;
using Telemetry;
using static Photo.Helpers.Helpers;

namespace Photo.Logged
{
    public class LoggedPreviewablePhoto : LoggedBase, IPreviewablePhoto
    {
        public event ErrorGeneratingPreviewEventHandler ErrorGeneratingPreviewHandler;

        IPreviewablePhoto _PreviewablePhoto;

        public LoggedPreviewablePhoto(
            ILogger logger, 
            IPreviewablePhoto previewablePhoto
        ) : base(logger)
        {
            if(logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _PreviewablePhoto = previewablePhoto ?? throw new ArgumentNullException(nameof(previewablePhoto));

            _PreviewablePhoto.ErrorGeneratingPreviewHandler +=
                (sender, args) =>
                {
                    Logger.Error(
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
                return _PreviewablePhoto.CreationDateTime;
            }
        }

        public string Location
        {
            get
            {
                return _PreviewablePhoto.Location;
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

        public int Width
        {
            get
            {
                return _PreviewablePhoto.Width;
            }
        }

        public int Height
        {
            get
            {
                return _PreviewablePhoto.Height;
            }
        }

        public string Size
        {
            get
            {
                return _PreviewablePhoto.Size;
            }
        }

        public byte[] Preview
        {
            get
            {
                Logger.Information($"Generating preview for \"{Location}\" with hash \"{Hash}\".");
                var result = _PreviewablePhoto.Preview;

                if(result == null)
                    Logger.Warning($"Cannot generate preview for photo \"{Location}\" with hash \"{Hash}\".");
                else
                    Logger.Information($"Preview size for \"{Location}\" with hash \"{Hash}\" is {result.Length} bytes.");

                return result;
            }
        }
        public byte[] View
        {
            get
            {
                Logger.Information($"Generating view for \"{Location}\" with hash \"{Hash}\".");
                var result = _PreviewablePhoto.View;

                if(result == null)
                    Logger.Warning($"Cannot generate full view for photo \"{Location}\" with hash \"{Hash}\".");
                else
                    Logger.Information($"Full size for \"{Location}\" with hash \"{Hash}\" is {result.Length} bytes.");

                return result;
            }
        }
    }
}