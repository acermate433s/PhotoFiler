using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace PhotoFiler.Hasher
{
    public class Base62HasherBase<THashAlgorithm> : IHashFunction where THashAlgorithm : HashAlgorithm, new()
    {
        protected THashAlgorithm _Algorithm;

        public Base62HasherBase()
        {
            _Algorithm = new THashAlgorithm();
        }

        /// <summary>
        /// Converts a number to Base 62
        /// </summary>
        /// <param name="number">Number to convert to Base 62</param>
        /// <returns>An array of char of Base 62</returns>
        private IEnumerable<char> ConvertToBase62(BigInteger number)
        {
            // This are the only allowed characters for a URL
            const string SYMBOLS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            do
            {
                var index = (int) (number % SYMBOLS.Length);
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
        /// <param name="text">String to be hashed</param>
        /// <returns></returns>
        private string ComputeHash(string text)
        {
            var bytes = GetBytes(text);
            var hashCode = _Algorithm.ComputeHash(bytes);
            var hashNumber = String.Join("", hashCode.Select(item => String.Format("{0}", item)));
            var number = BigInteger.Abs(BigInteger.Parse(hashNumber));
            var digits = (new String(ConvertToBase62(number).ToArray()));

            return digits;
        }

        /// <summary>
        /// Computes the hash of a string
        /// </summary>
        /// <param name="text">String to hash</param>
        /// <returns>A Base62 string</returns>
        public string Compute(string text)
        {
            var value = ComputeHash(text);

            if (HashLength > 0)
                value = value.Substring(value.Length - HashLength);

            return value;
        }

        public virtual int HashLength { get; protected set; } = 5;
    }
}