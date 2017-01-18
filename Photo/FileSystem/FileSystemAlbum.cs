using Photo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Photo.FileSystem
{
    public class FileSystemAlbum : IHashedAlbum
    {
        /// <summary>
        /// Photos in the album
        /// </summary>
        public IList<IPreviewablePhoto> Photos { get; private set; }

        /// <summary>
        /// Location of the directory where the generated preview of the photos are stored.
        /// </summary>
        public DirectoryInfo PreviewLocation { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemAlbum"/> class.
        /// </summary>
        /// <param name="previewLocation">The preview location.</param>
        /// <param name="photos">The photos.</param>
        /// <exception cref="System.ArgumentNullException">
        /// previewLocation
        /// or
        /// photos
        /// </exception>
        public FileSystemAlbum(
            DirectoryInfo previewLocation,
            List<IPreviewablePhoto> photos
        )
        {
            if (previewLocation == null)
                throw new ArgumentNullException(nameof(previewLocation));

            if (photos == null)
                throw new ArgumentNullException(nameof(photos));

            Photos = photos;
            PreviewLocation = previewLocation;
        }

        /// <summary>
        /// Number of photos in the album
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Photos.Count();
        }

        /// <summary>
        /// Generate previews of all the photos in the album.  
        /// </summary>
        /// <remarks>
        /// Creates a preview file of the photos if it doesn't exists.  If their is an error generating the preview the photo is removed from the album.
        /// </remarks>
        public void GeneratePreviews()
        {
            var errors = new List<string>();

            Photos
                .AsParallel()
                .ForAll(photo =>
                {
                    try
                    {
                        var filename = Path.Combine(PreviewLocation.FullName, photo.Hash);
                        filename = Path.ChangeExtension(filename, "prev");

                        if (!File.Exists(filename))
                        {
                            var preview = photo.Preview();
                            if (preview != null)
                                File.WriteAllBytes(filename, preview);
                            else
                                errors.Add(photo.Hash);

                        }
                    }
                    catch
                    {
                        errors.Add(photo.Hash);
                    }
                });

            foreach (var error in errors)
                Photos.Remove(Photos.First(item => item.Hash == error));
        }

        /// <summary>
        /// Retrieves a number of photos by page
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count">Maximum number of photos per page</param>
        /// <returns></returns>
        public IEnumerable<IPreviewablePhoto> List(int page = 1, int count = 10)
        {
            if (this.Count() > 0)
            {
                return
                    Photos
                        .Skip((page - 1) * count)
                        .Take(count)
                        .Select(item => item);
            }
            else
                return Enumerable.Empty<IPreviewablePhoto>();
        }

        /// <summary>
        /// Get the photo in the album using the hash
        /// </summary>
        /// <param name="hash">Hash generated for the photo</param>
        /// <returns></returns>
        public IPreviewablePhoto Photo(string hash)
        {
            return Photos.FirstOrDefault(item => item.Hash == hash);
        }

        /// <summary>
        /// Preview of the photo in the album
        /// </summary>
        /// <param name="hash">Hash of the photo</param>
        /// <remarks>
        /// Reads the generated preview if it exists; if not a preview file would be created.  If their is an error generated when creating a preview the photo is removed from the album and return null.
        /// </remarks>
        /// <returns>
        /// Byte array of the preview of the photo in the album.  Returns null if there is an error generating the preview file.
        /// </returns>
        public byte[] Preview(string hash)
        {
            var photo = Photos?.FirstOrDefault(item => item.Hash == hash);

            if (photo != null)
            {
                var previewFilename = Path.Combine(PreviewLocation.FullName, photo.Hash);
                previewFilename = Path.ChangeExtension(previewFilename, "prev");

                if (File.Exists(previewFilename))
                {
                    return File.ReadAllBytes(previewFilename);
                }
                else
                { 
                    var preview = photo.Preview();
                    if (preview != null)
                    {
                        File.WriteAllBytes(previewFilename, preview);
                        return preview;
                    }
                    else
                    {
                        Photos.Remove(Photos.First(item => item.Hash == hash));
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Full view of the photo in the album
        /// </summary>
        /// <param name="hash">Hash of the photo</param>
        /// <returns>
        /// Byte array of the full view of the photo in the album
        /// </returns>
        public byte[] View(string hash)
        {
            return
                Photos?
                    .FirstOrDefault(item => item.Hash == hash)?
                    .View();
        }
    }
}