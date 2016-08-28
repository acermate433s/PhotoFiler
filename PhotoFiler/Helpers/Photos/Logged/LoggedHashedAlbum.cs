using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedAlbum : IHashedAlbum
    {
        Telemetry.ILogger _Logger;
        IHashedAlbum _HashedAlbum;

        public LoggedHashedAlbum(Telemetry.ILogger logger, IHashedAlbum album)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (album == null)
                throw new ArgumentNullException("album");

            _Logger = logger;
            _HashedAlbum = album;

            logger.Verbose(album.Photos?.Select(item => item.Value.FileInfo.FullName).ToArray());
        }

        public IHashedPhotos Photos
        {
            get
            {
                return _HashedAlbum.Photos;
            }
        }

        public DirectoryInfo PreviewLocation
        {
            get
            {
                return _HashedAlbum.PreviewLocation;
            }
        }

        public int Count()
        {
            return _HashedAlbum.Count();
        }

        public void GeneratePreviews()
        {
            _HashedAlbum.GeneratePreviews();
        }

        public IEnumerable<IHashedPhoto> List(int page = 1, int count = 10)
        {
            var result =_HashedAlbum.List(page, count);

            return result;
        }

        public IHashedPhoto Photo(string hash)
        {
            var result = _HashedAlbum.Photo(hash);
            if (result == null)
                _Logger.Warning($"Cannot find photo for hash {hash}");

            return result;
        }

        public byte[] Preview(string hash)
        {
            var result = _HashedAlbum.Preview(hash);
            var photoFileInfo = Photos[hash]?.FileInfo;
            if (photoFileInfo != null)
            {
                if (result != null)
                    _Logger.Information($"Preview size for \"{photoFileInfo.FullName}\"({hash}) is {result.Length} bytes");
                else
                    _Logger.Warning($"Cannot generate preview for \"{photoFileInfo.FullName}\"({hash})");
            }
            else
                _Logger.Warning($"Cannot find photo for hash {hash}");
            return result;
        }

        public byte[] View(string hash)
        {
            var result = _HashedAlbum.View(hash);
            var photoFileInfo = Photos[hash]?.FileInfo;
            if (photoFileInfo != null)
            {
                if (result != null)
                    _Logger.Information($"Full size for \"{photoFileInfo.FullName}\"({hash}) is {result.Length} bytes");
                else
                    _Logger.Warning($"Cannot generate view for \"{photoFileInfo.FullName}\"({hash})");
            }
            else
                _Logger.Warning($"Cannot find photo for hash {hash}");

            return result;
        }
    }
}