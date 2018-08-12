using System;
using System.Collections.Generic;
using System.IO;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemAlbumRepository : IAlbumRepository
    {
        private readonly DirectoryInfo previewLocation;

        public FileSystemAlbumRepository(
            DirectoryInfo previewLocation
        )
        {
            this.previewLocation = previewLocation ?? throw new ArgumentNullException(nameof(previewLocation));
        }

        public IHashedAlbum Create(List<IPreviewablePhoto> photos)
        {
            return
                new FileSystemAlbum(
                    previewLocation,
                    photos
                );
        }
    }
}