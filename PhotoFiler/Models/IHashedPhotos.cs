using System.Collections.Generic;

namespace PhotoFiler.Models
{
    public interface IHashedPhotos : IDictionary<string, IHashedPhoto> 
    {
        IEnumerable<IHashedPhoto> List(int page = 1, int count = 10);

        IEnumerable<IHashedPhoto> All();
    }
}