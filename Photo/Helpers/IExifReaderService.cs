using System;
using System.IO;

namespace PhotoFiler.Photo
{
    public interface IExifReaderService
    {
        Stream Stream { get; set; }

        DateTime? DateTime();
    }
}
