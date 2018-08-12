using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemPreviewablePhotos : IPreviewablePhotos
    {
        private readonly DirectoryInfo source = null;
        private readonly IPhotoRepository photoRepository = null;
        private readonly IExifReaderService exifReader = null;

        public FileSystemPreviewablePhotos(
            DirectoryInfo source,
            IPhotoRepository photoRepository,
            IExifReaderService exifReader
        )
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            this.exifReader = exifReader ?? throw new ArgumentNullException(nameof(exifReader));
        }

        public List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            return
                GetPhotoFiles(source)
                    .Select(file => photoRepository.Create(file, this.exifReader, errorGeneratingPreviewHandler))
                    .ToList();
        }

        private List<FileInfo> GetPhotoFiles(DirectoryInfo root)
        {
            var value = new List<FileInfo>();

            // add files in the current directory
            value
                .AddRange(
                    root
                        .EnumerateFiles()
                        .Where(file => (new[] { ".jpg", ".png" }).Contains(file.Extension.ToLower(CultureInfo.CurrentCulture)))
                        .Cast<FileInfo>()
                );

            // iterate all directories and add files in that directory
            value
                .AddRange(
                    root
                        .EnumerateDirectories()
                        .SelectMany(directory => GetPhotoFiles(directory)
                    )
                );

            return value;
        }
    }
}