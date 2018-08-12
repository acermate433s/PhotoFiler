using System.IO;

using static PhotoFiler.Photo.Helpers;

namespace PhotoFiler.Photo
{
    public interface IImageResizerService
    {
        event ErrorGeneratingPreviewEventHandler ErrorGeneratingPreviewHandler;

        Stream Stream { get; set; }

        byte[] Resize(int height, int width, byte quality);
    }
}
