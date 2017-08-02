/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlText
 * Description : Represents a text node.
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
    /// Represents a text node.
    /// </summary>
    public class HtmlText : HtmlNode
    {
        /// <summary>
        /// Gets or sets the value of this node.
        /// </summary>
        /// <value>The value of this node.</value>
        public string Value { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HtmlText"/> class with 
        ///   the specified value.
        /// </summary>
        /// <param name="value">The value of this node.</param>
        public HtmlText(string value)
        {
            Value = value;
        }
        
        /// <inheritdoc/>
        public override string ToString(int indentation)
        {
            var ind = new string(' ', indentation);
            return ind + Value;
        }

        /// <inheritdoc/>
        public override string ToString(HtmlFormat format)
        {
            if (format == HtmlFormat.Compressed)
                return Value;
            else
                return ToString(0);
        }
    }
}
