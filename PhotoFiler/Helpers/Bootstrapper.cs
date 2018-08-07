﻿using System;

namespace PhotoFiler.Helpers
{
    public static class Bootstrapper
    {
        private static IServiceProvider serviceProvider = null;

        public static IServiceProvider ServiceProvider { 
            get 
            {
                return serviceProvider;
            }
            set
            {
                serviceProvider = value;
            }
        }
    }
}