using System;
using System.IO;

using ExifLib;

using PhotoFiler.Photo;

namespace PhotoFiler.Web.Helpers
{
    public class ExifReaderService : IExifReaderService, IDisposable
    {
        private Stream stream = null;
        private ExifReader exifReader = null;

        public ExifReaderService(Stream stream = null)
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

        #region IDisposable Support

        private bool disposing = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposing)
            {
                if (disposing)
                {
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