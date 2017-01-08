using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoFiler.Helpers
{
    public class Photo
    {
        /// <summary>
        /// Location of the photo
        /// </summary>
        public string Location { get; protected set; }

        /// <summary>
        /// Filename of the photo
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// File size of the photo in human readable format
        /// </summary>
        public string Size { get; protected set; }

        /// <summary>
        /// Hash code of the photo.
        /// </summary>
        public string Hash { get; protected set; }

        /// <summary>
        /// Resolution of the photo
        /// </summary>
        public string Resolution
        {
            get
            {
                if (Width > 0 || Height > 0)
                    return $"{Width}x{Height}";
                else
                    return "Unknown";
            }
        }

        public int Height { get; protected set; } = 0;

        public int Width { get; protected set; } = 0;

        /// <summary>
        /// Date when the photo was created
        /// </summary>
        public DateTime? CreationDateTime { get; protected set; }
    }
}