using ArffFileProcesser;
using FileLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoaderTester
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileLoader fileLoader = new FileLoader.FileLoader();
            string arffFile = fileLoader.LoadArffFile(string.Empty);

            IFileProcesser<string> fileProcesser = new SimpleFileProcceser();
            var proccessedFile = fileProcesser.Process(arffFile);
        }
    }
}
