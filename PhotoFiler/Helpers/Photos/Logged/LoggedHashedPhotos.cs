using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedPhotos : Dictionary<string, IHashedPhoto>, IHashedPhotos
    {
        ILogger _Logger;
        IHashedPhotos _Photos;

        public LoggedHashedPhotos(ILogger logger, IHashedPhotos photos)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (photos == null)
                throw new ArgumentNullException("photos");

            _Logger = logger;
            _Photos = photos;

            foreach(var photo in photos)
                this.Add(photo.Key, photo.Value);

            _Logger.Information($"{_Photos.Count()} photos loaded");
            _Logger.Verbose(_Photos.Select(item => item.Value.FileInfo.FullName).ToArray());
            
        }

        public IEnumerable<IHashedPhoto> All()
        {
            var result = _Photos.All();
            return _Photos.All();
        }

        public IEnumerable<IHashedPhoto> List(int page = 1, int count = 10)
        {
            _Logger.Information($"Retrieving {count} items from page {page}");
            var result = _Photos.List(page, count);
            _Logger.Information($"Retrieved {result.Count()} items from page {page}");

            return result;
        }
    }
}