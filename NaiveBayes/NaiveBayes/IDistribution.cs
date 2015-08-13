using System;
using System.Collections.Generic;
using System.Linq;


namespace NaiveBayes
{
    public interface IDistribution
    {
        double ApplyDistribution(IEnumerable<double> numbers);
    }

    public class StandardDeviation : IDistribution
    {
        public double ApplyDistribution(IEnumerable<double> numbers)
        {
            var numbersList = numbers.ToList();
            var totalCount = numbers.Count();
            return 1;
        }
    }
}
