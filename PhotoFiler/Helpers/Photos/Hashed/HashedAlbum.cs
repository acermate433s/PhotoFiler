using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoFiler.Helpers.Hashed
{
    public class HashedAlbum : IHashedAlbum
    {
        Dictionary<string, IHashedPhotoPreviewer> _HashedPhotoPreviewer = new Dictionary<string, IHashedPhotoPreviewer>();

        public IHashedPhotos Photos { get; private set; }

        public DirectoryInfo PreviewLocation { get; private set; }

        public HashedAlbum(
            string previewLocation,
            IHashedPhotos photos,
            Func<IHashedPhoto, DirectoryInfo, IHashedPhotoPreviewer> initiator
        )
        {
            Photos = photos;
            PreviewLocation = new DirectoryInfo(previewLocation);

            foreach(var photo in photos)
                _HashedPhotoPreviewer.Add(
                    photo.Key,
                    initiator.Invoke(photo.Value, PreviewLocation)
                );
        }

        public int Count()
        {
            return Photos.All().Count();
        }

        public void GeneratePreviews()
        {
            var errors = new List<string>();

            foreach (var previewer in _HashedPhotoPreviewer)
            {
                if (!previewer.Value.Generate())
                    errors.Add(previewer.Key);
            }

            foreach (var error in errors)
                Photos.Remove(error);
        }

        public IEnumerable<IHashedPhoto> List(int page = 1, int count = 10)
        {
            return Photos.List(page, count);
        }

        public IHashedPhoto Photo(string hash)
        {
            if (Photos.ContainsKey(hash))
                return Photos[hash];
            else
                return null;
        }

        public byte[] Preview(string hash)
        {
            var photo = Photo(hash);
            var previewer = _HashedPhotoPreviewer[hash];
            return previewer.Preview();
        }

        public byte[] View(string hash)
        {
            var photo = Photo(hash);
            var previewer = _HashedPhotoPreviewer[hash];
            return previewer.View();
        }
    }
}