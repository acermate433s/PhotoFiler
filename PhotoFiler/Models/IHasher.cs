using System.Security.Cryptography;

namespace PhotoFiler.Models
{
    public interface IHasher
    {
        string Hash(string text, int length = 0);
    }
}