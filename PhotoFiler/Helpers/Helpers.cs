using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoFiler.Helpers
{
    public static class Helpers
    {
        public delegate void ErrorGeneratingPreview(object sender, ErrorGeneratingPreviewArgs args);
    }

    public class ErrorGeneratingPreviewArgs : EventArgs
    {
        public IPhoto Photo { get; set; }

        public Exception Exception { get; set; }
    }

}