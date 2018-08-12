using System;
using System.IO;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemPhotoRepository : IPhotoRepository
    {
        private readonly int hashLength = 0;
        private readonly IHashFunction hashingFunction = null;
        private readonly DirectoryInfo previewLocation = null;

        public FileSystemPhotoRepository(
            int hashLength,
            IHashFunction hashingFunction,
            DirectoryInfo previewLocation
        )
        {
            if (hashLength < 0) throw new ArgumentOutOfRangeException(nameof(hashLength), "Hash length must be greater than zero.");

            this.hashLength = hashLength;
            this.hashingFunction = hashingFunction ?? throw new ArgumentNullException(nameof(hashingFunction));
            this.previewLocation = previewLocation ?? throw new ArgumentNullException(nameof(previewLocation));
        }

        public IPreviewablePhoto Create(
            FileInfo file,
            IExifReaderService exifReader,
            ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null            
        )
        {
            if (exifReader == null) throw new ArgumentNullException(nameof(exifReader));

            var result =
                new FileSystemPreviewablePhoto(
                    hashLength,
                    file.FullName,
                    hashingFunction,
                    exifReader
                );

            result.ErrorGeneratingPreviewHandler += errorGeneratingPreviewHandler;

            return result;
                
        }
    }
}