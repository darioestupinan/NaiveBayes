using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileLoader
{
    public interface IFileLoader
    {
        string LoadArffFile(string path);
    }

    public class FileLoader : IFileLoader
    {      
        public string LoadArffFile(string path)
        {
            var completeroute = string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                var defaultPath = ConfigurationManager.AppSettings["defaultPath"];
                var defaultFile = ConfigurationManager.AppSettings["defaultFile"];
                path = defaultPath + "/" + defaultFile;
            }
            try
            {
                var reader = new StreamReader(path);
                completeroute = reader.ReadToEnd();
                return completeroute;
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }                
        }
    }
}
