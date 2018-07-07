namespace Photo.Models
{
    /// <summary>
    /// Hashing function
    /// </summary>
    public interface IHashFunction
    {
        /// <summary>
        /// Generate a hash
        /// </summary>
        /// <param name="text">String to hash</param>
        /// <param name="length">Number of characters for the hash</param>
        /// <returns></returns>
        string Compute(string text);

        /// <summary>
        /// Maximum no. of characters in the hash code
        /// </summary>
        int HashLength { get; }
    }
}