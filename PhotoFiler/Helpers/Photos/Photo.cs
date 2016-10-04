using ExifLib;
using PhotoFiler.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace PhotoFiler.Helpers
{
    /// <summary>
    /// Represents a photo in the Album
    /// </summary>
    [Bind(Exclude = "FileInfo")]
    public class Photo : IPhoto
    {
        public Photo(
            string path
        )
        {
            try
            {
                FileInfo = new System.IO.FileInfo(path);
                Name = FileInfo.Name;
                Size = ComputeSize(FileInfo);

                DateTime? creationDateTime = null;
                string resolution = "Unknown";

                ReadFileData(
                    FileInfo,
                    out creationDateTime,
                    out resolution
                );

                CreationDateTime = creationDateTime;
                Resolution = resolution;
            }
            catch
            {
                CreationDateTime = DateTime.Now;
                Resolution = "Unknown";
            }
        }

        /// <summary>
        /// FileInfo object that represents the photo
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// Filename of the photo
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// File size of the photo in human readable format
        /// </summary>
        public string Size { get; private set; }

        /// <summary>
        /// Resolution of the photo
        /// </summary>
        public string Resolution { get; private set; }

        /// <summary>
        /// Date when the photo was created
        /// </summary>
        public DateTime? CreationDateTime { get; private set; }

        /// <summary>
        /// Get the size of the file to a human-readable format
        /// </summary>
        /// <param name="file">File to get the size</param>
        /// <returns>The size of the file in a human-readable format</returns>
        private string ComputeSize(FileInfo file)
        {
            // set the size of the file to a human-readable format
            long length = FileInfo.Length;
            var suffixes = new[] { "bytes", "KB", "MB", "GB" };
            int index = 0;

            while (length > 1024)
            {
                length /= 1024;
                index++;
            }

            return $"{length}{suffixes[index]}";
        }

        /// <summary>
        /// Read the file and get the image file's creation date time, resolution and preview.
        /// </summary>
        /// <param name="file">FileInfo representing the file</param>
        /// <param name="hash">Computed hash of the file</param>
        /// <param name="creationDateTime">EXIF or file creation date time</param>
        /// <param name="resolution">Resolution of the image file</param>
        /// <returns></returns>
        private void ReadFileData(
            FileInfo file,
            out DateTime? creationDateTime,
            out string resolution
        )
        {
            creationDateTime = file.CreationTime;
            resolution = "Unknown";

            // Read the entire file into memory.  This would be used throughout
            // the function to minimize reading the file multiple times
            byte[] buffer = null;
            try
            {
                buffer = File.ReadAllBytes(file.FullName);

                if (buffer != null)
                {
                    using (var stream = new MemoryStream(buffer))
                    using (var image = Image.FromStream(stream, false, false))
                        resolution = $"{image.Width}x{image.Height}";

                    // Read the creation date from the EXIF. If we cannot, then
                    // set the creation date time to the file creation date time
                    using (var stream = new MemoryStream(buffer))
                    {
                        var reader = new ExifReader(stream);
                        DateTime exifCreationDate;
                        if (reader.GetTagValue(ExifTags.DateTime, out exifCreationDate))
                            creationDateTime = exifCreationDate;
                        else
                            creationDateTime = file.CreationTime;
                    }
                }
            }
            catch
            {
                creationDateTime = null;
                resolution = "";
            }
        }
    }
}