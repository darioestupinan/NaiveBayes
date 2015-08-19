using System.Collections.Generic;

namespace ArffFileProcesser
{
    public class Attribute
    {
        private IList<string> _definition = new List<string>();
        private IList<object> _values = new List<object>();

        public double Precision { get; set; }
        public string Name { get; set; }

        public IList<string> Definition
        {
            get { return _definition; }
            set { _definition = value; }
        }
        public IList<object> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public void AddDefinition(string definition)
        {
            _definition.Add(definition);
        }

        public void AddValues(object value)
        {
            _values.Add(value);
        }
    }
}