﻿using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoFiler.Helpers.Photos.Hashed
{
    public class PreviewableHashedPhotos : IPreviewableHashedPhotos
    {
        DirectoryInfo _Source = null;
        Func<FileInfo, IPreviewableHashedPhoto> _Initiator;

        public PreviewableHashedPhotos(
            DirectoryInfo source,
            Func<FileInfo, IPreviewableHashedPhoto> initiator
        )
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source == null)
                throw new ArgumentNullException("initiator");

            _Source = source;
            _Initiator = initiator;
        }

        public List<IPreviewableHashedPhoto> Retrieve()
        {
            var files = GetPhotoFiles(_Source);

            return
                GetPhotoFiles(_Source)
                    .Select(file => _Initiator.Invoke(file))
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