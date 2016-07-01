using PhotoFiler.Helpers;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoFiler.Helper
{
    public class PhotosPreviewer : Photos
    {
        public PhotosPreviewer(string path, int hashLength, string previewLocation = "") : base(path, hashLength, previewLocation)
        {
        }

        public FileInfo FileInfo(string hash)
        {
            return this[hash].FileInfo;
        }

        /// <summary>
        /// Byte array content of the photo
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public byte[] View(string hash)
        {
            if (ContainsKey(hash))
            {
                return System.IO.File.ReadAllBytes(FileInfo(hash).FullName);
            }
            else
                return null;
        }

        /// <summary>
        /// Byte array content of the photo
        /// </summary>
        /// <param name="hash">Hash of the photo</param>
        /// <returns></returns>
        public byte[] Preview(string hash)
        {
            byte[] value = null;

            if (ContainsKey(hash))
            {
                var filename = Path.Combine(PreviewLocation, hash);
                filename = Path.ChangeExtension(filename, "prev");

                if (Directory.Exists(PreviewLocation) && File.Exists(filename))
                    value = System.IO.File.ReadAllBytes(filename);
                else
                    value = this[hash].Preview();
            }

            return value;
        }

        /// <summary>
        /// Creates preview files 
        /// </summary>
        /// <returns>True if successful otherwise false</returns>
        public bool CreatePreviews()
        {
            bool value = true;

            if (Directory.Exists(PreviewLocation))
            {
                this
                    .Values
                    .AsParallel()
                    .ForAll(item =>
                    {
                        var filename = Path.ChangeExtension(Path.Combine(PreviewLocation, item.Hash), "prev");
                        if (File.Exists(filename))
                            File.Delete(filename);

                        var buffer = item.Preview();
                        if (buffer != null)
                            File
                                .WriteAllBytes(
                                    filename,
                                    buffer
                                );
                    });
            }
            else
                value = false;

            return value;
        }
    }
}