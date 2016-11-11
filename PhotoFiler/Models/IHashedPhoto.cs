using System.ComponentModel;

namespace PhotoFiler.Models
{
    /// <summary>
    /// Hashable photo
    /// </summary>
    public interface IHashedPhoto : IPhoto
    {
        /// <summary>
        /// Hash for the photo
        /// </summary>
        [DisplayName("Hash")]
        string Hash { get; }
    }
}