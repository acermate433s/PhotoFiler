using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoFiler.Models
{
    public class FileHash
    {
        public string Hash { get; set; } 

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Size")]
        [DisplayFormat(DataFormatString = "{0:###,###,###}")]
        public string Size { get; set; }

        [DisplayName("Preview")]
        public string PreviewUrl { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreationDateTime { get; set; }

        public Func<byte[]> Preview { get; set; }
    }
}