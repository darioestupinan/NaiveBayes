using System.Collections.Generic;

namespace NaiveBayes
{
    public class ResultModel
    {
        public string resultAttribute { get; private set; }
        public IList<object> Values { get; set; }
    }
}