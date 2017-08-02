/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlNode
 * Description : Represents the abstract concept of a node in the XML tree.
 * Created     : 2015/8/25
 * Note        : 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.TreeConstruction
{
    /// <summary>
    /// Represents the abstract concept of a node in the XML tree.
    /// </summary>
    public abstract class HtmlNode : HtmlObject
    {

        /// <summary>
        /// Gets the parent <see cref="HtmlContainer"/> of the node.
        /// </summary>
        /// <value>The parent node.</value>
        public HtmlContainer Parent { get; internal set; }

        /// <summary>
        ///   Returns the string representation of the <see cref="HtmlNode"/> object with
        ///   the specified indent number of characters.
        /// </summary>
        /// <param name="indentation">The indent number of characters.</param>
        /// <returns>
        ///   The string representation of the <see cref="HtmlNode"/> object.
        /// </returns>
        public abstract string ToString(int indentation);

        /// <summary>
        /// Returns the string representation of the <see cref="HtmlNode"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="HtmlNode"/> object.
        /// </returns>
        public override string ToString()
        {
            return ToString(0);
        }

        /// <summary>
        ///   Returns the string representation of the <see cref="HtmlNode"/> object with
        ///   the specified <see cref="HtmlFormat"/>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///   The string representation of the <see cref="HtmlNode"/> object.
        /// </returns>
        public abstract string ToString(HtmlFormat format);

        internal HtmlNode() : base() { }
    }

    /// <summary>
    /// Represents the format of the HTML written.
    /// </summary>
    public enum HtmlFormat
    {
        /// <summary>
        /// The value indicates that the written HTML is formatted.
        /// </summary>
        Formatted,
        /// <summary>
        /// The value indicates that the written HTML is compressed.
        /// </summary>
        Compressed
    }
}
