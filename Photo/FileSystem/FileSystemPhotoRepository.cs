using System;
using System.IO;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemPhotoRepository : IPhotoRepository
    {
        private readonly IHashFunction hashingFunction = null;
        private readonly DirectoryInfo previewLocation = null;

        public FileSystemPhotoRepository(
            IHashFunction hashingFunction,
            DirectoryInfo previewLocation
        )
        {
            this.hashingFunction = hashingFunction ?? throw new ArgumentNullException(nameof(hashingFunction));
            this.previewLocation = previewLocation ?? throw new ArgumentNullException(nameof(previewLocation));
        }

        public IPreviewablePhoto Create(
            FileInfo file,
            IExifReaderService exifReader,
            IImageResizerService imageResizer,
            ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null            
        )
        {
            if (exifReader == null) throw new ArgumentNullException(nameof(exifReader));

            var result =
                new FileSystemPreviewablePhoto(
                    file.FullName,
                    hashingFunction,
                    exifReader,
                    imageResizer
                );

            result.ErrorGeneratingPreviewHandler += errorGeneratingPreviewHandler;

            return result;
                
        }
    }
}