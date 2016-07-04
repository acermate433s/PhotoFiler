using System.ComponentModel;

namespace PhotoFiler.Helpers.MD5
{
    public class MD5HashedPhoto : Photo, IHashedPhoto
    {
        public MD5HashedPhoto(
            int hashLength,
            string path
        ) : base(path)
        {
            var hasher = new MD5Hasher();
            Hash = hasher.Hash(FileInfo.FullName, hashLength);
        }

        [DisplayName("Hash")]
        public string Hash { get; private set; }
    }
}