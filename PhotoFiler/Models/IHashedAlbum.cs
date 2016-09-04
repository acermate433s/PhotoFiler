using System;
using System.Collections.Generic;
using System.IO;

namespace PhotoFiler.Models
{
    public interface IHashedAlbum
    {
        event EventHandler<IPreviewablePhoto> ErrorGeneratePreview;

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