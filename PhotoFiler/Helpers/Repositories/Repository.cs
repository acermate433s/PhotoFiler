using PhotoFiler.Models;
using System;

namespace PhotoFiler.Helpers.Repositories
{
    public class Repository : IRepository
    {
        protected IConfiguration Configuration { get; set; }

        public Repository(
            IConfiguration configuration
        )
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }

        public IAlbumRepository CreateAlbumRepository()
        {
            return
                new AlbumRepository(
                    Configuration.PreviewLocation
                );
        }

        public IPhotoRepository CreatePhotoRepository()
        {
            return 
                new PhotoRepository(
                    Configuration.HashLength,
                    Configuration.HashingFunction,
                    Configuration.PreviewLocation
                );
        }

        public IPhotosRepository CreatePhotosRepository()
        {
            return
                new PhotosRepository(
                    Configuration.RootPath,
                    CreatePhotoRepository()
                );
        }
    }
}