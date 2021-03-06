using System.Configuration;
using System.IO;
using System.Text;

namespace FileLoader
{
    public class FileLoader : IFileLoader
    {
        public TextReader LoadArffFile(string path)
        {
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

        public void SaveJsonFileToText(string json, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                var defaultPath = ConfigurationManager.AppSettings["defaultPath"];
                var defaultFile = ConfigurationManager.AppSettings["defaultJsonFile"];
                var defaultFiletype = ConfigurationManager.AppSettings["defaultJsonType"];
                path = defaultPath + "/" + defaultFile + defaultFiletype;
            }
            using (var streamWriter = new StreamWriter(path, false, Encoding.UTF8))
            {
                streamWriter.Write(json);
            }
        }
    }
}