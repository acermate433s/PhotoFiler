using Photo.Models;
using System;
using System.IO;
using static Photo.Helpers.Helpers;

namespace Photo.FileSystem
{
    public class FileSystemPhotoRepository : IPhotoRepository
    {
        int _HashLength = 0;
        IHashFunction _HashingFunction = null;
        DirectoryInfo _PreviewLocation = null;

        public FileSystemPhotoRepository(
            int hashLength,
            IHashFunction hashingFunction,
            DirectoryInfo previewLocation
        )
        {
            if (hashLength < 0)
                throw new ArgumentOutOfRangeException(nameof(hashLength), "Hash length must be greater than zero.");

            if (hashingFunction == null)
                throw new ArgumentNullException(nameof(hashingFunction));

            if (previewLocation == null)
                throw new ArgumentNullException(nameof(previewLocation));

            _HashLength = hashLength;
            _HashingFunction = hashingFunction;
            _PreviewLocation = previewLocation;
        }

        public IPreviewablePhoto Create(
            FileInfo file,
            ErrorGeneratingPreview errorGeneratingPreviewHandler = null
        )
        {
            var result =
                new FileSystemPreviewablePhoto(
                    _HashLength,
                    file.FullName,
                    _HashingFunction
                );

            result.ErrorGeneratingPreviewHandler += errorGeneratingPreviewHandler;

            return result;
                
        }
    }
}