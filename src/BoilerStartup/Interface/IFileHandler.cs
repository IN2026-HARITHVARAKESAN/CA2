namespace BoilerStartup.Interface
{
    /// <summary>
    /// Handle File write
    /// </summary>
    internal interface IFileHandler
    {
        /// <summary>
        /// Append log data into file
        /// </summary>
        /// <param name="message">log message</param>
        public void AppendDataToFile(string message);
    }
}
