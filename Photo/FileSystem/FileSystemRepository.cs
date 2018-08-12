using System;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemRepository : IRepository
    {
        private readonly IFileSystemConfiguration configuration;
        private readonly IExifReaderService exifReader;
        private readonly IImageResizerService imageResizer;

        public FileSystemRepository(
            IFileSystemConfiguration configuration,
            IExifReaderService exifReader,
            IImageResizerService imageResizer
        )
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.exifReader = exifReader ?? throw new ArgumentNullException(nameof(exifReader));
            this.imageResizer = imageResizer ?? throw new ArgumentNullException(nameof(imageResizer));
        }

        public IAlbumRepository CreateAlbumRepository()
        {
            return
                new FileSystemAlbumRepository(
                    configuration.PreviewLocationDirectory
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            return 
                new FileSystemPhotoRepository(
                    configuration.HashingFunction,
                    configuration.PreviewLocationDirectory
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            return
                new FileSystemPhotosRepository(
                    this.configuration.RootPathDirectory,
                    CreatePhotoRepository(),
                    this.exifReader,
                    this.imageResizer
                );
        }
    }
}