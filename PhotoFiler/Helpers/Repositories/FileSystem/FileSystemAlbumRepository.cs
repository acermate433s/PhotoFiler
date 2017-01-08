using PhotoFiler.Helpers.Photos;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace PhotoFiler.Helpers.Repositories
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

        public IHashedAlbum Create(List<IPreviewablePhoto> photos)
        {
            return
                new FileSystemAlbum(
                    _PreviewLocation,
                    photos
                );
        }
    }
}