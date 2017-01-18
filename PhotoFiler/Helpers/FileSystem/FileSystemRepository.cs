using PhotoFiler.Models;
using System;

namespace PhotoFiler.FileSystem
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
                    Configuration.PreviewLocation
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            return 
                new FileSystemPhotoRepository(
                    Configuration.HashLength,
                    Configuration.HashingFunction,
                    Configuration.PreviewLocation
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            return
                new FileSystemPhotosRepository(
                    Configuration.RootPath,
                    CreatePhotoRepository()
                );
        }
    }
}