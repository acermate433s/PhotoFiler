using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Models
{
    /// <summary>
    /// Previewable photo
    /// </summary>
    /// <seealso cref="PhotoFiler.Models.IPhoto" />
    public interface IPreviewablePhoto : IPhoto
    {
        event ErrorGeneratingPreview ErrorGeneratingPreviewHandler;

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