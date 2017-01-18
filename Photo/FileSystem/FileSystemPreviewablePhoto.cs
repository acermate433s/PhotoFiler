using ImageResizer;
using Photo.Helpers;
using Photo.Models;
using System;
using System.IO;
using static Photo.Helpers.Helpers;

namespace Photo.FileSystem
{
    public class FileSystemPreviewablePhoto : FileSystemPhoto, IPreviewablePhoto
    {
        public event ErrorGeneratingPreview ErrorGeneratingPreviewHandler;

        // JPEG compression quality
        private const int QUALITY = 50;

        // Maximum height and width
        private const int MAX = 300;

        public FileSystemPreviewablePhoto(
            int hashLength,
            string path,
            IHashFunction hasher
        ) : base(path, hasher)
        {
            if (hashLength <= 0)
                throw new ArgumentException("Hash length must be greater than zero.", nameof(path));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (hasher == null)
                throw new ArgumentNullException(nameof(hasher));
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
            catch (Exception ex)
            {
                var args = new ErrorGeneratingPreviewArgs();
                args.Photo = this;
                args.Exception = ex;

                ErrorGeneratingPreviewHandler?.Invoke(this, args);
                result = null;
            }

            return result;
        }
    }
}