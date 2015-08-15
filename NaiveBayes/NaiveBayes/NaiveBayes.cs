#region Namespaces
using ArffFileProcesser;
using FileLoader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
#endregion

namespace NaiveBayes
{
    public class NaiveBayes 
    {
        #region Attributes

        private ArffModel _model = new ArffModel();
        
        private readonly OcurrenceModelFactory _ocurrenceModelFactory = new OcurrenceModelFactory();

        private readonly List<string> _ignoreAttributes;

        private string _targetAttribute;

        private NaiveBayesModel _naiveBayesModel;

        #endregion

        #region Public Properties
        #endregion

        #region Constructors

        public NaiveBayes(ArffModel modelFromFile, List<string> ignoredAttributes, string targetAttribute)
        {
            _model = modelFromFile;
            _ignoreAttributes = ignoredAttributes;
            _targetAttribute = targetAttribute;
        }

        #endregion

        #region Public Methods

        

        public void TrainFromSet()
        {
            var naiveBayesModel = new NaiveBayesModel();
            naiveBayesModel.DataModel = _model;
            naiveBayesModel.TargetAttribute = _targetAttribute;
            //first identify the possible values of the target Attribute 
            var targetAttribute = naiveBayesModel.DataModel.Attributes.Where(attrib => attrib.Name.Equals(_targetAttribute, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var targetAttributeClassesCount = targetAttribute.Definition.Count();
            var totalData = targetAttribute.Values.Count();
            // calculate precision  and 3-std
            for (var i = 0; i < naiveBayesModel.DataModel.Attributes.Count(); i++)
            {
                var currentAttribute = naiveBayesModel.DataModel.Attributes[i];
                if (currentAttribute.Definition.Count() == 1 && currentAttribute.Definition[0].Equals("Real", StringComparison.InvariantCultureIgnoreCase))
                {
                    var listValues = currentAttribute.Values.Cast<double>().ToList();
                    var delta = CalculateDelta(listValues);
                    var distinct = CalculateDistinct(listValues);
                    var precision = (double)delta / (double)distinct;
                    naiveBayesModel.DataModel.Attributes[i].Precision = precision; 
                    for (var row = 0; row < naiveBayesModel.DataModel.Data.Count; row++)
                    {
                        var newValue = Math.Round((double)naiveBayesModel.DataModel.Data[row][i] / precision, 0) * precision;
                        naiveBayesModel.DataModel.Data[row][i] = newValue;
                    }                                        
                }
            }
            //calculate probability of the classes identified
            foreach (var definitionClass in targetAttribute.Definition)
            {
                var current = new AttributeValue();
                current.ClassName = definitionClass;
                current.Probability = (double)targetAttribute.Values.Where(val => val.ToString().Equals(definitionClass, StringComparison.InvariantCultureIgnoreCase)).Count() / (double)totalData;
                naiveBayesModel.TargetAttributeClasses.Add(current);
            }
            // change foreach continuous values the value according to the formula  value = ParteEntera (original value / precision ) * precision 


            //calculate possibilities foreach class within each value of other attribute
            //find the target attribute index
            var targetAttribIndex = naiveBayesModel.DataModel.Attributes.ToList().FindIndex(attr => attr.Name.Equals(naiveBayesModel.TargetAttribute, StringComparison.InvariantCultureIgnoreCase));
            for (var i=0; i< naiveBayesModel.DataModel.Attributes.Count(); i++)
            {
                var attribute = naiveBayesModel.DataModel.Attributes[i];
                var currentAttributteName = attribute.Name;
                if (attribute.Name.Equals(_targetAttribute, StringComparison.InvariantCultureIgnoreCase)) continue;
                if (attribute.Definition.Count() == 1 && attribute.Definition[0].Equals("Real", StringComparison.InvariantCultureIgnoreCase))
                {
                    //continuous
                    foreach (var classes in naiveBayesModel.TargetAttributeClasses)
                    {
                        var ocurrenceModelList = new List<IOcurrenceModel>();
                        var selectedData = naiveBayesModel.DataModel.Data.Cast<List<object>>().Where(item => item[targetAttribIndex].ToString().Trim().Equals(classes.ClassName, StringComparison.InvariantCultureIgnoreCase)).ToList();
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
                            ocurrenceModel.Precision = naiveBayesModel.DataModel.Attributes[i].Precision;
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
                        // total ocurrences by class
                        var selectedData = naiveBayesModel.DataModel.Data.Cast<List<object>>().Where(item => item[targetAttribIndex].ToString().Trim().Equals(classes.ClassName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                        for (var j = 0; j < attribute.Definition.Count; j++)
                        {
                            var ocurrenceModel = new DiscreteAttributeOcurrenceModel();
                            ocurrenceModel.CurrentAttributeName = currentAttributteName;
                            ocurrenceModel.CurrentAttributeValue = attribute.Definition[j];
                            ocurrenceModel.TargetAttributeClassName =  classes.ClassName;
                            // ocurrences of the current definition by class
                            var value = ((double)selectedData.Cast<List<object>>().Where(item => item[i].ToString().Equals(attribute.Definition[j], StringComparison.InvariantCultureIgnoreCase)).Count() + 1.0) / ((double)selectedData.Count + (double)attribute.Definition.Count);
                            ocurrenceModel.Value = value;
                            ocurrenceModelList.Add(ocurrenceModel);
                        }
                        //total of the current attribute in range of the class
                        var totalOcurrenceModel = new DiscreteAttributeOcurrenceModel();
                        totalOcurrenceModel.CurrentAttributeName = currentAttributteName + " total";
                        totalOcurrenceModel.TargetAttributeClassName = classes.ClassName;
                        totalOcurrenceModel.Value = ocurrenceModelList.Cast<DiscreteAttributeOcurrenceModel>().Sum(m=>m.Value);
                        ocurrenceModelList.Add(totalOcurrenceModel);
                        naiveBayesModel.AddOcurrenceMatrixRange(ocurrenceModelList);
                    }
                }
            }
            IFileLoader loader = new FileLoader.FileLoader();
            loader.SaveJsonFileToText(JsonConvert.SerializeObject(naiveBayesModel), string.Empty);
            _naiveBayesModel = naiveBayesModel;
        }

        public ResultModel TestNewData (IList<TestData> data)
        {
            var result = new ResultModel();
            var targetAttribute = _naiveBayesModel.DataModel.Attributes.Where(attrib => attrib.Name.Equals(_targetAttribute, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var targetAttribIndex = _naiveBayesModel.DataModel.Attributes.ToList().FindIndex(attr => attr.Name.Equals(_naiveBayesModel.TargetAttribute, StringComparison.InvariantCultureIgnoreCase));
            var probabilityResultList = new List<ProbabilityResult>();
            for (var i = 0; i < _naiveBayesModel.TargetAttributeClasses.Count; i++)
            {
                var probabilityList = new List<double>();
                var classProbability =_naiveBayesModel.TargetAttributeClasses[i].Probability;
                probabilityList.Add(classProbability);
                for (var j=0; j < data.Count; j++)
                {
                    var current = data[j];
                    var pairAttribute = _naiveBayesModel.DataModel.Attributes.Where(item => item.Name.Equals(current.DataName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (pairAttribute != null)
                    {
                        if (pairAttribute.Definition.Count == 1 && pairAttribute.Definition[0].Equals("Real", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var normalizedValue = Math.Round(Double.Parse(current.Value.ToString()) / pairAttribute.Precision, 0) * pairAttribute.Precision; 
                            //continuous
                            var searchedClasses = _naiveBayesModel.OcurrenceMatrix.Where(m => m.GetAttributeName().Equals(current.DataName, StringComparison.InvariantCultureIgnoreCase) && m.GetTargetClass().Equals(_naiveBayesModel.TargetAttributeClasses[i].ClassName, StringComparison.InvariantCultureIgnoreCase)).First();
                            // formula!!!
                            var values = searchedClasses.GetValues();

                            var probabilityValue = normalDistribution(normalizedValue, values[0], values[1], false);
                            probabilityList.Add(probabilityValue);
                        }
                        else
                        {
                            //discrete
                            var searchedClasses = _naiveBayesModel.OcurrenceMatrix.Where(m => m.GetAttributeName().Equals(current.DataName, StringComparison.InvariantCultureIgnoreCase) && m.GetAttributeValue().Equals(current.Value.ToString(), StringComparison.InvariantCultureIgnoreCase) && m.GetTargetClass().Equals(_naiveBayesModel.TargetAttributeClasses[i].ClassName, StringComparison.InvariantCultureIgnoreCase)).First();
                            var value = searchedClasses.GetValues().First();
                            // formula time!!! 
                            probabilityList.Add(value);                                
                        }
                    }
                }
                var probabilityResult = new ProbabilityResult();
                probabilityResult.ClassName = _naiveBayesModel.TargetAttributeClasses[i].ClassName;
                probabilityResult.Probability = probabilityList.Aggregate((a, x) => a * x);
                probabilityResultList.Add(probabilityResult);
            }
            var sumProbabilities = probabilityResultList.Select(x => x.Probability).Sum();
            foreach (var probability in probabilityResultList)
            {
                probability.Percentage = (probability.Probability / sumProbabilities) * 100;
            }
            var greaterProbability = probabilityResultList.OrderByDescending(x => x.Percentage).First();
            result.ResultAttribute = greaterProbability.ClassName;
            result.Values = greaterProbability.Percentage;
            return result;
        }
        
        #endregion

        #region Private Methods
        
        private double CalculateDelta (IList<double> values)
        {
            double result = 0.0;
            values = values.OrderBy(m=> m).ToList();
            for(var i =1; i<values.Count; i++)
                result += values[i] - values[i - 1];
            return result;
        }

        private double CalculateDistinct (IList<double> values)
        {
            double result = 0.0;
            values = values.OrderBy(m => m).ToList();
            result = values.Distinct().Count() - 1 ;
            return result;
        }


        // from http://www.cs.princeton.edu/introcs/...Math.java.html
        // fractional error less than 1.2 * 10 ^ -7.
        private static double normalDistribution(double x, double mean, double std, bool cumulative)
        {
            if (cumulative)
            {
                return Phi(x, mean, std);
            }

            var tmp = 1 / (Math.Sqrt(2 * Math.PI) * std);
            return tmp * Math.Exp(-.5 * Math.Pow((x - mean) / std, 2));
        }

        

        // cumulative normal distribution
        /// <summary>The phi.</summary>
        /// <param name="z">The z.</param>
        /// <returns>The phi.</returns>
        private static double Phi(double z)
        {
            return 0.5 * (1.0 + erf(z / Math.Sqrt(2.0)));
        }

        // cumulative normal distribution with mean mu and std deviation sigma
        /// <summary>The phi.</summary>
        /// <param name="z">The z.</param>
        /// <param name="mu">The mu.</param>
        /// <param name="sigma">The sigma.</param>
        /// <returns>The phi.</returns>
        private static double Phi(double z, double mu, double sigma)
        {
            return Phi((z - mu) / sigma);
        }

        /// <summary>The erf.</summary>
        /// <param name="z">The z.</param>
        /// <returns>The erf.</returns>
        private static double erf(double z)
        {
            var t = 1.0 / (1.0 + 0.5 * Math.Abs(z));

            // use Horner's method
            var ans = 1 -
                      t *
                      Math.Exp(
                          -z * z - 1.26551223 +
                          t *
                          (1.00002368 +
                           t *
                           (0.37409196 +
                            t *
                            (0.09678418 +
                             t *
                             (-0.18628806 +
                              t *
                              (0.27886807 +
                               t * (-1.13520398 + t * (1.48851587 + t * (-0.82215223 + t * 0.17087277)))))))));
            if (z >= 0)
            {
                return ans;
            }

            return -ans;
        }

        #endregion
    }   
}
