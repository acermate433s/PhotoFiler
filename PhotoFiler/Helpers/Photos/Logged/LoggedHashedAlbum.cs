using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedAlbum : IHashedAlbum
    {
        Telemetry.ILogger _Logger;
        IHashedAlbum _HashedAlbum;

        public event EventHandler<IPreviewablePhoto> ErrorGeneratePreview;

        public LoggedHashedAlbum(
            Telemetry.ILogger logger, 
            IHashedAlbum album
        )
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (album == null)
                throw new ArgumentNullException(nameof(album));

            _Logger = logger;
            _HashedAlbum = album;
            _HashedAlbum.ErrorGeneratePreview +=
                (sender, e) =>
                {
                    _Logger.Warning($"Error generating preview for photo \"{e.FileInfo.FullName}\" with hash \"{e.Hash}\" in album.");

                    ErrorGeneratePreview?.Invoke(sender, e);
                };
                
            logger.Verbose(album.Photos?.Select(item => $"\"{item.FileInfo.FullName}\" ({item.Hash}).").ToArray());
        }

        public IList<IPreviewablePhoto> Photos
        {
            get
            {
                return _HashedAlbum.Photos;
            }
        }

        public DirectoryInfo PreviewLocation
        {
            get
            {
                return _HashedAlbum.PreviewLocation;
            }
        }

        public int Count()
        {
            return _HashedAlbum.Count();
        }

        public void GeneratePreviews()
        {
            using (var logger = _Logger.Create($"Generating photo previews of {_HashedAlbum.Count()} photos in \"{_HashedAlbum.PreviewLocation.FullName}\" for album."))
            {
                _HashedAlbum.GeneratePreviews();
            }
        }

        public IEnumerable<IPreviewablePhoto> List(int page = 1, int count = 10)
        {
            _Logger.Information("Generate list of photos in album.");            
            var result = _HashedAlbum.List(page, count);

            if (result != null)
                _Logger.Information($"Retrieved page {page} expecting {count} photos but retrieved {result.Count()}.");
            else
                _Logger.Warning($"No photos where retrieved.");

            return result;
        }

        public IPreviewablePhoto Photo(string hash)
        {
            _Logger.Information($"Retrieving photo with hash \"{hash}\" in album.");            
            var result = _HashedAlbum.Photo(hash);

            if (result == null)
                _Logger.Warning($"Cannot find photo for hash \"{hash}\".");

            return result;            
        }

        public byte[] Preview(string hash)
        {
            _Logger.Information($"Generate preview for \"{hash}\" in album");
            
            var photo = Photos.FirstOrDefault(item => item.Hash == hash);
            if (photo != null)
            {
                var result = photo.Preview();
                if (photo.FileInfo != null)
                {
                    if (result != null)
                        _Logger.Information($"Preview size for \"{photo.FileInfo.FullName}\" with hash \"{hash}\" is {result.Length} bytes.");
                    else
                        _Logger.Warning($"Cannot generate preview for \"{photo.FileInfo.FullName} with hash \"{hash}\".");
                }
                else
                    _Logger.Warning($"Cannot find photo with hash \"{hash}\".");

                return result;
            }
            else
            {
                _Logger.Warning("No photos can be found in the album.");
                return null;
            }
        }

        public byte[] View(string hash)
        {
            _Logger.Information($"Generate view for \"{hash}\"  in album.");
            
            var photo = Photos.FirstOrDefault(item => item.Hash == hash);
            if (photo != null)
            {
                var result = photo.View();
                if (photo.FileInfo != null)
                {
                    if (result != null)
                        _Logger.Information($"Full size for \"{photo.FileInfo.FullName}\" with hash \"{hash}\" is {result.Length} bytes.");
                    else
                        _Logger.Warning($"Cannot generate view for \"{photo.FileInfo.FullName} with hash \"{hash}\".");
                }
                else
                    _Logger.Warning($"Cannot find photo with hash \"{hash}\".");

                return result;
            }
            else
            {
                _Logger.Warning("No photos can be found in the album.");
                return null;
            }
            
        }
    }
}