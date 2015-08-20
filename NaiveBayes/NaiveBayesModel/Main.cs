using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArffFileProcesser;
using FileLoader;
using Newtonsoft.Json;

namespace NaiveBayesModel
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"EDSI 2015 Dario Estupiñan version 1.0", @"About", MessageBoxButtons.OK);
        }

        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var start = System.DateTime.Now;
              
            IFileLoader fileLoader = new FileLoader.FileLoader();
            var arffFilePath = fileLoader.GetPath(string.Empty);

            IFileProcesser<string> fileProcesser = new SimpleFileProcceser();
            using (var textReader = new StreamReader(arffFilePath, Encoding.UTF8))
            {
                var processedFile = fileProcesser.Process(textReader.ReadToEnd());
                var naiveBayes = new NaiveBayes.NaiveBayes(processedFile, "Drug");
                naiveBayes.TrainFromSet();
                fileLoader.SaveJsonFileToText(JsonConvert.SerializeObject(naiveBayes.GetModel()), string.Empty);
            }

            var end = System.DateTime.Now;
            var difference = end.Subtract(start).TotalMilliseconds;
            MessageBox.Show(@"Processed! in " + difference + " milliseconds", @"Time elapsed", MessageBoxButtons.OK);
        }
    }
}
