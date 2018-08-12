using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Logging;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedAlbum : LoggedBase, IHashedAlbum
    {
        private readonly IHashedAlbum hashedAlbum;

        public LoggedAlbum(
            ILogger logger,
            IHashedAlbum album
        ) : base(logger)
        {
            hashedAlbum = album ?? throw new ArgumentNullException(nameof(album));

            this.Logger.LogInformation(String.Join(Environment.NewLine, album.Photos?.Select(item => $"\"{item.Location}\" ({item.Hash}).").ToArray()));
        }

        public IList<IPreviewablePhoto> Photos
        {
            get
            {
                return this.hashedAlbum.Photos;
            }
        }

        public DirectoryInfo PreviewLocation
        {
            get
            {
                return this.hashedAlbum.PreviewLocation;
            }
        }

        public int Count()
        {
            return this.hashedAlbum.Count();
        }

        public void GeneratePreviews()
        {
            using (var scope = Logger.BeginScope($"Generating photo previews of {hashedAlbum.Count()} photos in \"{hashedAlbum.PreviewLocation.FullName}\" for album."))
            {
                this.hashedAlbum.GeneratePreviews();
            }
        }

        public IEnumerable<IPreviewablePhoto> List(int page = 1, int count = 10)
        {
            using (var scope = this.Logger.BeginScope("Generate list of photos in album."))
            {
                var result = this.hashedAlbum.List(page, count);

                if (result != null)
                    this.Logger.LogInformation($"Retrieved page {page} expecting {count} photos but retrieved {result.Count()}.");
                else
                    this.Logger.LogWarning($"No photos where retrieved.");

                return result;
            }
        }

        public IPreviewablePhoto Photo(string hash)
        {
            using (var scope = this.Logger.BeginScope($"Get photo with hash \"{hash}\"."))
            {
                var result = this.hashedAlbum.Photo(hash);

                if (result == null)
                    this.Logger.LogWarning($"Cannot find photo for hash \"{hash}\".");

                return result;
            }
        }

        public byte[] Preview(string hash)
        {
            using (var scope = this.Logger.BeginScope($"Generate preview for photo with \"{hash}\" in album."))
            {
                var photo = this.Photos.FirstOrDefault(item => item.Hash == hash);
                if (photo != null)
                {
                    var result = photo.Preview();
                    if (!String.IsNullOrEmpty(photo.Location))
                    {
                        if (result != null)
                            this.Logger.LogInformation($"Preview size for \"{photo.Location}\" with hash \"{hash}\" is {result.Length} bytes.");
                        else
                            this.Logger.LogWarning($"Cannot generate preview for \"{photo.Location} with hash \"{hash}\".");
                    }
                    else
                        this.Logger.LogWarning($"Cannot find photo with hash \"{hash}\".");

                    return result;
                }
                else
                {
                    this.Logger.LogWarning("No photos can be found in the album.");
                    return null;
                }
            }
        }

        public byte[] View(string hash)
        {
            using (var scope = Logger.BeginScope($"Generate full view for photo with \"{hash}\" in album."))
            {
                var photo = Photos.FirstOrDefault(item => item.Hash == hash);
                if (photo != null)
                {
                    var result = photo.View();
                    if (!String.IsNullOrEmpty(photo.Location))
                    {
                        if (result != null)
                            Logger.LogInformation($"Full size for \"{photo.Location}\" with hash \"{hash}\" is {result.Length} bytes.");
                        else
                            Logger.LogWarning($"Cannot generate view for \"{photo.Location} with hash \"{hash}\".");
                    }
                    else
                        Logger.LogWarning($"Cannot find photo with hash \"{hash}\".");

                    return result;
                }
                else
                {
                    Logger.LogWarning("No photos can be found in the album.");
                    return null;
                }
            }
        }
    }
}