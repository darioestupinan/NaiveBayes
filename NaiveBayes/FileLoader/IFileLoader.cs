using System.Configuration;
using System.Text;
using System.IO;
using System;
using Newtonsoft.Json;

namespace FileLoader
{
    public interface IFileLoader
    {
        TextReader LoadArffFile(string path);
        string GetPath(string path);
        void SaveJsonFileToText(string json, string path);
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

        public void SaveJsonFileToText(string json, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                var defaultPath = ConfigurationManager.AppSettings["defaultPath"];
                var defaultFile = ConfigurationManager.AppSettings["defaultJsonFile"];
                var defaultFiletype = ConfigurationManager.AppSettings["defaultJsonType"];
                var timestamp = System.DateTime.Now.ToString("H-mm-ss-dd-MM-yyyy");
                path = defaultPath + "/" + defaultFile + "-" + timestamp + defaultFiletype;
            }
            using (var streamWriter = new StreamWriter(path, false, Encoding.UTF8))
            {
                streamWriter.Write(json);
            }
        }
    }
}
