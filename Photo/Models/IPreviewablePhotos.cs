using System.Collections.Generic;
using static Photo.Helpers.Helpers;

namespace Photo.Models
{
    /// <summary>
    /// List of previewable photos
    /// </summary>
    public interface IPreviewablePhotos
    {
        List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreview errorGeneratingPreviewHandler = null);
    }
}
