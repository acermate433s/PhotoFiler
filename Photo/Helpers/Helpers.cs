using Photo.Models;
using System;

namespace Photo.Helpers
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