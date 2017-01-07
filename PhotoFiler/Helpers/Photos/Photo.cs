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
    public class Photo 
    {
        public Photo(
            string path,
            IHashFunction hasher
        )
        {
            if (hasher == null)
                throw new ArgumentNullException(nameof(hasher), "Hash function cannot be null");

            try
            {
                FileInfo = new System.IO.FileInfo(path);
                Name = FileInfo.Name;
                Size = ComputeSize(FileInfo);

                Hash = hasher.Compute(FileInfo.FullName);

                DateTime? creationDateTime = null;
                int width = 0;
                int height = 0;

                ReadFileData(
                    FileInfo,
                    out creationDateTime,
                    out width,
                    out height
                );

                Width = width;
                Height = height;

                CreationDateTime = creationDateTime;
            }
            catch
            {
                CreationDateTime = DateTime.Now;
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
        /// Hash code of the photo.
        /// </summary>
        public string Hash { get; private set; }

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

        public int Height { get; private set; } = 0;

        public int Width { get; private set; } = 0;

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
            long length = file.Length;
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
        /// <param name="creationDateTime">EXIF or file creation date time</param>
        /// <param name="height">Height of the photo</param>
        /// <param name="width">Width of the photo</param>
        /// <returns></returns>
        private void ReadFileData(
            FileInfo file,
            out DateTime? creationDateTime,
            out int width,
            out int height
        )
        {
            creationDateTime = file.CreationTime;
            width = 0;
            height = 0;

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
                    {
                        width = image.Width;
                        height = image.Height;
                    }

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
                width = 0;
                height = 0;
            }
        }
    }
}