using ExifLib;
using Photo.Models;
using System;
using System.Drawing;
using System.IO;

namespace Photo.FileSystem
{
    /// <summary>
    /// Represents a photo in the Album
    /// </summary>
    public class FileSystemPhoto : Photo
    {
        public FileSystemPhoto(
            string path,
            IHashFunction hasher
        )
        {
            if (hasher == null)
                throw new ArgumentNullException(nameof(hasher), "Hash function cannot be null");

            try
            {
                var fileInfo = new FileInfo(path);

                Location = path;
                Name = fileInfo.Name;
                Size = ComputeSize(fileInfo);

                Hash = hasher.Compute(fileInfo.FullName);

                ReadFileData(
                    fileInfo,
                    out DateTime? creationDateTime,
                    out int width,
                    out int height
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
        /// Get the size of the file to a human-readable format
        /// </summary>
        /// <param name="file">File to get the size</param>
        /// <returns>The size of the file in a human-readable format</returns>
        private static string ComputeSize(FileInfo file)
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
        private static void ReadFileData(
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
                        if(reader.GetTagValue(ExifTags.DateTime, out DateTime exifCreationDate))
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