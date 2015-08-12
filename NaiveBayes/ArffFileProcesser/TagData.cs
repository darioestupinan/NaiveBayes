using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArffFileProcesser
{
    public class TagDictionary<TFileProcessed, TAttributeType>
    {
        public TagDictionary()
        {
            Items = new Dictionary<TAttributeType, TagData<TFileProcessed>>();
        }
        public Dictionary<TAttributeType, TagData<TFileProcessed>> Items { get; private set; }
        public TagData<TFileProcessed> SystemTag { get; set; }
    }

    public class TagData<TFileProcessed>
    {
        public int TrainCount { get; set; }
        public int TokenCount { get; set; }
        public Dictionary<TFileProcessed, double> Items { get; private set; }

        public TagData()
        {
            Items = new Dictionary<TFileProcessed, double>();
            TrainCount = 0;
        }

        public double Get(TFileProcessed token, double defaultValue)
        {
            if (Items.ContainsKey(token))
            {
                return Items[token];
            }
            return defaultValue;
        }
    }
}
