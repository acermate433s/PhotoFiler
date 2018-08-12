using System;
using System.IO;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemPhotosRepository : IPhotosRepository
    {
        private readonly DirectoryInfo rootPath;
        private readonly IPhotoRepository photoRepository;
        private readonly IExifReaderService exifReader;
        private readonly IImageResizerService imageResizer;

        public FileSystemPhotosRepository(
            DirectoryInfo roothPath,
            IPhotoRepository photoRepository,
            IExifReaderService exifReader,
            IImageResizerService imageResizer
        )
        {
            this.rootPath = roothPath ?? throw new ArgumentNullException(nameof(roothPath));
            this.photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            this.exifReader = exifReader ?? throw new ArgumentNullException(nameof(exifReader));
            this.imageResizer = imageResizer ?? throw new ArgumentNullException(nameof(imageResizer));
        }

        public IPreviewablePhotos Create()
        {
            return
                new FileSystemPreviewablePhotos(
                    this.rootPath,
                    this.photoRepository,
                    this.exifReader,
                    this.imageResizer
                );
        }
    }
}