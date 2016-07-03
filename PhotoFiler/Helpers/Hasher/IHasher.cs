using System.Security.Cryptography;

namespace PhotoFiler.Helpers
{
    public interface IHasher<THashAlgorithm> where THashAlgorithm : HashAlgorithm
    {
        string Hash(string text, int length = 0);
    }
}