using System;
using System.IO;

using ExifLib;
using ImageResizer;

using PhotoFiler.Photo;
using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Web.Helpers
{
    public class ImageServices : IImageResizerService, IExifReaderService, IDisposable
    {
        public event ErrorGeneratingPreviewEventHandler ErrorGeneratingPreviewHandler;

        private Stream stream = null;
        private ExifReader exifReader = null;

        public ImageServices(Stream stream = null)
        {
            if (stream != null)
            {
                this.exifReader = new ExifReader(stream);
            }
            else
            {
                this.stream = stream;
            }
        }

        public Stream Stream
        {
            get
            {
                return this.stream;
            }
            set
            {
                this.stream = value;
                if (value != null)
                {
                    this.exifReader = this.stream != null ? new ExifReader(value) : null;
                }
            }
        }

        public DateTime? DateTime()
        {
            if (this.exifReader == null)
            {
                this.exifReader.GetTagValue<DateTime>(ExifTags.DateTime, out var result);
                return result;
            }
            else
            {
                return null;
            }
        }

        public byte[] Resize(int height, int width, byte quality)
        {
            if (this.stream == null)
            {
                return null;
            }

            byte[] result = null;
            ImageJob job;

            try
            {
                // resize the image to MAX pixels by MAX
                // pixels to server as the preview image
                using (var input = this.stream)
                using (var output = new MemoryStream())
                {
                    job = new ImageJob(
                        input,
                        output,
                        new Instructions($"?height={height}&width={width}&mode=crop&quality={quality}&format=jpg")
                    );
                    job.Build();

                    result = output.ToArray();
                }
            }
            catch (Exception ex)
            {
                var args = new ErrorGeneratingPreviewEventArgs
                {
                    Exception = ex
                };

                ErrorGeneratingPreviewHandler?.Invoke(this, args);
                result = null;
            }

            return result;
        }

        #region IDisposable Support

        private bool disposing = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposing)
            {
                if (disposing)
                {
                    this.Stream.Dispose();
                    this.exifReader.Dispose();                    
                }
                this.disposing = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}