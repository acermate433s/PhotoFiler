using PhotoFiler.Photo.Models;
using System;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemRepository : IRepository
    {
        protected IFileSystemConfiguration Configuration { get; set; }

        public FileSystemRepository(
            IFileSystemConfiguration configuration
        )
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }

        public IAlbumRepository CreateAlbumRepository()
        {
            return
                new FileSystemAlbumRepository(
                    Configuration.PreviewLocationDirectory
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            return 
                new FileSystemPhotoRepository(
                    Configuration.HashLength,
                    Configuration.HashingFunction,
                    Configuration.PreviewLocationDirectory
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            return
                new FileSystemPhotosRepository(
                    Configuration.RootPathDirectory,
                    CreatePhotoRepository()
                );
        }
    }
}