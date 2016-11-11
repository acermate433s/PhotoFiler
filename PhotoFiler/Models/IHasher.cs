using System.Security.Cryptography;

namespace PhotoFiler.Models
{
    /// <summary>
    /// Hashing function
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Generate a hash
        /// </summary>
        /// <param name="text">String to hash</param>
        /// <param name="length">Number of characters for the hash</param>
        /// <returns></returns>
        string Hash(string text, int length = 0);
    }
}