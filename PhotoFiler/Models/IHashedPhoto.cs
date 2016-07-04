namespace PhotoFiler.Models
{
    public interface IHashedPhoto : IPhoto
    {
        string Hash { get; }
    }
}