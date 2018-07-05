using Photo.Models;
using System;
using System.IO;

namespace Photo.FileSystem
{
    public class FileSystemAlbumRepository : IAlbumRepository
    {
        DirectoryInfo _PreviewLocation;

        public FileSystemAlbumRepository(
            DirectoryInfo previewLocation
        )
        {
            if (previewLocation == null)
                throw new ArgumentNullException(nameof(previewLocation));

            _PreviewLocation = previewLocation;
        }

        public IHashedAlbum Create(IPhotosRepository repository, Helpers.Helpers.ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            return
                new FileSystemAlbum(
                    _PreviewLocation,
                    repository,
                    errorGeneratingPreviewHandler
                );
        }
    }
}