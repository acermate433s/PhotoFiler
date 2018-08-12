using System;

using Microsoft.Extensions.Logging;

namespace PhotoFiler.Photo.Logged
{
    public class LoggedBase
    {
        public LoggedBase(ILogger logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected ILogger Logger { get; set; }
    }
}