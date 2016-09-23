using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoFiler.Helpers
{
    public static class Helpers
    {
        public delegate void ErrorGeneratingPreview(IPhoto photo, Exception exception);
    }
}