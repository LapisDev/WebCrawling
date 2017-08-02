/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : XPathParser
 * Description : Implements an IParser that uses XPath.
 * Created     : 2016/1/25
 * Note        :
*********************************************************************************/

//#if SYSTEM_XML_XPATH

using Lapis.WebCrawling.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lapis.WebCrawling.Parsing
{
    /// <summary>
    /// Implements an <see cref="IParser{XDocument, String}"/> that uses XPath.
    /// </summary>
    public class XPathParser : IParser<XDocument, string>
    {
        /// <summary>
        ///   Inintialize a new instance of the <see cref="XPathParser"/> class 
        ///   with the specified parameters.
        /// </summary>
        /// <param name="xpath">The XPath of the node to select.</param>
        /// <param name="resultType">
        ///   A value indicating whether the parser returns the inner text or outer HTML 
        ///   of the selected node. The value of the parameter must not be 
        ///   <see cref="NodeParserResultType.Attribute"/> in this constructor.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="xpath"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="resultType"/> is <see cref="NodeParserResultType.Attribute"/>.
        /// </exception>
        public XPathParser(string xpath, NodeParserResultType resultType)
        {
            if (xpath == null)
                throw new ArgumentNullException(nameof(xpath));
            if (resultType != NodeParserResultType.Node && resultType != NodeParserResultType.Value)
                throw new ArgumentOutOfRangeException(nameof(resultType));
            XPath = xpath;
            ResultType = resultType;
        }

        /// <summary>
        ///   Inintialize a new instance of the <see cref="XPathParser"/> class 
        ///   with the specified parameters.
        /// </summary>
        /// <param name="xpath">The XPath of the node to select.</param>
        /// <param name="attribute">
        ///   The name of the attribute of the selected node whose value is returned by the 
        ///   parser. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="xpath"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="attribute"/> is invalid.
        /// </exception>
        public XPathParser(string xpath, string attribute)
        {
            if (xpath == null)
                throw new ArgumentNullException(nameof(xpath));
            if (string.IsNullOrWhiteSpace(attribute))
                throw new ArgumentException(nameof(attribute));         
            XPath = xpath;
            ResultType = NodeParserResultType.Attribute;
            Attribute = attribute;
        }

        /// <summary>
        /// Gets the XPath of the node to select.
        /// </summary>
        /// <value>The XPath of the node to select.</value>
        public string XPath { get; }

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
        public IEnumerable<string> Parse(XDocument input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            return System.Xml.XPath.Extensions.XPathSelectElements(input,XPath)
                .Select((XElement node )=>
                {
                    if (ResultType == NodeParserResultType.Node)
                        return node.ToString(SaveOptions.DisableFormatting);
                    else if (ResultType == NodeParserResultType.Value)
                        return node.Value;
                    else if (ResultType == NodeParserResultType.Attribute)
                        return node.Attribute(Attribute)?.Value;
                    else
                        return node.ToString(SaveOptions.DisableFormatting);
                });
        }
    }
}

//#endif