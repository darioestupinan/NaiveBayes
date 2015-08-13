using ArffFileProcesser;
using System.Collections.Generic;

namespace NaiveBayes
{
    public class NaiveBayesModel
    {
        private ArffModel _originalData;
        private string _targetAttribute;
        private List<AttributeValue> _targeAttributePercentage;


    }

    public class AttributeValue
    {
        public string ClassName { get; set; }
        public double Probability { get; set; }
    }

    public interface IOcurrenceModel
    {       
        
    }

    public abstract class OcurrenceModelBase
    {
        public double CurrentAttributeName { get; set; }
        public AttributeValue TargetAttributeValues { get; set; }
        public abstract bool IsContinuous { get; }
    }

    public class ContinousAttributeOcurrenceModel: OcurrenceModelBase
    {
        public double Mean { get; set; }
        public double StdDev { get; set; }
        public double WeightSum { get; set; }
        public double Total { get; set; }
        public override bool IsContinuous { get { return true; } }        
    }

    public class DiscreteAttributeOcurrenceModel: OcurrenceModelBase
    {
        public double Total { get; set; }
        public override bool IsContinuous { get { return false; } }
    }
}
