using PhotoFiler.Models;
using System.ComponentModel;

namespace PhotoFiler.Helpers.Hashed
{
    public class HashedPhoto : Photo, IHashedPhoto
    {
        public HashedPhoto(
            int hashLength,
            string path,
            IHasher hasher
        ) : base(path)
        {
            Hash = hasher.Hash(path, hashLength);
        }

        [DisplayName("Hash")]
        public string Hash { get; private set; }
    }
}