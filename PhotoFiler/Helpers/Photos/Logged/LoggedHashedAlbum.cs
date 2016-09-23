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
        ILogger _Logger;
        IHashedAlbum _HashedAlbum;

        public LoggedHashedAlbum(
            ILogger logger, 
            IHashedAlbum album
        )
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (album == null)
                throw new ArgumentNullException(nameof(album));

            _Logger = logger;
            _HashedAlbum = album;

            _Logger.Verbose(album.Photos?.Select(item => $"\"{item.FileInfo.FullName}\" ({item.Hash}).").ToArray());
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
            using (var scope = _Logger.CreateScope($"Generating photo previews of {_HashedAlbum.Count()} photos in \"{_HashedAlbum.PreviewLocation.FullName}\" for album."))
            {
                _HashedAlbum.GeneratePreviews();
            }
        }

        public IEnumerable<IPreviewablePhoto> List(int page = 1, int count = 10)
        {
            using (var scope = _Logger.CreateScope("Generate list of photos in album."))
            {
                var result = _HashedAlbum.List(page, count);

                if (result != null)
                    scope.Information($"Retrieved page {page} expecting {count} photos but retrieved {result.Count()}.");
                else
                    scope.Warning($"No photos where retrieved.");

                return result;
            }
        }

        public IPreviewablePhoto Photo(string hash)
        {
            using (var scope = _Logger.CreateScope($"Get photo with hash \"{hash}\"."))
            {
                var result = _HashedAlbum.Photo(hash);

                if (result == null)
                    scope.Warning($"Cannot find photo for hash \"{hash}\".");

                return result;
            }
        }

        public byte[] Preview(string hash)
        {
            using (var scope = _Logger.CreateScope($"Generate preview for photo with \"{hash}\" in album."))
            {
                var photo = Photos.FirstOrDefault(item => item.Hash == hash);
                if (photo != null)
                {
                    var result = photo.Preview();
                    if (photo.FileInfo != null)
                    {
                        if (result != null)
                            scope.Information($"Preview size for \"{photo.FileInfo.FullName}\" with hash \"{hash}\" is {result.Length} bytes.");
                        else
                            scope.Warning($"Cannot generate preview for \"{photo.FileInfo.FullName} with hash \"{hash}\".");
                    }
                    else
                        scope.Warning($"Cannot find photo with hash \"{hash}\".");

                    return result;
                }
                else
                {
                    scope.Warning("No photos can be found in the album.");
                    return null;
                }
            }
        }

        public byte[] View(string hash)
        {
            using (var scope = _Logger.CreateScope($"Generate full view for photo with \"{hash}\" in album."))
            {
                var photo = Photos.FirstOrDefault(item => item.Hash == hash);
                if (photo != null)
                {
                    var result = photo.View();
                    if (photo.FileInfo != null)
                    {
                        if (result != null)
                            scope.Information($"Full size for \"{photo.FileInfo.FullName}\" with hash \"{hash}\" is {result.Length} bytes.");
                        else
                            scope.Warning($"Cannot generate view for \"{photo.FileInfo.FullName} with hash \"{hash}\".");
                    }
                    else
                        scope.Warning($"Cannot find photo with hash \"{hash}\".");

                    return result;
                }
                else
                {
                    scope.Warning("No photos can be found in the album.");
                    return null;
                }
            }
        }
    }
}