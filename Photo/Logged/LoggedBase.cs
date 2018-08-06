﻿using Microsoft.Extensions.Logging;

namespace Photo.Logged
{
    public class LoggedBase
    {
        public LoggedBase(ILogger logger)
        {
            Logger = logger;
        }

        private ILogger logger;

        protected ILogger Logger { get => logger; set => logger = value; }
    }
}