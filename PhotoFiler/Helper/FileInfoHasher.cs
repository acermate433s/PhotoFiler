using ExifLib;
using ImageResizer;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using DictionaryHash = System.Collections.Generic.Dictionary<string, System.IO.FileInfo>;

namespace PhotoFiler.Helper
{
    public class FileInfoHasher 
    {
        const int MAX_LENGTH = 19;                              // Maximum hash length return by the algorithm

        private string _RootPath = "";                          // Root path to recursively scan all files
        private int _HashLength = 10;                           // Maximum length of the filename hash
        private DictionaryHash _HashTable = null;               // Dictionary of all the hashed filename of the root path
        private IEnumerable<FileHash> _List = null;

        /// <param name="path">Root path to recursively scan all files</param>
        /// <param name="hashLength">Maximum lenght of the filename hash</param>
        public FileInfoHasher(string path, int hashLength)
        {
            _RootPath = path;
            _HashLength =  hashLength <= MAX_LENGTH ? hashLength : MAX_LENGTH;
                
            try
            {
                var files = FileInfo(_RootPath);
                _HashTable = FileInfoHash(files);

                _List = Files().ToList();
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

            return _List.FirstOrDefault(item => item.Hash == hash).Preview.Invoke();
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
        /// <param name="fileInfos">List of FileInfo whose filenames were are going to hash</param>
        /// <returns>Returns a Dictionary of the hash of the filename</returns>
        /// <remarks>The hash would be truncated to the hash length</remarks>
        private DictionaryHash FileInfoHash(List<FileInfo> fileInfos)
        {
            var algorithm = (HashAlgorithm) MD5.Create();

            var value =
                fileInfos
                    .Select(item =>
                        new
                        {
                            Hash = ComputeHash(ref algorithm, item.FullName),
                            FileInfo = item,
                        }
                    )
                    .Select(item =>
                        new
                        {
                            Hash = item.Hash.Substring(item.Hash.Length - _HashLength),
                            FileInfo = item.FileInfo,
                        }
                    );

            return
                value
                    .ToDictionary(
                        item => item.Hash.Substring(item.Hash.Length - _HashLength),
                        item => item.FileInfo
                    );
        }

        /// <summary>
        /// Converts a number to Base 62
        /// </summary>
        /// <param name="number">Number to convert to Base 62</param>
        /// <returns>An array of char of Base 62</returns>
        private IEnumerable<char> ConvertToBase62(BigInteger number)
        {
            // There are the only allowed characters for a URL 
            const string SYMBOLS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            do
            {
                var index = (int)(number % SYMBOLS.Length);
                yield return SYMBOLS[index];
                number /= SYMBOLS.Length;
            }
            while (number > 0);
        }

        /// <summary>
        /// Converts string to array of bytes
        /// </summary>
        /// <param name="text">String to convert to array of bytes</param>
        /// <returns>Array of bytes of string</returns>
        private byte[] GetBytes(string text)
        {
            byte[] bytes = new byte[text.Length * sizeof(char)];
            System.Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Converts array of bytes to string
        /// </summary>
        /// <param name="bytes">Array of bytes to convert to string</param>
        /// <returns>String converted from bytes</returns>
        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Computes the hash of the text using the hashing algorithm and convert result to Base62
        /// </summary>
        /// <param name="algorithm">Algorith to use to computer the hash</param>
        /// <param name="text">String to be hashed</param>
        /// <returns></returns>
        private string ComputeHash(ref HashAlgorithm algorithm, string text)
        {
            var bytes = GetBytes(text);
            var hashCode = algorithm.ComputeHash(bytes);
            var hashNumber = String.Join("", hashCode.Select(item => String.Format("{0}", item)));
            var number = BigInteger.Abs(BigInteger.Parse(hashNumber));
            var digits = (new String(ConvertToBase62(number).ToArray()));

            return digits;
        }

        private IEnumerable<FileHash> Files()
        {
            var value =
                Items
                    .Select(item =>
                        new FileHash()
                        {
                            Hash = item.Key,
                            Name = item.Value.Name,
                            CreationDateTime =
                                (new Func<FileInfo, DateTime>(
                                    (file) =>
                                    {
                                        try {
                                            var reader = new ExifReader(file.FullName);
                                            DateTime exifCreationDate;
                                            if (reader.GetTagValue(ExifTags.DateTime, out exifCreationDate))
                                                return exifCreationDate;
                                            else
                                                return file.CreationTime;
                                        }
                                        catch
                                        {
                                            return file.CreationTime;
                                        }
                                    })
                                )
                                .Invoke(item.Value),
                            Size =
                                (new Func<long, string>(
                                    (length) =>
                                    {
                                        var suffixes = new[] { "bytes", "KB", "MB", "GB" };
                                        int index = 0;

                                        while (length > 1024)
                                        {
                                            length /= 1024;
                                            index++;
                                        }

                                        return String.Format("{0}{1}", length, suffixes[index]);
                                    })
                                )
                                .Invoke(item.Value.Length),
                            Preview =
                                (new Func<byte[]>(
                                    () =>
                                    {
                                        const int QUALITY = 50;             // JPEG compression quality
                                        const int MAX = 300;                // Maximum height or width

                                        if (!_HashTable.ContainsKey(item.Key))
                                            return null;

                                        byte[] photoBytes = File.ReadAllBytes(this[item.Key].FullName);
                                        using (MemoryStream inStream = new MemoryStream(photoBytes))
                                        using (MemoryStream outStream = new MemoryStream())
                                        {
                                            var job =
                                                new ImageJob(
                                                    inStream,
                                                    outStream,
                                                    new Instructions($"?height={MAX}&width={MAX}&mode=crop&quality={QUALITY}&format=jpg")
                                                );
                                            job.Build();

                                            return outStream.ToArray();
                                        }
                                    })
                                ),
                            PreviewUrl = $"Preview?hash={item.Key}",
                        }
                    )
                    .OrderBy(item => item.Name);
                
            return value;
        }

        /// <summary>
        /// Generates a page of IEnumerable<FileHash> with a no. of items.
        /// </summary>
        /// <param name="page">Page to show</param>
        /// <param name="count">No. of items per page</param>
        /// <returns></returns>
        public IEnumerable<FileHash> List(int page = 1, int count = 10)
        {
            IEnumerable<FileHash> value;

            if (_List.Count() > 0)
            {
                value =
                    _List
                        .Skip((page - 1) * count)
                        .Take(count);
            }
            else
                value = Enumerable.Empty<FileHash>();

            return value;
        }
    }
}