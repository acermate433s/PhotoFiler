using PhotoFiler.Helpers.Photos;
using PhotoFiler.Models;
using System;
using System.IO;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Helpers.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        int _HashLength = 0;
        IHashFunction _HashingFunction = null;
        DirectoryInfo _PreviewLocation = null;

        public PhotoRepository(
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
                new PreviewablePhoto(
                    _HashLength,
                    file.FullName,
                    _HashingFunction
                );

            result.ErrorGeneratingPreviewHandler += errorGeneratingPreviewHandler;

            return result;
                
        }
    }
}