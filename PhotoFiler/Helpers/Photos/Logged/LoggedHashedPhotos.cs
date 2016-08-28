using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Telemetry;

namespace PhotoFiler.Helpers.Photos.Logged
{
    public class LoggedHashedPhotos : Dictionary<string, IHashedPhoto>, IHashedPhotos
    {
        enum ActivityType
        {
            New = 0,
            All = 1, 
            List = 2,
        }

        TraceSource _TraceSource;
        IHashedPhotos _Photos;

        public LoggedHashedPhotos(TraceSource source, IHashedPhotos photos)
        {
            _TraceSource = source;

            using (var scope = new ActivityTracerScope<ActivityType>(_TraceSource, ActivityType.New))
            {
                _Photos = photos;
            }
        }

        public IEnumerable<IHashedPhoto> All()
        {
            using (var scope = new ActivityTracerScope<ActivityType>(_TraceSource, ActivityType.All))
            {
                scope.Information("No. of photos: {0}", _Photos.Count());

                var result = _Photos.All();
                scope.Information(result.Select(item => item.FileInfo.FullName).ToArray());

                return _Photos.All();
            }
        }

        public IEnumerable<IHashedPhoto> List(int page = 1, int count = 10)
        {
            using (var scope = new ActivityTracerScope<ActivityType>(_TraceSource, ActivityType.List))
            {
                scope.Information("Retrieving {0} items from page {0}", page, count);
                return _Photos.List(page, count);
            }
        }
    }
}