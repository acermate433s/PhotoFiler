using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Models
{
    public interface IPreviewablePhoto : IHashedPhoto
    {
        event ErrorGeneratingPreview ErrorGeneratingPreviewHandler;

        byte[] Preview();

        byte[] View();
    }
}