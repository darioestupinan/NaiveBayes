using System;
using System.Collections.Generic;
using System.Linq;


namespace NaiveBayes
{
    public interface IDistribution
    {
        double GetMean();
        double GetStandardDeviation();
        double GetWeightSum();
        double GetPrecision();
    }

    public class StandardDeviation : IDistribution
    {
        private IList<double> _values;

        public StandardDeviation(IList<double> values)
        {
            _values = values;
        }

        public double GetMean()
        {
            return _values.Average();
        }

        public double GetStandardDeviation()
        {
            double stddev = 0;
            if (_values.Count > 0)
            {
                var average = _values.Average();
                double sum = _values.Sum(d => Math.Pow(d - average, 2));
                stddev = Math.Sqrt((sum) / (_values.Count() - 1));
            }
            return stddev;
        }

        public double GetPrecision()
        {
            return 0;
        }

        public double GetWeightSum()
        {
            return _values.Count;
        }
    }
}
