using System;
using System.Collections.Generic;
using System.IO;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Models
{
    /// <summary>
    /// Photo album of hashed photos
    /// </summary>
    public interface IHashedAlbum
    {
        /// <summary>
        /// Photos in the album
        /// </summary>
        IList<IPreviewablePhoto> Photos { get; }

        /// <summary>
        /// Location of the directory where the generated preview of the photos are stored.
        /// </summary>
        DirectoryInfo PreviewLocation { get; }

        /// <summary>
        /// Number of photos in the album
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Generate previews of all the photos in the album
        /// </summary>
        void GeneratePreviews();

        /// <summary>
        /// Retrieves photos by page
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count">Maximum number of photos per page</param>
        /// <returns></returns>
        IEnumerable<IPreviewablePhoto> List(int page = 1, int count = 10);

        /// <summary>
        /// Get the photo in the album using the hash
        /// </summary>
        /// <param name="hash">Hash generated for the photo</param>
        /// <returns></returns>
        IPreviewablePhoto Photo(string hash);
    }
}