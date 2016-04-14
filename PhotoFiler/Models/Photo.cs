using ExifLib;
using ImageResizer;
using PhotoFiler.Helper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace PhotoFiler.Models
{
    [Bind(Exclude = "Preview,FileInfo")]
    public class Photo
    {
        // JPEG compression quality
        const int QUALITY = 50;

        // Maximum height and width
        const int MAX = 300;

        string _PreviewLocation;

        public Photo(
            string previewLocation,
            int hashLength,
            string path
        )
        {
            try
            {
                _PreviewLocation = previewLocation;

                FileInfo = new System.IO.FileInfo(path);
                Name = FileInfo.Name;
                Hash = (new Hasher()).Hash(FileInfo.FullName, hashLength);

                // set the size of the file to a human-readable format
                long length = FileInfo.Length;
                var suffixes = new[] { "bytes", "KB", "MB", "GB" };
                int index = 0;

                while (length > 1024)
                {
                    length /= 1024;
                    index++;
                }
                Size = String.Format("{0}{1}", length, suffixes[index]);

                DateTime? creationDateTime = null;
                string resolution = "Unknown";

                ReadFileData(
                    FileInfo,
                    Hash,
                    out creationDateTime,
                    out resolution
                );

                CreationDateTime = creationDateTime;
                Resolution = resolution;
            }
            catch (IOException iox)
            {
                throw iox;
            }
            catch
            {
                CreationDateTime = DateTime.Now;
                Resolution = "Unknown";
            }
        }

        [DisplayName("FileInfo")]
        public FileInfo FileInfo { get; private set; }

        [DisplayName("Hash")]
        public string Hash { get; private set; }

        [DisplayName("Name")]
        public string Name { get; private set; }

        [DisplayName("Size")]
        public string Size { get; private set; }

        [DisplayName("Resolution")]
        public string Resolution { get; private set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? CreationDateTime { get; private set; }

        /// <summary>
        /// Generates the preview of the image file
        /// </summary>
        /// <returns>
        /// A byte array of the preview image
        /// </returns>
        public byte[] Preview()
        {
            byte[] result = null;

            // Read the entire file into memory.  This would be used throughout 
            // the function to minimize reading the file multiple times
            byte[] buffer = null;
            try
            {
                buffer = File.ReadAllBytes(FileInfo.FullName);
            }
            catch
            {
                buffer = null;
            }

            if (buffer != null && Directory.Exists(_PreviewLocation))
            {
                // the preview file name should be the hash and the extension 'prev'
                var previewFile = Path.ChangeExtension(Path.Combine(_PreviewLocation, Hash), "prev");
                if (File.Exists(previewFile))
                {
                    // use the existing preview file if it's already in there
                    result = File.ReadAllBytes(previewFile);
                }
                else
                {
                    try
                    {
                        // resize the image to MAX pixels by MAX  
                        // pixels to server as the preview image
                        using (var input = new MemoryStream(buffer))
                        using (var output = new MemoryStream())
                        {
                            var job =
                                new ImageJob(
                                    input,
                                    output,
                                    new Instructions($"?height={MAX}&width={MAX}&mode=crop&quality={QUALITY}&format=jpg")
                                );
                            job.Build();
                            result = output.ToArray();
                        }
                    }
                    catch
                    {
                        result = null;
                    }
                }
            }

            return result;

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
            string hash,
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
            }
        }
    }
}