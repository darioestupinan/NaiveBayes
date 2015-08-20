using System.Collections.Generic;

namespace ArffFileProcesser
{
    public class ArffModel
    {
        private IList<Attribute> _attributes = new List<Attribute>();
        private IList<IList<object>> _data = new List<IList<object>>();

        public string Relation { get; set; }
        public IList<Attribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public IList<IList<object>> Data => _data;

        public void AddAttribute(Attribute attribute)
        {
            _attributes.Add(attribute);
        }

        public void AddDataRow (IList<object> row)
        {
            _data.Add(row);
        }
    }
}
