using System;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemRepository : IRepository
    {
        private readonly IFileSystemConfiguration configuration;
        private readonly IExifReaderService exifReader;

        public FileSystemRepository(
            IFileSystemConfiguration configuration,
            IExifReaderService exifReader
        )
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.exifReader = exifReader ?? throw new ArgumentNullException(nameof(exifReader));
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
                    configuration.HashLength,
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
                    this.exifReader
                );
        }
    }
}