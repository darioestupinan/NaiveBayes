﻿using System.Collections.Generic;

namespace ArffFileProcesser
{
    public class ArffModel
    {
        public IList<Attribute> _attributes = new List<Attribute>();
        private IList<IList<object>> _data = new List<IList<object>>();

        public string Relation { get; set; }
        public IList<Attribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public IList<IList<object>> Data
        {
            get { return _data; }            
        }

        public void AddAttribute(Attribute attribute)
        {
            _attributes.Add(attribute);
        }

        public void AddDataRow (IList<object> row)
        {
            _data.Add(row);
        }
    }

    public class Attribute
    {
        private IList<string> _definition = new List<string>();
        private IList<string> _values = new List<string>();
        
        public string Name { get; set; }
        public IList<string> Definition
        {
            get { return _definition; }
            set { _definition = value; }
        }
        public IList<string> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public void AddDefinition(string definition)
        {
            _definition.Add(definition);
        }

        public void AddValues(string value)
        {
            _values.Add(value);
        }
    }
}