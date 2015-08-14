#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace ArffFileProcesser
{
    public interface IFileProcesser<out TFileProcessed>
    {
        ArffModel Process(object input);
    }

    public class SimpleFileProcceser : IFileProcesser<string>
    {
        #region Attributes

        private readonly Regex _wordRe = new Regex(@"\w+");
        private readonly Regex _lineSplit = new Regex(@"\r\n |\r |\n");
        private readonly ArffModel _model;

        #endregion

        #region Constructors

        public SimpleFileProcceser()
        {
            _model = new ArffModel();
        }
        #endregion

        #region Public Methods
        public ArffModel Process(object input)
        {
            if (input.GetType() != typeof(string))
            {
                throw new FormatException(string.Format("Expected string, given {0}", input.GetType()));
            }
            var lines = _lineSplit.Split((string)input).Where(line => !string.IsNullOrEmpty(line)).ToArray();

            GetAllData(lines);

            return _model;
        }

        #endregion

        #region Private Methods
        
        private void GetAllData(string[] lines)
        {
            for (var i = 0; i < lines.Count(); i++)
            {
                var currentLine = lines[i].Replace('\r', ' ').Trim();
                if (string.IsNullOrEmpty(currentLine)) continue;
                if (currentLine.StartsWith("@"))
                    GetFileDescriptors(currentLine);
                else
                    GetFileData(currentLine);                
            }
        }

        private void GetFileDescriptors(string currentLine)
        {
            var words = _wordRe.Matches(currentLine).Cast<Match>().Select(m => m.Value).ToList();
            if (words.Count > 1)
            {
                var descriptor = words[0];
                words.RemoveAt(0);
                var definitionList = words;
                ProcessDescriptors(descriptor, definitionList);
            }
        }

        private void GetFileData (string currentLine)
        {
            var words = currentLine.Split(new[] { ',' }).ToList();
            var row = new List<object>();
            if (words.Count == _model.Attributes.Count)
            {
                for (var j = 0; j < words.Count; j++)
                {
                    _model.Attributes[j].AddValues(words[j]);
                    if (_model.Attributes[j].Definition.Count == 1 && _model.Attributes[j].Definition[0].Equals("Real", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var numericValue = Double.Parse(words[j]);
                        row.Add(numericValue);
                    }
                    else row.Add(words[j]);
                }
                _model.AddDataRow(row);
            }
            
        }

        private void ProcessDescriptors(string descriptor, List<string> descriptorDefinition)
        {
            switch (descriptor)
            {
                case "relation":
                    ProcessRelation(descriptorDefinition);
                    break;

                case "attribute":
                    ProcessAttributes(descriptorDefinition);
                    break;

                default:
                    break;

            }
        }

        private void ProcessRelation(List<string> relationDefinition)
        {
            var relationName = string.Join(" ", relationDefinition);
            _model.Relation = relationName;
        }

        private void ProcessAttributes (List<string> attributeDefinition)
        {
            var currentAttribute = new Attribute();
            for (var j = 0; j < attributeDefinition.Count; j++)
            {
                if (j == 0) currentAttribute.Name = attributeDefinition[j];
                else
                    currentAttribute.AddDefinition(attributeDefinition[j]);
            }
            _model.AddAttribute(currentAttribute);
        }
    }

    #endregion
}

