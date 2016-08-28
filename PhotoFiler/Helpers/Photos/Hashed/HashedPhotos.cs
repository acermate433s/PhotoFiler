using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PhotoFiler.Helpers.Hashed
{
    public class HashedPhotos : Dictionary<string, IHashedPhoto>, IHashedPhotos
    {
        // Root path to recursively scan all files
        private string _RootPath = "";

        /// <param name="path">Root path to recursively scan all files</param>
        /// <param name="hashLength">Maximum lenght of the filename hash</param>
        public HashedPhotos(
            string path, 
            int hashLength,
            Func<string, IHashedPhoto> instantiator
        )
        {
            _RootPath = path;

            var photos = GetPhotos(_RootPath);
            photos
                .Select(item => instantiator.Invoke(item.FullName))
                .ToList()
                .ForEach(item =>
                {
                    // discard items with an existing key
                    if(!this.ContainsKey(item.Hash))
                        this.Add(item.Hash, item);
                });
        }

        /// <summary>
        /// Generates a page of IEnumerable<FileHash> with a no. of items.
        /// </summary>
        /// <param name="page">Page to show</param>
        /// <param name="count">No. of items per page</param>
        /// <returns></returns>
        public IEnumerable<IHashedPhoto> List(int page = 1, int count = 10)
        {
            if (this.Count() > 0)
            {
                return
                    this
                        .Values
                        .Skip((page - 1) * count)
                        .Take(count)
                        .Select(item => item);
            }
            else
                return Enumerable.Empty<IHashedPhoto>();
        }

        public IEnumerable<IHashedPhoto> All()
        {
            return this.Values;
        }

        /// <summary>
        /// Recursively get the photo files in a given path
        /// </summary>
        /// <param name="root">Root path</param>
        /// <returns>A list of all files in a path scanned recursively</returns>
        private List<FileInfo> GetPhotos(string root)
        {
            var value = new List<FileInfo>();
            var directory = (new DirectoryInfo(root));

            // add files in the current directory
            value
                .AddRange(
                    directory
                        .EnumerateFiles()
                        .Where(item => (new[] { ".jpg", ".png" }).Contains(item.Extension.ToLower()))
                        .Cast<FileInfo>()
                );

            // iterate all directories and add files in that directory
            value
                .AddRange(
                    directory
                        .EnumerateDirectories()
                        .SelectMany(item => GetPhotos(item.FullName)
                    )
                );

            return value;
        }
    }
}