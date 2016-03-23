using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhotoFiler.Models
{
    public class Photo
    {
        public string Hash { get; set; } 

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Size")]
        public string Size { get; set; }

        [DisplayName("Resolution")]
        public string Resolution { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreationDateTime { get; set; }

        public Func<byte[]> Preview { get; set; }
    }
}