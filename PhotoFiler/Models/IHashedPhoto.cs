using System.ComponentModel;

namespace PhotoFiler.Models
{
    public interface IHashedPhoto : IPhoto
    {
        [DisplayName("Hash")]
        string Hash { get; }
    }
}