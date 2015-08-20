using System.Collections.Generic;
using NaiveBayes;

namespace NaiveBayesModel.Model
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

        public TestDataModel(IList<object> row)
        {
            var Age = new TestData();
            Age.DataName = "Age";
            Age.Value = (double) row[0];

            var Sex = new TestData();
            Sex.DataName = "Sex";
            Sex.Value = (string) row [1];

            var BP = new TestData();
            BP.DataName = "BP";
            BP.Value = (string) row[2];

            var Cholesterol = new TestData();
            Cholesterol.DataName = "Cholesterol";
            Cholesterol.Value = (string) row[3];

            var Na = new TestData();
            Na.DataName = "Na";
            Na.Value = (double) row[4];

            var K = new TestData();
            K.DataName = "K";
            K.Value = (double) row[5];

            var Droug = new TestData();
            K.DataName = "Droug";
            K.Value = (string) row[6];

            _testData = new List<TestData>()
            {
                Age, Sex, BP, Cholesterol, Na, K, Droug
            };
        }
    }
}
