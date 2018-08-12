using System;
using System.IO;

using PhotoFiler.Photo.Models;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.FileSystem
{
    public class FileSystemPreviewablePhoto : FileSystemPhoto, IPreviewablePhoto
    {
        public event ErrorGeneratingPreviewEventHandler ErrorGeneratingPreviewHandler;

        private readonly IImageResizerService imageResizer;

        // JPEG compression quality
        private const int QUALITY = 50;

        // Maximum height and width
        private const int MAX = 300;

        public FileSystemPreviewablePhoto(
            string path,
            IHashFunction hasher,
            IExifReaderService exifReader,
            IImageResizerService imageResizer
        ) : base(path, hasher, exifReader)
        {
            this.imageResizer = imageResizer ?? throw new ArgumentNullException(nameof(imageResizer));
        }

        /// <summary>
        /// Byte array content of the photo
        /// </summary>
        /// <returns></returns>
        public byte[] View()
        {
            if (File.Exists(Location))
                return System.IO.File.ReadAllBytes(Location);
            else
                return null;
        }

        /// <summary>
        /// Reads the preview file from the preview location if it the photo exists
        /// </summary>
        /// <returns>Byte array of the preview file if it exists, otherwise null</returns>
        public byte[] Preview()
        {
            byte[] result = null;

            try
            {
                byte[] buffer = File.ReadAllBytes(Location);
                result = this.imageResizer.Resize(MAX, MAX, QUALITY);
            }
            catch (Exception ex)
            {
                var args = new ErrorGeneratingPreviewEventArgs
                {
                    Photo = this,
                    Exception = ex
                };

                ErrorGeneratingPreviewHandler?.Invoke(this, args);
                result = null;
            }

            return result;
        }
    }
}