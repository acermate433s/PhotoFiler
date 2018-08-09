using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Models
{
    /// <summary>
    /// Previewable photo
    /// </summary>
    /// <seealso cref="PhotoFiler.Photo.Models.IPhoto" />
    public interface IPreviewablePhoto : IPhoto
    {
        event ErrorGeneratingPreviewEventHandler ErrorGeneratingPreviewHandler;

        /// <summary>
        /// Get the generated preview of the photo using the hash 
        /// </summary>
        /// <returns></returns>
        byte[] Preview();

        /// <summary>
        /// Get the full photo using the hash
        /// </summary>
        /// <returns></returns>
        byte[] View();
    }
}