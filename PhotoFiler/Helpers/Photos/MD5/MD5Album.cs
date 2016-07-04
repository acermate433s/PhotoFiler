using PhotoFiler.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoFiler.Helpers.MD5
{
    public class MD5HashedAlbum : IHashedAlbum<MD5HashedPhoto>
    {
        public IHashedPhotos<MD5HashedPhoto> Photos { get; private set; }

        public DirectoryInfo PreviewLocation { get; private set; }

        public MD5HashedAlbum(
            string path,
            int hashLength,
            string previewLocation
        )
        {
            Photos = new MD5HashedPhotos(path, hashLength);
            PreviewLocation = new DirectoryInfo(previewLocation);
        }

        public int Count()
        {
            return Photos.All().Count();
        }

        public void GeneratePreviews()
        {
            foreach (var photo in Photos.Values)
            {
                var previewer = new MD5PhotoPreviewer(photo, PreviewLocation);
                previewer.Generate();
            }
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
            var previewer = new MD5PhotoPreviewer((MD5HashedPhoto) photo, PreviewLocation);
            return previewer.Preview();
        }

        public byte[] View(string hash)
        {
            var photo = Photo(hash);
            var previewer = new MD5PhotoPreviewer((MD5HashedPhoto) photo, PreviewLocation);
            return previewer.View();
        }
    }
}