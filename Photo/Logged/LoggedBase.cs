using System;
using Telemetry;

namespace Photo.Logged
{
    public class LoggedBase : IDisposable
    {
        public LoggedBase(ILogger logger)
        {
            Logger = logger;
        }

        private ILogger logger;

        // To detect redundant calls
        private bool _Disposed = false;

        protected ILogger Logger { get => logger; set => logger = value; }

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
            GC.SuppressFinalize(this);
        }
    }
}