/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlComment
 * Description : Represents a HTML comment.
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
    /// Represents a HTML comment.
    /// </summary>
    public class HtmlComment : HtmlNode
    {
        /// <summary>
        /// Gets or sets the text of the comment.
        /// </summary>
        /// <value>The text of the comment.</value>
        public string Value { get; set; }
       
        /// <summary>
        ///   Initializes a new instance of the <see cref="HtmlComment"/> class with 
        ///   the specified value.
        /// </summary>
        /// <param name="value">The text of the comment.</param>
        public HtmlComment(string value)
        {
            Value = value;
        }
        
        /// <inheritdoc/>
        public override string ToString(int indentation)
        {
            var ind = new string(' ', indentation);
            return $"{ind}<!--{Value}-->";
        }

        /// <inheritdoc/>
        public override string ToString(HtmlFormat format)
        {
            if (format == HtmlFormat.Compressed)
                return $"<!--{Value}-->";
            else
                return ToString(0);
        }
    }
}
