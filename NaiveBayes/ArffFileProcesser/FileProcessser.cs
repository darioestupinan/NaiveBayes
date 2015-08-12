using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArffFileProcesser
{
    public interface IFileProcesser<out TFileProcessed>
    {
        IEnumerable<TFileProcessed> Process(object input);
    }

    public class SimpleFileProcceser : IFileProcesser<string>
    {
        private readonly Regex _wordRe = new Regex(@"\w+");
        private readonly bool _convertToLower;
        private readonly List<string> _ignoreList;

        public SimpleFileProcceser() : this(true, null)
        {
        }
        
        public SimpleFileProcceser(bool convertToLower, List<string> ignoreList)
        {
            _ignoreList = ignoreList;
            _convertToLower = convertToLower;
        }
        
        public IEnumerable<string> Process(object input)
        {
            if (input.GetType() != typeof(string))
            {
                throw new FormatException(string.Format("Expected string, given {0}", input.GetType()));
            }
            var tokens = MatchParameters(input);
            if (_ignoreList == null)
            {
                return tokens;
            }
            return tokens.Where(token => !_ignoreList.Contains(token));
        }

        private IEnumerable<string> MatchParameters(object input)
        {
            foreach (Match match in _wordRe.Matches((string)input))
            {
                if (_convertToLower)
                {
                    yield return match.Value.ToLower();
                }
                else
                {
                    yield return match.Value;
                }
            }
        }
    }
}
