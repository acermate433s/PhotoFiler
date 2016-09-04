namespace PhotoFiler.Models
{
    public interface IPreviewablePhoto : IHashedPhoto
    {
        byte[] Preview();

        byte[] View();
    }
}