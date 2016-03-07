using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using DictionaryHash = System.Collections.Generic.Dictionary<string, System.IO.FileInfo>;

namespace PhotoFiler.Helper
{
    public class FileInfoHasher
    {
        const int MAX_LENGTH = 19;                          // Maximum hash length return by the algorithm

        private string _RootPath = "";                      // Root path to recursively scan all files
        private int _HashLength = 4;                        // Maximum length of the filename hash
        private DictionaryHash _HashTable = null;           // Dictionary of all the hashed filename of the root path

        #region Constructors

        /// <param name="path">Root path to recursively scan all files</param>
        /// <param name="hashLength">Maximum lenght of the filename hash</param>
        public FileInfoHasher(string path, int hashLength)
        {
            _RootPath = path;
            _HashLength =  hashLength <= MAX_LENGTH ? hashLength : MAX_LENGTH;

            try
            {
                _HashTable = FileInfoHash(FileInfo(_RootPath));
            }
            catch(Exception ex)
            {
                _HashTable = new DictionaryHash();
            }
        }

        #endregion

        #region Properties

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

        /// <summary>
        /// Checks if the hash can be found
        /// </summary>
        /// <param name="hash">Hash to search</param>
        /// <returns>True if found, otherwise False</returns>
        public Boolean ContainsHash(string hash)
        {
            return _HashTable.ContainsKey(hash);
        }

        #endregion

        #region Functions

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
                        .Cast<FileInfo>()
                );

            // iterate all directories and add files in that directory
            value
                .AddRange(
                    directory
                        .EnumerateDirectories()
                        .AsParallel()
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

            return
                fileInfos
                    .Select(item => new
                        {
                            Hash = ComputeHash(ref algorithm, item.FullName),
                            FileInfo = item,
                        }
                    )
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

        #endregion
    }
}