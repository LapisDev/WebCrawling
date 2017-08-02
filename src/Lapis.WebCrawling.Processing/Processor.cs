/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : Processor
 * Description : Provides methods for record extraction from HTML.
 * Created     : 2016/1/25
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing;
using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using Lapis.WebCrawling.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lapis.WebCrawling.Processing
{
    /// <summary>
    /// Provides methods for record extraction from HTML.
    /// </summary>
    public class Processor
    {
        /// <summary>
        ///   Gets the maximum times of repetition. The repetition has no limit 
        ///   if the value is set negative.
        /// </summary>
        /// <value>The maximum times of repetition. </value>
        public int MaxRecordCount { get; } = -1;

        /// <summary>
        /// Gets a value indicating whether the record includes the URI of the page.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the record includes the URI of the page;
        ///   otherwise, <see langword="false"/>.
        /// </value>
        public bool IncludeUri { get; } = true;

        /// <summary>
        /// Parses the HTML and returns the extracted records.
        /// </summary>
        /// <param name="input">The HTML to parse.</param>
        /// <param name="uri">The URI of the web page.</param>
        /// <returns>The extracted results.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerable<Record> Process(Uri uri, string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            var ts = Parse(input).Select(t => Tuple.Create(t.Item1, t.Item2.GetEnumerator())).ToArray();
            for (int i = 0; MaxRecordCount < 0 || i < MaxRecordCount; i++)
            {
                var record = new Dictionary<string, string>();             
                bool hasValue = false;
                foreach (var t in ts)
                {
                    if (t.Item2.MoveNext())
                    {
                        record.Add(t.Item1, t.Item2.Current);
                        hasValue = true;
                    }
                    else
                        record.Add(t.Item1, null);
                }
                if (hasValue)
                {
                    if (IncludeUri)
                    {
                        if (!record.ContainsKey(_uriKey))
                            record.Add(_uriKey, uri?.ToString());
                        else
                        {
                            for (int j = 1; j < 1000; j++)
                            {
                                _uriKey = $"{_uriKey}_{j}";
                                if (!record.ContainsKey(_uriKey))
                                {
                                    record.Add(_uriKey, uri?.ToString());
                                    break;
                                }
                            }
                        }
                    }
                    yield return new Record(record);
                }
                else
                    yield break;
            }
        }

        private IEnumerable<Tuple<string, IEnumerable<string>>> Parse(string input)
        {
            string text = input;
            HtmlDocument html = null;
            XDocument xml = null;
            foreach (var pair in _parsers)
            {
                IEnumerable<string> result;
                if (pair.Value.ParserType == 0)
                {
                    var parser = pair.Value.TextParser;
                    result = parser.Parse(text);
                }
                else if (pair.Value.ParserType == 1)
                {
                    var parser = pair.Value.HtmlParser;
                    result = parser.Parse(html ?? (html = Html.Parse(input)));
                }
                else if (pair.Value.ParserType == 2)
                {
                    var parser = pair.Value.XmlParser;
                    result = parser.Parse(xml ?? (xml = (html ?? (html = Html.Parse(input))).ToXml()));
                }
                else
                {
                    result = Enumerable.Empty<string>();
                }
                yield return Tuple.Create(pair.Key, result);
            }
        }

        internal Processor(Dictionary<string, Parser> parsers, int maxRecordCount = -1, bool includeUri = true)
        {
            MaxRecordCount = maxRecordCount;
            _parsers = parsers;
            IncludeUri = includeUri;
        }

        private Dictionary<string, Parser> _parsers;

        internal class Parser
        {
            public int ParserType;
            public IParser<string, string> TextParser;
            public IParser<HtmlDocument, string> HtmlParser;
            public IParser<XDocument, string> XmlParser;
        }

        private string _uriKey = "Uri";
    }
}
