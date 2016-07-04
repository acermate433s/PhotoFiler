using System;
using System.IO;

namespace PhotoFiler.Models
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