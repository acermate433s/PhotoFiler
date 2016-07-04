using System.Security.Cryptography;

namespace PhotoFiler.Models
{
    public interface IHasher<THashAlgorithm> where THashAlgorithm : HashAlgorithm
    {
        string Hash(string text, int length = 0);
    }
}