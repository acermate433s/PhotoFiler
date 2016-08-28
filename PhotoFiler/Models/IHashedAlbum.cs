using System.Collections.Generic;
using System.IO;

namespace PhotoFiler.Models
{
    public interface IHashedAlbum
    {
        IHashedPhotos Photos { get; }

        DirectoryInfo PreviewLocation { get; }

        int Count();

        void GeneratePreviews();

        IEnumerable<IHashedPhoto> List(int page = 1, int count = 10);

        IHashedPhoto Photo(string hash);

        byte[] Preview(string hash);

        byte[] View(string hash);
    }
}