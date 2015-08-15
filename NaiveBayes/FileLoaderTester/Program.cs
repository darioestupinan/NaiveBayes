using ArffFileProcesser;
using FileLoader;
using NaiveBayes;
using System;
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
                var naiveBayes = new NaiveBayes.NaiveBayes(processedFile, null, "Drug");
                naiveBayes.TrainFromSet();

                var testModel = new TestDataModel();
                var result = naiveBayes.TestNewData(testModel.TestData);

                Console.WriteLine("the recommended drug is : " + result.ResultAttribute);
                Console.WriteLine("the porcentage of accuracy is: " + result.Values.ToString());
                Console.ReadKey();
            };
        }
    }
}
