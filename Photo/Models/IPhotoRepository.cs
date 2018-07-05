using Photo.Models;
using System.IO;
using static Photo.Helpers.Helpers;

namespace Photo.Models
{
    /// <summary>
    /// IPhoto Repository
    /// </summary>
    public interface IPhotoRepository
    {
        IPreviewablePhoto Create(
            FileInfo file, 
            ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null
        );
    }
}
