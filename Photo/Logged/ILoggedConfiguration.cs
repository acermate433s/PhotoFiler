namespace Photo.Logged
{
    public interface ILoggedConfiguration
    {
        /// <summary>
        /// Flag to indicate if logging is generated
        /// </summary>
        bool EnableLogging { get; set; }
    }
}
