using PhotoFiler.Models;
using System;
using Telemetry;

namespace PhotoFiler.Helpers.Repositories.Logged
{
    public class LoggedRepository : Repository
    {
        ILogger _Logger;

        public LoggedRepository(
            ILogger logger,
            IConfiguration configuration            
        ) : base(configuration)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _Logger = logger;
        }

        public new LoggedAlbumRepository CreateAlbumRepository()
        {
            return
                new LoggedAlbumRepository(
                    _Logger,
                    Configuration.PreviewLocation
                );
        }

        public new LoggedPhotoRepository CreatePhotoRepository()
        {
            return 
                new LoggedPhotoRepository(
                    _Logger,
                    Configuration.HashLength,
                    Configuration.HashingFunction,
                    Configuration.PreviewLocation
                );
        }

        public new LoggedPhotosRepository CreatePhotosRepository()
        {
            return
                new LoggedPhotosRepository(
                    _Logger,
                    Configuration.RootPath,
                    CreatePhotoRepository()
                );
        }
    }
}