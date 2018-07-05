using Photo.Models;
using System;

namespace Photo.Helpers
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