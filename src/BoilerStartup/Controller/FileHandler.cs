using System.Text;
using BoilerStartup.Interface;

namespace BoilerStartup.Controller
{
    /// <summary>
    /// Handles file read and write
    /// </summary>
    internal class FileHandler : IFileHandler
    {
        private string? FilePath { get; set; } = @"./../../../Assets/Log.txt";

        /// <inheritdoc/>
        public void AppendDataToFile(string message)
        {
            string? directoryPath = Path.GetDirectoryName(this.FilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(this.FilePath))
            {
                using (FileStream fs = new FileStream(this.FilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes("Timestamp, Event, Event Data");
                    fs.Write(byteArray, 0, byteArray.Length);
                }
            }

            using (FileStream fs = new FileStream(this.FilePath, FileMode.Append, FileAccess.Write))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(System.Environment.NewLine + message);
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }
    }
}
