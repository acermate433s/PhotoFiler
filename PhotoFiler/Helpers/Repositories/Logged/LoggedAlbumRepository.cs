using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedAlbumRepository : AlbumRepository
    {
        ILogger _Logger;

        public LoggedAlbumRepository(
            ILogger logger,
            DirectoryInfo previewLocation
        ) : base(previewLocation)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _Logger = logger;
        }

        public new LoggedHashedAlbum Create(List<IPreviewablePhoto> photos)
        {
            return
                new LoggedHashedAlbum(
                    _Logger,
                    base.Create(photos)
                );
        }
    }
}