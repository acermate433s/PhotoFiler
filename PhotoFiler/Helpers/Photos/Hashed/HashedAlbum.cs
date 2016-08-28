using PhotoFiler.Helpers.Hashed;
using PhotoFiler.Helpers.MD5;
using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoFiler.Helpers.Hashed
{
    public class HashedAlbum : IHashedAlbum
    {
        public IHashedPhotos Photos { get; private set; }

        public DirectoryInfo PreviewLocation { get; private set; }

        public HashedAlbum(
            string previewLocation,
            IHashedPhotos photos
        )
        {
            Photos = photos;

            PreviewLocation = new DirectoryInfo(previewLocation);
        }

        public int Count()
        {
            return Photos.All().Count();
        }

        public void GeneratePreviews()
        {
            var errors = new List<IHashedPhoto>();

            foreach (var photo in Photos.Values.Cast<IHashedPhoto>())
            {
                var previewer = new HashedPhotoPreviewer(photo, PreviewLocation);
                if (!previewer.Generate())
                    errors.Add(photo);
            }

            foreach (var error in errors)
                Photos.Remove(error.Hash);
        }

        public IEnumerable<IHashedPhoto> List(int page = 1, int count = 10)
        {
            return Photos.List(page, count);
        }

        public IHashedPhoto Photo(string hash)
        {
            return Photos[hash];
        }

        public byte[] Preview(string hash)
        {
            var photo = Photo(hash);
            var previewer = new HashedPhotoPreviewer((HashedPhoto) photo, PreviewLocation);
            return previewer.Preview();
        }

        public byte[] View(string hash)
        {
            var photo = Photo(hash);
            var previewer = new HashedPhotoPreviewer((HashedPhoto) photo, PreviewLocation);
            return previewer.View();
        }
    }
}