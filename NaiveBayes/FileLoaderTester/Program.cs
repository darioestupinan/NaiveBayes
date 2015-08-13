using ArffFileProcesser;
using FileLoader;
using NaiveBayes;
using System.IO;
using System.Text;

namespace FileLoaderTester
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileLoader fileLoader = new FileLoader.FileLoader();
            var arffFilePath = fileLoader.GetPath(string.Empty);

            IFileProcesser<string> fileProcesser = new SimpleFileProcceser();
            using (var textReader = new StreamReader(arffFilePath, Encoding.UTF8))
            {
                var processedFile = fileProcesser.Process(textReader.ReadToEnd());
            }
            //var proccessedFile = fileProcesser.Process(arffFile.ReadToEnd());

            var naiveBayesClassifier = new NaiveBayes<string, string>(fileProcesser);
            naiveBayesClassifier.Load(arffFilePath);
            
        }
    }
}
