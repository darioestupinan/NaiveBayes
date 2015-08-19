using ArffFileProcesser;
using System.Collections.Generic;
using System;

namespace NaiveBayes
{
    public class NaiveBayesModel
    {
        private ArffModel _dataModel;
        private string _targetAttribute;
        private IList<AttributeValue> _targetAttributeClasses = new List<AttributeValue>();
        private IList<IOcurrenceModel> _ocurrenceMatrix = new List<IOcurrenceModel>();

        public ArffModel DataModel
        {
            get { return _dataModel; }
            set { _dataModel = value; }
        }

        public string TargetAttribute
        {
            get { return _targetAttribute; }
            set { _targetAttribute = value;}
        }

        public IList<AttributeValue> TargetAttributeClasses
        {
            get { return _targetAttributeClasses; }
            set { _targetAttributeClasses = value; }
        }

        public IList<IOcurrenceModel> OcurrenceMatrix
        {
            get { return _ocurrenceMatrix; }
            set { _ocurrenceMatrix = value; }
        }

        public void AddOcurrenceMatrixRange(IList<IOcurrenceModel> ocurrenceList)
        {
            (_ocurrenceMatrix as List<IOcurrenceModel>).AddRange(ocurrenceList);
        }
    }

    public class AttributeValue
    {
        public string ClassName { get; set; }
        public double Probability { get; set; }
        public double Precision { get; set; }
        public double ThirdStd { get; set; }
    }
    
    public class OcurrenceModelFactory
    {
        private Dictionary<string, IOcurrenceModel> OcurrenceDictionary = new Dictionary<string, IOcurrenceModel>
        {
            { "continuous", new ContinousAttributeOcurrenceModel() },
            { "discrete", new DiscreteAttributeOcurrenceModel() }
        };

        public IOcurrenceModel GetInstance(string type)
        {
            if (string.IsNullOrEmpty(type)) return null;
            else
            {
                if (OcurrenceDictionary.ContainsKey(type)) return OcurrenceDictionary[type];
                else return null;
            }
        }
    }

    public interface IOcurrenceModel
    {
        bool IsContinuous { get; }
        string GetAttributeName();
        string GetAttributeValue();
        string GetTargetClass();
        IList<double> GetValues();
    }

    public abstract class OcurrenceModelBase 
    {
        public string CurrentAttributeName { get; set; }
        public string CurrentAttributeValue { get; set; }
        public string TargetAttributeClassName { get; set; }
        public abstract bool IsContinuous { get; }

        public string GetAttributeName()
        {
            return CurrentAttributeName;
        }

        public string GetAttributeValue()
        {
            return CurrentAttributeValue;
        }

        public string GetTargetClass()
        {
            return TargetAttributeClassName;
        }

    }

    public class ContinousAttributeOcurrenceModel : OcurrenceModelBase, IOcurrenceModel
    {
        public double Mean { get; set; }
        public double StdDev { get; set; }
        public double WeightSum { get; set; }
        public double Precision { get; set; }
        public override bool IsContinuous { get { return true; } }

        

        public IList<double> GetValues()
        {
            var listValues = new List<double>
            {
                Mean,StdDev,WeightSum,Precision
            };
            return listValues;
        }
    }

    public class DiscreteAttributeOcurrenceModel : OcurrenceModelBase, IOcurrenceModel
    {
        public double Value { get; set; }
        public override bool IsContinuous { get { return false; } }

        public IList<double> GetValues()
        {
            var listValues = new List<double> { Value };
            return listValues;
        }
    }
}
