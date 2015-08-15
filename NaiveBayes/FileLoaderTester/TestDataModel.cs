using NaiveBayes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoaderTester
{
    public class TestDataModel
    {
        private IList<TestData> _testData;

        public IList<TestData> TestData { get { return _testData; } }

        public TestDataModel()
        {
            var Age = new TestData();
            Age.DataName = "Age";
            Age.Value = 60;

            var Sex = new TestData();
            Sex.DataName = "Sex";
            Sex.Value = "M";

            var BP = new TestData();
            BP.DataName = "BP";
            BP.Value = "NORMAL";

            var Cholesterol = new TestData();
            Cholesterol.DataName = "Cholesterol";
            Cholesterol.Value = "HIGH";

            var Na = new TestData();
            Na.DataName = "Na";
            Na.Value = 0.77721;

            var K = new TestData();
            K.DataName = "K";
            K.Value = 0.05123;

            _testData = new List<TestData>()
            {
                Age, Sex, BP, Cholesterol, Na, K
            };
        }
    }
}
