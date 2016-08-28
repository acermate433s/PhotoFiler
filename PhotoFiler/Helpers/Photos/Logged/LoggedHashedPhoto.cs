using System;
using System.IO;
using PhotoFiler.Models;
using System.Diagnostics;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedPhoto : IHashedPhoto
    {
        IHashedPhoto _Photo;

        public LoggedHashedPhoto(TraceSource source, IHashedPhoto photo)
        {
            using (var scope = new ActivityTracerScope(source, "Hashed Photo"))
            {
                scope.Information("Photo: {0}", photo.FileInfo.FullName);
                _Photo = photo;
            }
        }

        public DateTime? CreationDateTime
        {
            get
            {
                return _Photo.CreationDateTime;
            }
        }

        public FileInfo FileInfo
        {
            get
            {
                return _Photo.FileInfo;
            }
        }

        public string Hash
        {
            get
            {
                return _Photo.Hash;
            }
        }

        public string Name
        {
            get
            {
                return _Photo.Name;
            }
        }

        public string Resolution
        {
            get
            {
                return _Photo.Resolution;
            }
        }

        public string Size
        {
            get
            {
                return _Photo.Size;
            }
        }
    }
}