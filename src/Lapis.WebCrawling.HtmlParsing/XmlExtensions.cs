/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : XmlExtensions
 * Description : Provides extension methods for conversion of HTML DOM to XML.
 * Created     : 2015/8/26
 * Note        : Separated from Html, 2016/1/8
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lapis.WebCrawling.HtmlParsing
{
    /// <summary>
    /// Provides extension methods for conversion of HTML DOM to XML.
    /// </summary>
    public static partial class XmlExtensions
    {
        /// <summary>
        /// Converts a <see cref="HtmlNode"/> to a <see cref="XNode"/>.
        /// </summary>
        /// <param name="node">The <see cref="HtmlNode"/> to be converted.</param>
        /// <returns>
        ///   The <see cref="XNode"/> converted from <paramref name="node"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static XNode ToXml(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException();
            var text = node as HtmlText;
            if (text != null)
                return text.ToXml();
            var comment = node as HtmlComment;
            if (comment != null)
                return comment.ToXml();
            var container = node as HtmlContainer;
            if (container != null)
                return container.ToXml();
            throw new ArgumentException();
        }

        /// <summary>
        /// Converts a <see cref="HtmlComment"/> to a <see cref="XComment"/>.
        /// </summary>
        /// <param name="comment">The <see cref="HtmlComment"/>.</param>
        /// <returns>
        ///   The <see cref="XComment"/> converted from <paramref name="comment"/>. 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static XComment ToXml(this HtmlComment comment)
        {
            if (comment == null)
                throw new ArgumentNullException();
            XComment xml = new XComment(comment.Value ?? string.Empty);
            return xml;
        }

        /// <summary>
        /// Converts a <see cref="HtmlText"/> to a <see cref="XText"/>.
        /// </summary>
        /// <param name="text">The <see cref="HtmlText"/>.</param>
        /// <returns>The <see cref="XText"/> converted from <paramref name="text"/>. </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static XText ToXml(this HtmlText text)
        {
            if (text == null)
                throw new ArgumentNullException();
            XText xml = new XText(text.Value ?? string.Empty);
            return xml;
        }

        /// <summary>
        /// Converts a <see cref="HtmlContainer"/> to a <see cref="XContainer"/>.
        /// </summary>
        /// <param name="container">The <see cref="HtmlContainer"/>.</param>
        /// <returns>
        ///   The <see cref="XContainer"/> converted from <paramref name="container"/>. 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static XContainer ToXml(this HtmlContainer container)
        {
            if (container == null)
                throw new ArgumentNullException();
            var element = container as HtmlElement;
            if (element != null)
                return element.ToXml();
            var document = container as HtmlDocument;
            if (document != null)
                return document.ToXml();
            throw new ArgumentException();
        }

        /// <summary>
        /// Converts a <see cref="HtmlElement"/> to a <see cref="XElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="HtmlElement"/>.</param>
        /// <returns>
        ///   The <see cref="XElement"/> converted from <paramref name="element"/>. 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static XElement ToXml(this HtmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException();
            XElement xml = new XElement(element.Name);
            foreach (var attr in element.Attributes)
                xml.SetAttributeValue(attr.Name, attr.Value ?? string.Empty);
            foreach (var child in element.Children)
                xml.Add(child.ToXml());
            return xml;
        }

        /// <summary>
        /// Converts a <see cref="HtmlDocument"/> to a <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document">The <see cref="HtmlDocument"/>.</param>
        /// <returns>
        ///   The <see cref="XDocument"/> converted from <paramref name="document"/>. 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static XDocument ToXml(this HtmlDocument document)
        {
            if (document == null)
                throw new ArgumentNullException();
            XDocument xml = new XDocument();
            if (document.DocumentType != null)
                xml.AddFirst(document.DocumentType.ToXml());
            foreach (var child in document.Children)
                xml.Add(child.ToXml());
            return xml;
        }

        /// <summary>
        /// Converts a <see cref="HtmlDocumentType"/> to a <see cref="XDocumentType"/>.
        /// </summary>
        /// <param name="documentType">The <see cref="HtmlDocumentType"/>.</param>
        /// <returns>
        ///   The <see cref="XDocumentType"/> converted from <paramref name="documentType"/>. 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static XDocumentType ToXml(this HtmlDocumentType documentType)
        {
            if (documentType == null)
                throw new ArgumentNullException();
            XDocumentType xml = new XDocumentType(documentType.Name, documentType.PublicId, documentType.SystemId, "");
            return xml;
        }

        /// <summary>
        /// Returns the XPath for a HTML element.
        /// </summary>
        /// <param name="element">The HTML element.</param>
        /// <returns>A string containing the XPath.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static string GetXPath(this HtmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            var sb = new StringBuilder();
            HtmlNode node = element;
            bool stop = false;
            while (!stop && node != null &&
                !(node is HtmlDocument))
            {
                string str = "";
                if (node is HtmlElement)
                {
                    var id = node.Id();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        str = $".//*[@id='{id}']";
                        stop = true;
                        goto done;
                    }                   
                    var tagName = (node as HtmlElement).Name;
                    {
                        int position;
                        if ((node as HtmlElement).IsUniqueTagName(out position))
                            str = $"{tagName.ToLower()}";
                        else
                            str = $"{tagName.ToLower()}[{position}]";
                    }
                    done:
                    if (sb.Length > 0)
                        sb.Insert(0, "/").Insert(0, str);
                    else
                        sb.Append(str);
                }
                node = node.Parent;
            }
            return sb.ToString();
        }
    }
}
