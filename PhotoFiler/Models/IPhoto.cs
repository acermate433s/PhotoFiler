using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PhotoFiler.Models
{
    /// <summary>
    /// Photo
    /// </summary>
    public interface IPhoto
    {
        /// <summary>
        /// Date when the photo is created
        /// </summary>
        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        DateTime? CreationDateTime { get; }

        /// <summary>
        /// FileInfo object of the photo
        /// </summary>
        [DisplayName("FileInfo")]
        FileInfo FileInfo { get; }

        /// <summary>
        /// Name of the photo
        /// </summary>
        [DisplayName("Name")]
        string Name { get; }

        /// <summary>
        /// Resolution of the photo
        /// </summary>
        [DisplayName("Resolution")]
        string Resolution { get; }

        /// <summary>
        /// Height in pixels of the photo
        /// </summary>
        [DisplayName("Height")]
        int Height { get; }

        /// <summary>
        /// Width in pixels of the photo
        /// </summary>
        [DisplayName("Width")]
        int Width { get; }

        /// <summary>
        /// Formatted disk space used of the photo
        /// </summary>
        [DisplayName("Size")]
        string Size { get; }

        /// <summary>
        /// Hash code of the photo.
        /// </summary>
        string Hash { get; }
    }
}