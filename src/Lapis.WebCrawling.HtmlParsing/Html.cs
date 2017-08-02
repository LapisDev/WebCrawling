/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Html
 * Description : Provides methods for HTML parsing.
 * Created     : 2015/8/26
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lapis.WebCrawling.HtmlParsing
{
    /// <summary>
    /// Provides methods for HTML parsing.
    /// </summary>
    public static partial class Html
    {
        /// <summary>
        /// Parses a HTML string and returns the HTML document.
        /// </summary>  
        /// <param name="html">The HTML string to parse.</param>
        /// <returns>The HTML document.</returns>
        public static HtmlDocument Parse(string html)
        {
            var treeConstructor = new TreeConstructor(html);
            return treeConstructor.Parse();
        }

        /// <summary>
        /// Parses a HTML string and converts to <see cref="XDocument"/>.
        /// </summary>
        /// <param name="html">The HTML string to parse.</param>
        /// <returns>The <see cref="XDocument"/> converted from <paramref name="html"/>.</returns>
        public static XDocument ToXml(string html)
        {
            return Parse(html).ToXml();
        }
    }
}
