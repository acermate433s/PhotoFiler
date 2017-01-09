using System;
using Telemetry;

namespace PhotoFiler.Logged
{
    public class LoggedBase : IDisposable
    {
        public LoggedBase(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger;

        // To detect redundant calls
        private bool _Disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                    Logger.Dispose();

                _Disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}