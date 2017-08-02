/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : ProcessorBuilder
 * Description : Provides methods for creating a Processor.
 * Created     : 2016/1/25
 * Note        :
*********************************************************************************/

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
    /// Provides methods for creating a <see cref="Processor"/>.
    /// </summary>
    public class ProcessorBuilder
    {
        /// <summary>
        /// Initial a new instance of the <see cref="ProcessorBuilder"/> class.
        /// </summary>
        public ProcessorBuilder() { }

        /// <summary>
        /// Creates a new instance of the <see cref="Processor"/> class.
        /// </summary>
        /// <returns>The created <see cref="Processor"/>.</returns>
        public Processor GetProcessor()
        {
            return new Processor(
                new Dictionary<string, Processor.Parser>(_parsers),
                maxRecordCount: MaxRecordCount,
                includeUri: IncludeUri);
        }

        /// <summary>
        ///   Gets or sets the maximum times of repetition. The repetition has no limit 
        ///   if the value is set negative.
        /// </summary>
        /// <value>The maximum times of repetition. </value>
        public int MaxRecordCount { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether the record includes the URI of the page.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the record includes the URI of the page;
        ///   otherwise, <see langword="false"/>.
        /// </value>
        public bool IncludeUri { get; set; } = true;

        /// <summary>
        /// Sets a parser with the specified identifier.
        /// </summary>
        /// <param name="key">The identifier.</param>
        /// <param name="parser">The parser.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="key"/> is invalid.
        /// </exception>
        public void SetParser(string key, IParser<string, string> parser)
        {
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));
            if (!Util.IsValidIdentifier(key))
                throw new ArgumentOutOfRangeException(nameof(key));
            Processor.Parser p;
            if (_parsers.TryGetValue(key, out p))
            {
                if (p.ParserType == 1)
                {
                    p.HtmlParser = null;
                    p.ParserType = 0;
                }
                else if (p.ParserType == 2)
                {
                    p.XmlParser = null;
                    p.ParserType = 0;
                }
                p.TextParser = parser;
            }
            else
            {
                p = new Processor.Parser()
                {
                    ParserType = 0,
                    TextParser = parser
                };
                _parsers.Add(key, p);
            }
        }

        /// <inheritdoc cref="SetParser(string, IParser{string, string})"/>
        public void SetParser(string key, IParser<HtmlDocument, string> parser)
        {
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));
            if (!Util.IsValidIdentifier(key))
                throw new ArgumentOutOfRangeException(nameof(key));
            Processor.Parser p;
            if (_parsers.TryGetValue(key, out p))
            {
                if (p.ParserType == 0)
                {
                    p.TextParser = null;
                    p.ParserType = 1;
                }
                else if (p.ParserType == 2)
                {
                    p.XmlParser = null;
                    p.ParserType = 1;
                }
                p.HtmlParser = parser;
            }
            else
            {
                p = new Processor.Parser()
                {
                    ParserType = 1,
                    HtmlParser = parser
                };
                _parsers.Add(key, p);
            }
        }

        /// <inheritdoc cref="SetParser(string, IParser{string, string})"/>
        public void SetParser(string key, IParser<XDocument, string> parser)
        {
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));
            if (!Util.IsValidIdentifier(key))
                throw new ArgumentOutOfRangeException(nameof(key));
            Processor.Parser p;
            if (_parsers.TryGetValue(key, out p))
            {
                if (p.ParserType == 0)
                {
                    p.TextParser = null;
                    p.ParserType = 2;
                }
                else if (p.ParserType == 1)
                {
                    p.HtmlParser = null;
                    p.ParserType = 2;
                }
                p.XmlParser = parser;
            }
            else
            {
                p = new Processor.Parser()
                {
                    ParserType = 2,
                    XmlParser = parser
                };
                _parsers.Add(key, p);
            }
        }

        /// <summary>
        /// Removes the parser with the specified identifier.
        /// </summary>
        /// <param name="key">The identifier.</param>
        /// <returns>
        ///   <see langword="true"/> if the parser with the identifier <paramref name="key"/> 
        ///   is found and removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="key"/> is invalid.
        /// </exception>
        public bool RemoveParser(string key)
        {
            if (!Util.IsValidIdentifier(key))
                throw new ArgumentOutOfRangeException(nameof(key));
            return _parsers.Remove(key);
        }

        /// <summary>
        /// Removes all the parsers.
        /// </summary>
        public void ClearAllParsers()
        {
            _parsers.Clear();
        }

        /// <summary>
        /// Gets the parser with the specified identifier.
        /// </summary>
        /// <param name="key">The identifier.</param>
        /// <value>
        ///   The parser with the identifier <paramref name="key"/> if it is found;
        ///   otherwise, <see langword="null"/>.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="key"/> is invalid.
        /// </exception>
        public object this[string key]
        {
            get
            {
                if (!Util.IsValidIdentifier(key))
                    throw new ArgumentOutOfRangeException(nameof(key));
                Processor.Parser parser;
                if (_parsers.TryGetValue(key, out parser))
                    if (parser.ParserType == 0)
                        return parser.TextParser;
                    else if (parser.ParserType == 1)
                        return parser.HtmlParser;
                    else if (parser.ParserType == 2)
                        return parser.XmlParser;
                return null;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the parsers.
        /// </summary>
        /// <returns>An enumerator for the parsers.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetParsers()
        {
            return _parsers.Select(p =>
            {
                object value;
                Processor.Parser parser = p.Value;
                if (parser.ParserType == 0)
                    value = parser.TextParser;
                else if (parser.ParserType == 1)
                    value = parser.HtmlParser;
                else if (parser.ParserType == 2)
                    value = parser.XmlParser;
                else
                    throw new ArgumentOutOfRangeException(nameof(Processor.Parser.ParserType));
                return new KeyValuePair<string, object>(p.Key, value);
            });
        }

        private Dictionary<string, Processor.Parser> _parsers = new Dictionary<string, Processor.Parser>();
    }
}
