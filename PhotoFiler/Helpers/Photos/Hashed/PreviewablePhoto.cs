using ImageResizer;
using PhotoFiler.Models;
using System;
using System.IO;

namespace PhotoFiler.Helpers.Photos.Hashed
{
    public class PreviewablePhoto : HashedPhoto, IPreviewableHashedPhoto
    {
        // JPEG compression quality
        private const int QUALITY = 50;

        // Maximum height and width
        private const int MAX = 300;

        private DirectoryInfo _PreviewLocation;

        public PreviewablePhoto(
            int hashLength,
            string path,
            IHasher hasher,
            DirectoryInfo previewLocation
        ) : base(hashLength, path, hasher)
        {
            if (previewLocation == null)
                throw new ArgumentNullException(nameof(path));

            _PreviewLocation = previewLocation;
        }

        /// <summary>
        /// Byte array content of the photo
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public byte[] View()
        {
            return System.IO.File.ReadAllBytes(FileInfo.FullName);
        }

        /// <summary>
        /// Generates the preview of the image file
        /// </summary>
        /// <param name="hash">Hash of the photo</param>
        /// <returns>Byte array content of the photo preview</returns>
        public byte[] Preview()
        {
            byte[] result = Preview(Hash);
            if (result == null)
            {
                result = Preview(FileInfo);
            }

            return result;
        }

        private string PreviewFile
        {
            get
            {
                return Path.ChangeExtension(Path.Combine(_PreviewLocation.FullName, Hash), "prev");
            }
        }

        /// <summary>
        /// Reads the preview file from the preview location if it the photo exists
        /// </summary>
        /// <param name="hash">Filename of the preview file derived from the hash of the photo</param>
        /// <returns>Byte array of the preview file if it exists, otherwise null</returns>
        private byte[] Preview(string hash)
        {
            byte[] result = null;

            var previewFile = Path.ChangeExtension(Path.Combine(_PreviewLocation.FullName, hash), "prev");
            if (File.Exists(PreviewFile))
            {
                result = File.ReadAllBytes(previewFile);
            }

            return result;
        }

        /// <summary>
        /// Generates a preview file of the photo
        /// </summary>
        /// <param name="fileInfo">FileInfo of the photo to create the preview from</param>
        /// <returns>Byte array of the preview file of the photo if it exists, otherwise retuls null</returns>
        private byte[] Preview(FileInfo fileInfo)
        {
            byte[] result = null;

            try
            {
                byte[] buffer = File.ReadAllBytes(fileInfo.FullName);

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

            return result;
        }
    }
}