using ArffFileProcesser;
using FileLoader;
using NaiveBayes;
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
            var arffFilePath = fileLoader.GetPath(string.Empty);

            IFileProcesser<string> fileProcesser = new SimpleFileProcceser();
            //var proccessedFile = fileProcesser.Process(arffFile.ReadToEnd());

            var naiveBayesClassifier = new NaiveBayes<string, string>(fileProcesser);
            naiveBayesClassifier.Load(arffFilePath);
            
        }
    }
}
