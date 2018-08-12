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

        public FileSystemPhotosRepository(
            DirectoryInfo roothPath,
            IPhotoRepository photoRepository,
            IExifReaderService exifReader
        )
        {
            this.rootPath = roothPath ?? throw new ArgumentNullException(nameof(roothPath));
            this.photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            this.exifReader = exifReader ?? throw new ArgumentNullException(nameof(exifReader));
        }

        public IPreviewablePhotos Create()
        {
            return
                new FileSystemPreviewablePhotos(
                    this.rootPath,
                    this.photoRepository,
                    this.exifReader
                );
        }
    }
}