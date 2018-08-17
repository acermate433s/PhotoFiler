using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Directory
{
    public class DirectoryPreviewablePhotos : IPreviewablePhotos
    {
        private readonly DirectoryInfo source = null;
        private readonly IPhotoRepository photoRepository = null;
        private readonly IExifReaderService exifReader = null;
        private readonly IImageResizerService imageResizer = null;

        public DirectoryPreviewablePhotos(
            DirectoryInfo source,
            IPhotoRepository photoRepository,
            IExifReaderService exifReader,
            IImageResizerService imageResizer
        )
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            this.exifReader = exifReader ?? throw new ArgumentNullException(nameof(exifReader));
            this.imageResizer = imageResizer ?? throw new ArgumentNullException(nameof(imageResizer));
        }

        public List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            var value = new List<FileInfo>();

            // add files in the current directory
            value
                .AddRange(
                    this.source
                        .EnumerateFiles()
                        .Where(file => (new[] { ".jpg", ".png" }).Contains(file.Extension.ToLower(CultureInfo.CurrentCulture)))
                        .Cast<FileInfo>()
                );

            return value
                .Select(file => photoRepository.Create(
                    file, 
                    this.exifReader, 
                    this.imageResizer, errorGeneratingPreviewHandler))
                .ToList();
        }
    }
}