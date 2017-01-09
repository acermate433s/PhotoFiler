using PhotoFiler.Models;
using System;
using System.IO;

namespace PhotoFiler.FileSystem
{
    public class FileSystemPhotosRepository : IPhotosRepository
    {
        DirectoryInfo _RootPath;
        IPhotoRepository _PhotoRepository;

        public FileSystemPhotosRepository(
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
                new FileSystemPreviewablePhotos(
                    _RootPath,
                    _PhotoRepository
                );
        }
    }
}