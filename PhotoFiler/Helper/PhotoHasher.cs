using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DictionaryHash = System.Collections.Generic.Dictionary<string, System.IO.FileInfo>;

namespace PhotoFiler.Helper
{
    public class PhotoHasher 
    {
        const int MAX_LENGTH = 19;                              // Maximum hash length return by the algorithm

        private string _RootPath = "";                          // Root path to recursively scan all files
        private int _HashLength = 10;                           // Maximum length of the filename hash
        private DictionaryHash _HashTable = null;               // Dictionary of all the hashed filename of the root path
        private IEnumerable<Photo> _List = null;

        public string PreviewLocation = "";

        /// <param name="path">Root path to recursively scan all files</param>
        /// <param name="hashLength">Maximum lenght of the filename hash</param>
        public PhotoHasher(string path, int hashLength, string previewLocation = "")
        {
            _RootPath = path;
            _HashLength =  hashLength <= MAX_LENGTH ? hashLength : MAX_LENGTH;

            PreviewLocation = previewLocation;
                
            try
            {
                var files = FileInfo(_RootPath);
                _List = files.Select(item => new Photo(PreviewLocation, _HashLength, item.FullName)).ToList();
                _HashTable = Hash(files);
            }
            catch
            {
                _HashTable = new DictionaryHash();
            }
        }

        /// <summary>
        /// Dictionary of all FileInfo hashed by filename
        /// </summary>
        public Dictionary<string, FileInfo> Items
        {
            get { return _HashTable; }
        }

        /// <summary>
        /// FileInfo look-up using the hash
        /// </summary>
        /// <param name="hash">Hash of the filename of the FileInfo</param>
        /// <returns>FileInfo keyed using the hash</returns>
        public FileInfo this[string hash]
        {
            get
            {
                if (ContainsHash(hash))
                    return _HashTable[hash];
                else
                    return null;
            }   
        }

        public byte[] View(string hash)
        {
            if (!_HashTable.ContainsKey(hash))
                return null;

            return System.IO.File.ReadAllBytes(this[hash].FullName);
        }

        /// <summary>
        /// Resized the image to 25% of the size of the original image
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>Byte array of the resized image</returns>
        public byte[] Preview(string hash)
        {
            if (!_HashTable.ContainsKey(hash))
                return null;

            return _List.FirstOrDefault(item => item.Hash == hash).Preview();
        }

        /// <summary>
        /// Checks if the hash can be found
        /// </summary>
        /// <param name="hash">Hash to search</param>
        /// <returns>True if found, otherwise False</returns>
        public Boolean ContainsHash(string hash)
        {
            return _HashTable.ContainsKey(hash);
        }

        /// <summary>
        /// Recursively get the files in a given path
        /// </summary>
        /// <param name="root">Root path</param>
        /// <returns>A list of all files in a path scanned recursively</returns>
        private List<FileInfo> FileInfo(string root)
        {
            var value = new List<FileInfo>();
            var directory = (new DirectoryInfo(root));

            // add files in the current directory
            value
                .AddRange(
                    directory
                        .EnumerateFiles()
                        .Where(item => (new[] { ".jpg", ".png" }).Contains(item.Extension))
                        .Cast<FileInfo>()
                );

            // iterate all directories and add files in that directory
            value
                .AddRange(
                    directory
                        .EnumerateDirectories()
                        .SelectMany(item => FileInfo(item.FullName)
                    )
                );

            return value;
        }

        /// <summary>
        /// Computes the MD5 hash of the filenames in the list of FileInfo
        /// </summary>
        /// <param name="files">List of FileInfo whose filenames were are going to hash</param>
        /// <returns>Returns a Dictionary of the hash of the filename</returns>
        /// <remarks>The hash would be truncated to the hash length</remarks>
        private DictionaryHash Hash(List<FileInfo> files)
        {
            var value =
                _List
                    .ToDictionary(
                        item => item.Hash,
                        item => item.FileInfo
                    );

            return value;
        }

        /// <summary>
        /// Generates a page of IEnumerable<FileHash> with a no. of items.
        /// </summary>
        /// <param name="page">Page to show</param>
        /// <param name="count">No. of items per page</param>
        /// <returns></returns>
        public IEnumerable<Photo> List(int page = 1, int count = 10)
        {
            IEnumerable<Photo> value;

            if (_List.Count() > 0)
            {
                value =
                    _List
                        .Skip((page - 1) * count)
                        .Take(count);
            }
            else
                value = Enumerable.Empty<Photo>();

            return value;
        }

        public IEnumerable<Photo> List()
        {
            return _List;
        }

        public bool CreatePreviews()
        {
            bool value = true;

            if (Directory.Exists(PreviewLocation))
            {
                List()
                    .AsParallel()
                    .ForAll(item => 
                    {
                        try
                        {
                            var filename = Path.ChangeExtension(Path.Combine(PreviewLocation, item.Hash), "prev");
                            if (File.Exists(filename))
                                File.Delete(filename);

                            var buffer = item.Preview();
                            if(buffer != null)
                                File
                                    .WriteAllBytes(
                                        filename,
                                        buffer
                                    );
                        }
                        catch { }
                    });
            }
            else
                value = false;

            return value;
        }
    }
}