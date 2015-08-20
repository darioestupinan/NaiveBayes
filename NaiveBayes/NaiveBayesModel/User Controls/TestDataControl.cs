using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArffFileProcesser;
using FileLoader;
using NaiveBayes;
using NaiveBayesModel.Model;

namespace NaiveBayesModel.User_Controls
{
    public partial class TestDataControl : UserControl
    {
        private double _age;
        private string _sex;
        private string _bp;
        private string _cholesterol;
        private double _na;
        private double _k;
 
        public TestDataControl()
        {
            InitializeComponent();
        }
        
        private void buttonTest_Click(object sender, EventArgs e)
        {
            bool succeded;
            succeded = Double.TryParse(textBoxAge.Text, out _age);
            if (!succeded) _age = 0;
            _sex = textBoxSex.Text;
            _bp = textBoxBP.Text;
            _cholesterol = textBoxCholesterol.Text;
            succeded = Double.TryParse(textBoxNa.Text, out _na);
            if (!succeded) _na = 0;
            succeded = Double.TryParse(textBoxK.Text, out _k);
            if (!succeded) _k = 0;

            var testList = new List<object>()
            {
                _age, _sex, _bp, _cholesterol, _na, _k, ""
            };

            var testModel = new Model.TestDataModel(testList);

            var result = ExecuteNaiveClassiffication(testModel);

            MessageBox.Show(@"result: " + result.ResultAttribute + @", prob: " + result.Values);
        }

        private ResultModel ExecuteNaiveClassiffication(TestDataModel testModel)
        {
            IFileLoader fileLoader = new FileLoader.FileLoader();
            var arffFilePath = fileLoader.GetPath(string.Empty);

            IFileProcesser<string> fileProcesser = new SimpleFileProcceser();
            using (var textReader = new StreamReader(arffFilePath, Encoding.UTF8))
            {
                var processedFile = fileProcesser.Process(textReader.ReadToEnd());
                var naiveBayes = new NaiveBayes.NaiveBayes(processedFile, "Drug");
                naiveBayes.TrainFromSet();
                var currentResult = naiveBayes.TestNewData(testModel.TestData);
                return currentResult;
            }
        }
    }
}
