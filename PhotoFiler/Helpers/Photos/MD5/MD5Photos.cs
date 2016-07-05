using PhotoFiler.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoFiler.Helpers.MD5
{
    public class MD5HashedPhotos : Dictionary<string, IHashedPhoto>, IHashedPhotos<IHashedPhoto>
    {
        private const int MAX_LENGTH = 19;                              // Maximum hash length return by the algorithm

        private string _RootPath = "";                                  // Root path to recursively scan all files
        private int _HashLength = 10;                                   // Maximum length of the filename hash

        /// <param name="path">Root path to recursively scan all files</param>
        /// <param name="hashLength">Maximum lenght of the filename hash</param>
        public MD5HashedPhotos(string path, int hashLength)
        {
            _RootPath = path;
            _HashLength = hashLength <= MAX_LENGTH ? hashLength : MAX_LENGTH;

            try
            {
                var photos = GetPhotos(_RootPath);
                photos
                    .Select(item => new MD5HashedPhoto(_HashLength, item.FullName))
                    .ToList()
                    .ForEach(item => this.Add(item.Hash, item));
            }
            catch
            {
            }
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
                return Enumerable.Empty<MD5HashedPhoto>();
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