﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telemetry;

namespace PhotoFiler.Helpers
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