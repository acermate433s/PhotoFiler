using System.Collections.Generic;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo.Models
{
    /// <summary>
    /// List of previewable photos
    /// </summary>
    public interface IPreviewablePhotos
    {
        List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null);
    }
}
