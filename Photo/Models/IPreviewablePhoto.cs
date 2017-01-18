using static Photo.Helpers.Helpers;

namespace Photo.Models
{
    /// <summary>
    /// Previewable photo
    /// </summary>
    /// <seealso cref="Photo.Models.IPhoto" />
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