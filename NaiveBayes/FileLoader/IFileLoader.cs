using System.Configuration;
using System.Text;
using System.IO;

namespace FileLoader
{
    public interface IFileLoader
    {
        TextReader LoadArffFile(string path);
        string GetPath(string path);
    }

    public class FileLoader : IFileLoader
    {
        public TextReader LoadArffFile(string path)
        {
            var completeroute = string.Empty;
            path = GetPath(path);
            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            {
                return streamReader;
            }
        }

        public string GetPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                var defaultPath = ConfigurationManager.AppSettings["defaultPath"];
                var defaultFile = ConfigurationManager.AppSettings["defaultFile"];
                path = defaultPath + "/" + defaultFile;
            }
            return path;
        }
    }
}
