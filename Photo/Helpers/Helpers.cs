using System;

using PhotoFiler.Photo.Models;

namespace PhotoFiler.Photo
{
    public static class Helpers
    {
        public delegate void ErrorGeneratingPreviewEventHandler(object sender, ErrorGeneratingPreviewEventArgs args);
    }

    public class ErrorGeneratingPreviewEventArgs : EventArgs
    {
        public IPhoto Photo { get; set; }

        public Exception Exception { get; set; }
    }

}