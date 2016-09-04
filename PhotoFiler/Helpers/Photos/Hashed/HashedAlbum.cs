using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoFiler.Helpers.Photos.Hashed
{
    public class HashedAlbum : IHashedAlbum
    {
        public event EventHandler<IPreviewableHashedPhoto> ErrorGeneratePreview;

        public IList<IPreviewableHashedPhoto> Photos { get; private set; }

        public DirectoryInfo PreviewLocation { get; private set; }

        public HashedAlbum(
            string previewLocation,
            List<IPreviewableHashedPhoto> photos
        )
        {
            if (previewLocation == null)
                throw new ArgumentNullException(nameof(previewLocation));

            if (photos == null)
                throw new ArgumentNullException(nameof(photos));

            Photos = photos;
            PreviewLocation = new DirectoryInfo(previewLocation);
        }

        public int Count()
        {
            return Photos.Count();
        }

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
                                ErrorGeneratePreview?.Invoke(this, photo);
                        }
                    }
                    catch
                    {
                        ErrorGeneratePreview?.Invoke(this, photo);

                        errors.Add(photo.Hash);
                    }
                });

            foreach (var error in errors)
                Photos.Remove(Photos.First(item => item.Hash == error));
        }

        public IEnumerable<IPreviewableHashedPhoto> List(int page = 1, int count = 10)
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
                return Enumerable.Empty<IPreviewableHashedPhoto>();
        }

        public IPreviewableHashedPhoto Photo(string hash)
        {
            return Photos.FirstOrDefault(item => item.Hash == hash);
        }

        public byte[] Preview(string hash)
        {
            return 
                Photos?
                    .FirstOrDefault(item => item.Hash == hash)?
                    .Preview();
        }

        public byte[] View(string hash)
        {
            return
                Photos?
                    .FirstOrDefault(item => item.Hash == hash)?
                    .View();
        }
    }
}