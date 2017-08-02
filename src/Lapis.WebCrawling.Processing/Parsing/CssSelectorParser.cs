/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : CssSelectorParser
 * Description : Implements an IParser that wraps a CssSelector.
 * Created     : 2016/1/25
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing;
using Lapis.WebCrawling.HtmlParsing.CssSelectors;
using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Parsing
{
    /// <summary>
    ///   Implements an <see cref="IParser{HtmlDocument, String}"/> that wraps a 
    ///   <see cref="CssSelector"/> object.
    /// </summary>
    public class CssSelectorParser : IParser<HtmlDocument, string>
    {
        /// <summary>
        ///   Inintialize a new instance of the <see cref="CssSelectorParser"/> class 
        ///   with the specified parameters.
        /// </summary>
        /// <param name="cssSelector">
        ///   The <see cref="CssSelector"/> wrapped by the <see cref="CssSelectorParser"/>.
        /// </param>
        /// <param name="resultType">
        ///   A value indicating whether the parser returns the inner text or outer HTML 
        ///   of the selected node. The value of the parameter must not be 
        ///   <see cref="NodeParserResultType.Attribute"/> in this constructor.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="cssSelector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="resultType"/> is <see cref="NodeParserResultType.Attribute"/>.
        /// </exception>
        public CssSelectorParser(CssSelector cssSelector, NodeParserResultType resultType)
        {
            if (cssSelector == null)
                throw new ArgumentNullException(nameof(cssSelector));
            if (resultType != NodeParserResultType.Node && resultType != NodeParserResultType.Value)
                throw new ArgumentOutOfRangeException(nameof(resultType));
            CssSelector = cssSelector;
            ResultType = resultType;
        }

        /// <summary>
        ///   Inintialize a new instance of the <see cref="CssSelectorParser"/> class 
        ///   with the specified parameters.
        /// </summary>
        /// <param name="cssSelector">
        ///   The <see cref="CssSelector"/> wrapped by the <see cref="CssSelectorParser"/>.
        /// </param>
        /// <param name="attribute">
        ///   The name of the attribute of the selected node whose value is returned by the 
        ///   parser. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="cssSelector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="attribute"/> is invalid.
        /// </exception>
        public CssSelectorParser(CssSelector cssSelector, string attribute)
        {
            if (cssSelector == null)
                throw new ArgumentNullException(nameof(cssSelector));
            if (string.IsNullOrWhiteSpace(attribute))
                throw new ArgumentException(nameof(attribute));         
            CssSelector = cssSelector;
            ResultType = NodeParserResultType.Attribute;
            Attribute = attribute;
        }

        /// <summary>
        ///   Gets the <see cref="CssSelector"/> object wrapped by the 
        ///   <see cref="CssSelectorParser"/> object.
        /// </summary>
        /// <value>
        ///   The <see cref="CssSelector"/> wrapped by the <see cref="CssSelectorParser"/>.
        /// </value>
        public CssSelector CssSelector { get; }

        /// <summary>
        ///   Gets a value indicating whether the parser returns the inner text, outer HTML, 
        ///   or the value of an attribute of the selected node. 
        /// </summary>
        /// <value>
        ///   A <see cref="NodeParserResultType"/> value.
        /// </value>
        public NodeParserResultType ResultType { get; }

        /// <summary>
        ///   Gets the name of the attribute of the selected node whose value is returned by the parser 
        ///   when <see cref="ResultType"/> is set <see cref="NodeParserResultType.Attribute"/>. 
        /// </summary>
        /// <value>The name of the attribute.</value>
        public string Attribute { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Parse(HtmlDocument input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            return CssSelector.Select(input.AsEnumerable())
                .Select(node =>
                {
                    if (ResultType == NodeParserResultType.Node)
                        return node.ToString(HtmlFormat.Compressed);
                    else if (ResultType == NodeParserResultType.Value)
                        return (node as HtmlElement)?.Children?.ToString(HtmlFormat.Compressed);
                    else if (ResultType == NodeParserResultType.Attribute)
                        return node.Attribute(Attribute)?.Value;
                    else
                        return node.ToString(HtmlFormat.Compressed);
                });
        }
    }

    /// <summary>
    ///   Represents a value indicating whether the parser returns the inner text, outer HTML, 
    ///   or the value of an attribute of the selected node. 
    /// </summary>
    public enum NodeParserResultType
    {
        /// <summary>
        /// The value indicates that the parser returns the outer HTML.
        /// </summary>
        Node,
        /// <summary>
        /// The value indicates that the parser returns the inner text.
        /// </summary>
        Value,
        /// <summary>
        /// The value indicates that the parser returns the value of an attribute.
        /// </summary>
        Attribute
    }
}
