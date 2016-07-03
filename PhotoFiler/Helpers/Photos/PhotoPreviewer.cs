using ImageResizer;
using System.IO;

namespace PhotoFiler.Helpers
{
    public class PhotoPreviewer<IHasher>
    {
        // JPEG compression quality
        private const int QUALITY = 50;

        // Maximum height and width
        private const int MAX = 300;

        public IHashedPhoto Photo { get; set; }
        public DirectoryInfo PreviewLocation { get; set; }

        public PhotoPreviewer(
            IHashedPhoto photo,
            DirectoryInfo previewLocation
        )
        {
            Photo = photo;
            PreviewLocation = previewLocation;
        }

        /// <summary>
        /// Byte array content of the photo
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public byte[] View()
        {
            return System.IO.File.ReadAllBytes(Photo.FileInfo.FullName);
        }

        /// <summary>
        /// Generates the preview of the image file
        /// </summary>
        /// <param name="hash">Hash of the photo</param>
        /// <returns>Byte array content of the photo preview</returns>
        public byte[] Preview()
        {
            byte[] result = Preview(Photo.Hash);
            if (result == null)
            {
                result = Preview(Photo.FileInfo);
            }

            return result;
        }

        private string PreviewFile
        {
            get
            {
                return Path.ChangeExtension(Path.Combine(PreviewLocation.FullName, Photo.Hash), "prev");
            }
        }

        public void Generate()
        {
            if (!File.Exists(PreviewFile))
            {
                File.WriteAllBytes(PreviewFile, Preview(Photo.FileInfo));
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

            var previewFile = Path.ChangeExtension(Path.Combine(PreviewLocation.FullName, hash), "prev");
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

            return result;
        }
    }
}