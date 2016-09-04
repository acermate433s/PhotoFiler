using PhotoFiler.Models;
using System;
using System.ComponentModel;

namespace PhotoFiler.Helpers.Photos.Hashed
{
    public class HashedPhoto : Photo, IHashedPhoto
    {
        public HashedPhoto(
            int hashLength,
            string path,
            IHasher hasher
        ) : base(path)
        {
            if (hashLength == 0)
                throw new ArgumentException("Hash length cannot be zero", nameof(hashLength));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (path == null)
                throw new ArgumentNullException(nameof(hasher));

            Hash = hasher.Hash(path, hashLength);
        }

        [DisplayName("Hash")]
        public string Hash { get; private set; }
    }
}