using System;
using System.Collections.Generic;
using System.IO;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Models
{
    public interface IHashedAlbum
    {
        IList<IPreviewablePhoto> Photos { get; }

        DirectoryInfo PreviewLocation { get; }

        int Count();

        void GeneratePreviews();

        IEnumerable<IPreviewablePhoto> List(int page = 1, int count = 10);

        IPreviewablePhoto Photo(string hash);

        byte[] Preview(string hash);

        byte[] View(string hash);
    }
}