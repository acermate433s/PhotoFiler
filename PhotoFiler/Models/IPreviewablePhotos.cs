using System.Collections.Generic;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Models
{
    /// <summary>
    /// List of previewable photos
    /// </summary>
    public interface IPreviewablePhotos
    {
        List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreview errorGeneratingPreviewHandler = null);
    }
}
