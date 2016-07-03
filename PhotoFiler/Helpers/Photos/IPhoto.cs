using System;
using System.IO;

namespace PhotoFiler.Helpers
{
    public interface IPhoto
    {
        DateTime? CreationDateTime { get; }
        FileInfo FileInfo { get; }
        string Name { get; }
        string Resolution { get; }
        string Size { get; }
    }
}