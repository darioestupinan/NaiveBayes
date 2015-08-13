#region Namespaces
using ArffFileProcesser;
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

        private bool _mustReloadCache;

        private readonly List<string> _ignoreAttributes;

        #endregion

        #region Public Properties
        #endregion

        #region Constructors

        public NaiveBayes(ArffModel modelFromFile, IDistribution distribution, List<string> ignoredAttributes)
        {
            _model = modelFromFile;
            _distribution = distribution;
            _ignoreAttributes = ignoredAttributes;
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
    //#region Attributes
    //private TagDictionary<TFileProcessed, TAttributeType> _tags = new TagDictionary<TFileProcessed, TAttributeType>();
    //private TagDictionary<TFileProcessed, TAttributeType> _cache;

    //private readonly IFileProcesser<TFileProcessed> _tokenizer;
    //private readonly IDistribution _distribution;

    //private bool _mustRecache;
    //private const double Tolerance = 0.0001;
    //private const double Threshold = 0.1;
    //#endregion

    //#region Constructors
    //public NaiveBayes(IFileProcesser<TFileProcessed> tokenizer)
    //   : this(tokenizer, new StandardDeviation())
    //{
    //}

    //public NaiveBayes(IFileProcesser<TFileProcessed> tokenizer, IDistribution distribution)
    //{
    //    if (tokenizer == null) throw new ArgumentNullException("tokenizer");
    //    if (distribution == null) throw new ArgumentNullException("combiner");

    //    _tokenizer = tokenizer;
    //    _distribution = distribution;

    //    _tags.SystemTag = new TagData<TFileProcessed>();
    //    _mustRecache = true;
    //}
    //#endregion

    //#region Public Methods
    //public void AddTag(TAttributeType tagId)
    //{
    //    GetAndAddIfNotFound(_tags.Items, tagId);
    //    _mustRecache = true;
    //}

    //public void RemoveTag(TAttributeType tagId)
    //{
    //    _tags.Items.Remove(tagId);
    //    _mustRecache = true;
    //}

    //public void ChangeTagId(TAttributeType oldTagId, TAttributeType newTagId)
    //{
    //    _tags.Items[newTagId] = _tags.Items[oldTagId];
    //    RemoveTag(oldTagId);
    //    _mustRecache = true;
    //}

    //public void MergeTags(TAttributeType sourceTagId, TAttributeType destTagId)
    //{
    //    var sourceTag = _tags.Items[sourceTagId];
    //    var destTag = _tags.Items[destTagId];
    //    var count = 0;
    //    foreach (var tagItem in sourceTag.Items)
    //    {
    //        count++;
    //        var tok = tagItem;
    //        if (destTag.Items.ContainsKey(tok.Key))
    //        {
    //            destTag.Items[tok.Key] += count;
    //        }
    //        else
    //        {
    //            destTag.Items[tok.Key] = count;
    //            destTag.TokenCount += 1;
    //        }
    //    }
    //    RemoveTag(sourceTagId);
    //    _mustRecache = true;
    //}

    //public TagData<TFileProcessed> GetTagById(TAttributeType tagId)
    //{
    //    return _tags.Items.ContainsKey(tagId) ? _tags.Items[tagId] : null;
    //}

    //public IEnumerable<TAttributeType> TagIds()
    //{
    //    return _tags.Items.Keys.OrderBy(p => p);
    //}

    //public void Train(TAttributeType tagId, string input)
    //{
    //    var tokens = _tokenizer.Process(input);
    //    var tag = GetAndAddIfNotFound(_tags.Items, tagId);
    //    _train(tag, tokens);
    //    _tags.SystemTag.TrainCount += 1;
    //    tag.TrainCount += 1;
    //    _mustRecache = true;
    //}

    //public void Untrain(TAttributeType tagId, string input)
    //{
    //    var tokens = _tokenizer.Process(input);
    //    var tag = _tags.Items[tagId];
    //    if (tag == null)
    //    {
    //        return;
    //    }
    //    _untrain(tag, tokens);
    //    _tags.SystemTag.TrainCount += 1;
    //    tag.TrainCount += 1;
    //    _mustRecache = true;
    //}

    //public Dictionary<TAttributeType, double> Classify(string input)
    //{
    //    var tokens = _tokenizer.Process(input).ToList();
    //    var tags = CreateCacheAnsGetTags();

    //    var stats = new Dictionary<TAttributeType, double>();

    //    foreach (var tag in tags.Items)
    //    {
    //        var probs = GetProbabilities(tag.Value, tokens).ToList();
    //        if (probs.Count() != 0)
    //        {
    //            stats[tag.Key] = _distribution.ApplyDistribution(probs);
    //        }
    //    }
    //    return stats.OrderByDescending(s => s.Value).ToDictionary(s => s.Key, pair => pair.Value);
    //}

    //#region Input Output Methods
    //public void Save(string path)
    //{
    //    using (var streamWriter = new StreamWriter(path, false, Encoding.UTF8))
    //    {
    //        JsonSerializer.Create().Serialize(streamWriter, _tags);
    //    }
    //}

    //public void Load(string path)
    //{
    //    using (var textReader = new StreamReader(path, Encoding.UTF8))
    //    {
    //        //var result = textReader.ReadToEnd();
    //        using (var jsonTextReader = new JsonTextReader(textReader))
    //        {
    //            var text = jsonTextReader.ReadAsString();
    //            var jsonSerializer = JsonSerializer.Create();
    //            var deserialized = jsonSerializer.Deserialize<TagDictionary<TFileProcessed, TAttributeType>>(jsonTextReader);
    //            //_tags = JsonSerializer.Create().Deserialize<TagDictionary<TFileProcessed, TAttributeType>>(text);
    //        }
    //    }
    //    _mustRecache = true;
    //}

    //public void ImportJsonData(string json)
    //{
    //    _tags = JsonConvert.DeserializeObject<TagDictionary<TFileProcessed, TAttributeType>>(json);
    //    _mustRecache = true;
    //}

    //public string ExportJsonData()
    //{
    //    return JsonConvert.SerializeObject(_tags);
    //}
    //#endregion

    //#endregion

    //#region Private Methods

    //private void _train(TagData<TFileProcessed> tag, IEnumerable<TFileProcessed> tokens)
    //{
    //    var tokenCount = 0;
    //    foreach (var token in tokens)
    //    {
    //        var count = tag.Get(token, 0);
    //        tag.Items[token] = count + 1;
    //        count = _tags.SystemTag.Get(token, 0);
    //        _tags.SystemTag.Items[token] = count + 1;
    //        tokenCount += 1;
    //    }
    //    tag.TokenCount += tokenCount;
    //    _tags.SystemTag.TokenCount += tokenCount;
    //}

    //private void _untrain(TagData<TFileProcessed> tag, IEnumerable<TFileProcessed> tokens)
    //{
    //    foreach (var token in tokens)
    //    {
    //        var count = tag.Get(token, 0);
    //        if (count > 0)
    //        {
    //            if (Math.Abs(count - 1) < Tolerance)
    //            {
    //                tag.Items.Remove(token);
    //            }
    //            else
    //            {
    //                tag.Items[token] = count - 1;
    //            }
    //            tag.TokenCount -= 1;
    //        }
    //        count = _tags.SystemTag.Get(token, 0);
    //        if (count > 0)
    //        {
    //            if (Math.Abs(count - 1) < Tolerance)
    //            {
    //                _tags.SystemTag.Items.Remove(token);
    //            }
    //            else
    //            {
    //                _tags.SystemTag.Items[token] = count - 1;
    //            }
    //            _tags.SystemTag.TokenCount -= 1;
    //        }
    //    }
    //}

    //private static TagData<TFileProcessed> GetAndAddIfNotFound(IDictionary<TAttributeType, TagData<TFileProcessed>> dic, TAttributeType key)
    //{
    //    if (dic.ContainsKey(key))
    //    {
    //        return dic[key];
    //    }
    //    dic[key] = new TagData<TFileProcessed>();
    //    return dic[key];
    //}

    //private TagDictionary<TFileProcessed, TAttributeType> CreateCacheAnsGetTags()
    //{
    //    if (!_mustRecache) return _cache;

    //    _cache = new TagDictionary<TFileProcessed, TAttributeType> { SystemTag = _tags.SystemTag };
    //    foreach (var tag in _tags.Items)
    //    {
    //        var thisTagTokenCount = tag.Value.TokenCount;
    //        var otherTagsTokenCount = Math.Max(_tags.SystemTag.TokenCount - thisTagTokenCount, 1);
    //        var cachedTag = GetAndAddIfNotFound(_cache.Items, tag.Key);

    //        foreach (var systemTagItem in _tags.SystemTag.Items)
    //        {
    //            var thisTagTokenFreq = tag.Value.Get(systemTagItem.Key, 0.0);
    //            if (Math.Abs(thisTagTokenFreq) < Tolerance)
    //            {
    //                continue;
    //            }
    //            var otherTagsTokenFreq = systemTagItem.Value - thisTagTokenFreq;

    //            var goodMetric = thisTagTokenCount == 0 ? 1.0 : Math.Min(1.0, otherTagsTokenFreq / thisTagTokenCount);
    //            var badMetric = Math.Min(1.0, thisTagTokenFreq / otherTagsTokenCount);
    //            var f = badMetric / (goodMetric + badMetric);

    //            if (Math.Abs(f - 0.5) >= Threshold)
    //            {
    //                cachedTag.Items[systemTagItem.Key] = Math.Max(Tolerance, Math.Min(1 - Tolerance, f));
    //            }
    //        }
    //    }
    //    _mustRecache = false;
    //    return _cache;
    //}

    //private static IEnumerable<double> GetProbabilities(TagData<TFileProcessed> tag, IEnumerable<TFileProcessed> tokens)
    //{
    //    var probs = tokens.Where(tag.Items.ContainsKey).Select(t => tag.Items[t]);
    //    return probs.OrderByDescending(p => p).Take(2048);
    //}

    //#endregion

    //}
}
