using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoFiler.Helpers
{
    public class Photos : Dictionary<string, Photo>
    {
        const int MAX_LENGTH = 19;                              // Maximum hash length return by the algorithm

        private string _RootPath = "";                          // Root path to recursively scan all files
        private int _HashLength = 10;                           // Maximum length of the filename hash

        public string PreviewLocation = "";

        /// <param name="path">Root path to recursively scan all files</param>
        /// <param name="hashLength">Maximum lenght of the filename hash</param>
        public Photos(string path, int hashLength, string previewLocation = "")
        {
            _RootPath = path;
            _HashLength = hashLength <= MAX_LENGTH ? hashLength : MAX_LENGTH;

            PreviewLocation = previewLocation;

            var photos = GetPhotos(_RootPath);
            photos
                .Select(item => new Photo(PreviewLocation, _HashLength, item.FullName))
                .AsParallel()
                .ForAll(item => this.Add(item.Hash, item));
        }

        /// <summary>
        /// Generates a page of IEnumerable<FileHash> with a no. of items.
        /// </summary>
        /// <param name="page">Page to show</param>
        /// <param name="count">No. of items per page</param>
        /// <returns></returns>
        public IEnumerable<Photo> List(int page = 1, int count = 10)
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
                return Enumerable.Empty<Photo>();
        }

        /// <summary>
        /// Recursively get the files in a given path
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