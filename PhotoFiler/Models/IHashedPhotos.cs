using System.Collections.Generic;

namespace PhotoFiler.Models
{
    public interface IHashedPhotos<THashedPhoto> : IDictionary<string, THashedPhoto> where THashedPhoto : IHashedPhoto
    {
        IEnumerable<THashedPhoto> List(int page = 1, int count = 10);

        IEnumerable<THashedPhoto> All();
    }
}