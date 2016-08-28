using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Telemetry;
using System.Diagnostics;

namespace PhotoFiler.Helpers.Logged
{
    public class LoggedPhotoPreviewer<TLogger> : IHashedPhotoPreviewer where TLogger : ILogger
    {
        enum ActityType
        {
            Generate = 1,
            Preview = 2,
            View = 3,
            PreviewLocation = 4,
            Photo = 5,
        }

        TraceSource _TraceSource;
        IHashedPhotoPreviewer _HashedPhotoPreviewer;

        public LoggedPhotoPreviewer(TraceSource traceSource, IHashedPhotoPreviewer photoPreviewer)
        {
            _TraceSource = traceSource;
            _HashedPhotoPreviewer = photoPreviewer;
        }

        public IHashedPhoto Photo
        {
            get
            {
                using (var scope = new ActivityTracerScope<ActityType>(_TraceSource, ActityType.Photo))
                {
                    return _HashedPhotoPreviewer.Photo;
                }
            }
            set
            {
                using (var scope = new ActivityTracerScope<ActityType>(_TraceSource, ActityType.Photo))
                {
                    scope.Information("Photo full name: {0}", value.FileInfo.FullName);
                    _HashedPhotoPreviewer.Photo = value;
                }
            }
        }
        public DirectoryInfo PreviewLocation
        {
            get
            {
                using (var scope = new ActivityTracerScope<ActityType>(_TraceSource, ActityType.PreviewLocation))
                {
                    return _HashedPhotoPreviewer.PreviewLocation;
                }
            }
            set
            {
                using (var scope = new ActivityTracerScope<ActityType>(_TraceSource, ActityType.PreviewLocation))
                {
                    scope.Information("Preview location: {0}", value.FullName);
                    _HashedPhotoPreviewer.PreviewLocation = value;
                }
            }
        }
        public bool Generate()
        {
            using (var scope = new ActivityTracerScope<ActityType>(_TraceSource, ActityType.Generate))
            {
                return _HashedPhotoPreviewer.Generate();
            }
        }
        public byte[] Preview()
        {
            using (var scope = new ActivityTracerScope<ActityType>(_TraceSource, ActityType.Preview))
            {
                var result = _HashedPhotoPreviewer.Preview();
                scope.Information("Preview size: {0} bytes", result.Length);

                return result;
            }
        }
        public byte[] View()
        {
            using (var scope = new ActivityTracerScope<ActityType>(_TraceSource, ActityType.View))
            {
                var result = _HashedPhotoPreviewer.View();
                scope.Information("Full size: {0} bytes", result.Length);

                return result;
            }
        }
    }
}