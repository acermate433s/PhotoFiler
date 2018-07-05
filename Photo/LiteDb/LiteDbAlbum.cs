using System;
using System.Collections.Generic;
using System.IO;
using Photo.Models;

namespace Photo.LiteDb
{
    public class LiteDbAlbum : IHashedAlbum
    {
        const string PHOTOS = "Photos";

        private IPhotosRepository _PhotosRepository;

        public LiteDbAlbum(IPhotosRepository repository)
        {
            _PhotosRepository = repository;
        }

        public IList<IPreviewablePhoto> Photos
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public void GeneratePreviews()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPreviewablePhoto> List(int page = 1, int count = 10)
        {
            throw new NotImplementedException();
        }

        public IPreviewablePhoto Photo(string hash)
        {
            throw new NotImplementedException();
        }

        public byte[] Preview(string hash)
        {
            throw new NotImplementedException();
        }

        public byte[] View(string hash)
        {
            throw new NotImplementedException();
        }
    }
}
