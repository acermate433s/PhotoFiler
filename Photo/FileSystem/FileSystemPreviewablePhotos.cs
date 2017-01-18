using Photo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Photo.Helpers.Helpers;

namespace Photo.FileSystem
{
    public class FileSystemPreviewablePhotos : IPreviewablePhotos
    {
        DirectoryInfo _Source = null;
        IPhotoRepository _PhotoRepository = null;

        public FileSystemPreviewablePhotos(
            DirectoryInfo source,
            IPhotoRepository photoRepository
        )
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (photoRepository == null)
                throw new ArgumentNullException(nameof(photoRepository));

            _Source = source;
            _PhotoRepository = photoRepository;
        }

        public List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreview errorGeneratingPreviewHandler = null)
        {
            return
                GetPhotoFiles(_Source)
                    .Select(file => _PhotoRepository.Create(file, errorGeneratingPreviewHandler))
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
                        .Where(file => (new[] { ".jpg", ".png" }).Contains(file.Extension.ToLower()))
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