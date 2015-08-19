using System.Data.Objects;

namespace NaiveBayes
{
    public interface IDistribution
    {
        double GetMean();
        double GetStandardDeviation();
        double GetWeightSum();
    }
}
