using System;
using System.Collections.Generic;
using System.IO;

namespace PhotoFiler.Models
{
    public interface IHashedAlbum
    {
        event EventHandler<IPreviewableHashedPhoto> ErrorGeneratePreview;

        IList<IPreviewableHashedPhoto> Photos { get; }

        DirectoryInfo PreviewLocation { get; }

        int Count();

        void GeneratePreviews();

        IEnumerable<IPreviewableHashedPhoto> List(int page = 1, int count = 10);

        IPreviewableHashedPhoto Photo(string hash);

        byte[] Preview(string hash);

        byte[] View(string hash);
    }
}