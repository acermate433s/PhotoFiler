using PhotoFiler.Helpers.Photos;
using PhotoFiler.Models;
using System;
using System.IO;

namespace PhotoFiler.Helpers.Repositories
{
    public class PhotosRepository : IPhotosRepository
    {
        DirectoryInfo _RootPath;
        IPhotoRepository _PhotoRepository;

        public PhotosRepository(
            DirectoryInfo roothPath,
            IPhotoRepository photoRepository
        )
        {
            if (roothPath == null)
                throw new ArgumentNullException(nameof(roothPath));

            if (photoRepository == null)
                throw new ArgumentNullException(nameof(photoRepository));

            _RootPath = roothPath;
            _PhotoRepository = photoRepository;
        }

        public IPreviewablePhotos Create()
        {
            return
                new PreviewablePhotos(
                    _RootPath,
                    _PhotoRepository
                );
        }
    }
}