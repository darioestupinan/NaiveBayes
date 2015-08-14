#region Namespaces
using ArffFileProcesser;
using FileLoader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#endregion

namespace NaiveBayes
{
    public class NaiveBayes 
    {
        #region Attributes

        private ArffModel _model = new ArffModel();
        private ArffModel _cache = new ArffModel();

        private readonly IDistribution _distribution;

        private readonly OcurrenceModelFactory _ocurrenceModelFactory = new OcurrenceModelFactory();

        private readonly List<string> _ignoreAttributes;

        private string _targetAttribute;

        #endregion

        #region Public Properties
        #endregion

        #region Constructors

        public NaiveBayes(ArffModel modelFromFile, IDistribution distribution, List<string> ignoredAttributes, string targetAttribute)
        {
            _model = modelFromFile;
            _distribution = distribution;
            _ignoreAttributes = ignoredAttributes;
            _targetAttribute = targetAttribute;
        }

        #endregion

        #region Public Methods

        public void TrainFromSet()
        {           
            var naiveBayesModel = new NaiveBayesModel();
            naiveBayesModel.OriginalData = _model;
            naiveBayesModel.TargetAttribute = _targetAttribute;
            //first identify the possible values of the target Attribute, 
            var targetAttribute = _model.Attributes.Where(attrib => attrib.Name.Equals(_targetAttribute, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var targetAttributeClassesCount = targetAttribute.Definition.Count();
            var totalData = targetAttribute.Values.Count();
            //calculate probability of the classes identified
            foreach (var definitionClass in targetAttribute.Definition)
            {
                var current = new AttributeValue();
                current.ClassName = definitionClass;
                current.Probability = (double)targetAttribute.Values.Where(val => val.Equals(definitionClass, StringComparison.InvariantCultureIgnoreCase)).Count() / (double)totalData;
                naiveBayesModel.TargetAttributeClasses.Add(current);
            }
            //calculate possibilities foreach class within each value of other attribute
            //find the target attribute index
            var targetAttribIndex = _model.Attributes.ToList().FindIndex(attr => attr.Name.Equals(naiveBayesModel.TargetAttribute, StringComparison.InvariantCultureIgnoreCase));
            for (var i=0; i< _model.Attributes.Count(); i++)
            {
                var attribute = _model.Attributes[i];
                var currentAttributteName = attribute.Name;
                if (attribute.Name.Equals(_targetAttribute, StringComparison.InvariantCultureIgnoreCase)) continue;
                if (attribute.Definition.Count() == 1 && attribute.Definition[0].Equals("Real", StringComparison.InvariantCultureIgnoreCase))
                {
                    //continuous
                    foreach (var classes in naiveBayesModel.TargetAttributeClasses)
                    {
                        var ocurrenceModelList = new List<IOcurrenceModel>();
                        var selectedData = _model.Data.Cast<List<object>>().Where(item => item[targetAttribIndex].ToString().Trim().Equals(classes.ClassName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                        for (var j = 0; j < attribute.Definition.Count; j++)
                        {
                            var ocurrenceModel = new ContinousAttributeOcurrenceModel();
                            ocurrenceModel.CurrentAttributeName = attribute.Name;
                            ocurrenceModel.TargetAttributeClassName = classes.ClassName;
                            var selectedItemsValues = selectedData.Cast<List<object>>().Select(s => (double)s[i]).ToList();
                            var currentDistribution = new StandardDeviation(selectedItemsValues);
                            ocurrenceModel.Mean = currentDistribution.GetMean();
                            ocurrenceModel.StdDev = currentDistribution.GetStandardDeviation();
                            ocurrenceModel.WeightSum = currentDistribution.GetWeightSum();
                            ocurrenceModel.Precision = currentDistribution.GetPrecision();
                            ocurrenceModelList.Add(ocurrenceModel);
                        }
                        naiveBayesModel.AddOcurrenceMatrixRange(ocurrenceModelList);
                    }
                }
                else
                {
                    //discrete
                    foreach (var classes in naiveBayesModel.TargetAttributeClasses)
                    {
                        var ocurrenceModelList = new List<IOcurrenceModel>();
                        var selectedData = _model.Data.Cast<List<object>>().Where(item => item[targetAttribIndex].ToString().Trim().Equals(classes.ClassName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                        for (var j = 0; j < attribute.Definition.Count; j++)
                        {
                            var ocurrenceModel = new DiscreteAttributeOcurrenceModel();
                            ocurrenceModel.CurrentAttributeName = currentAttributteName + " " + attribute.Definition[j];
                            ocurrenceModel.TargetAttributeClassName =  classes.ClassName;
                            ocurrenceModel.Value = selectedData.Cast<List<object>>().Where(item => item[i].ToString().Equals(attribute.Definition[j], StringComparison.InvariantCultureIgnoreCase)).Count();
                            ocurrenceModelList.Add(ocurrenceModel);
                        }
                        //total of the current attribute in range of the class
                        var totalOcurrenceModel = new DiscreteAttributeOcurrenceModel();
                        totalOcurrenceModel.CurrentAttributeName = currentAttributteName + " total";
                        totalOcurrenceModel.TargetAttributeClassName = classes.ClassName;
                        totalOcurrenceModel.Value = selectedData.Count();
                        ocurrenceModelList.Add(totalOcurrenceModel);
                        naiveBayesModel.AddOcurrenceMatrixRange(ocurrenceModelList);
                    }
                }
            }
            IFileLoader loader = new FileLoader.FileLoader();
            loader.SaveJsonFileToText(JsonConvert.SerializeObject(naiveBayesModel), string.Empty);
        }

        #endregion

        #region Private Methods
        #endregion
    }   
}
