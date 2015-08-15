using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;



namespace NaiveBayes
{
    public interface IDistribution
    {
        double GetMean();
        double GetStandardDeviation();
        double GetWeightSum();
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
            double M = 0.0;
            double S = 0.0;
            int k = 1;
            foreach (double value in _values)
            {
                double tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;
            }
            return Math.Sqrt(S / (k - 1));
        }
        

        public double GetWeightSum()
        {
            return _values.Count;
        }
    }
}
