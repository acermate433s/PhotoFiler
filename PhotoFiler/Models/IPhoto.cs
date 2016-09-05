using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PhotoFiler.Models
{
    public interface IPhoto
    {
        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        DateTime? CreationDateTime { get; }

        [DisplayName("FileInfo")]
        FileInfo FileInfo { get; }

        [DisplayName("Name")]
        string Name { get; }

        [DisplayName("Resolution")]
        string Resolution { get; }

        [DisplayName("Size")]
        string Size { get; }
    }
}