namespace PhotoFiler.Helpers
{
    public interface IHashedPhoto : IPhoto 
    {
        string Hash { get; }
    }
}
