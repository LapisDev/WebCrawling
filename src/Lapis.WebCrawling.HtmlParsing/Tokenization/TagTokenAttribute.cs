/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : TagTokenAttribute
 * Description : Represents an attribute in a tag.
 * Created     : 2015/8/24
 * Note        : 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.Tokenization
{
    /// <summary>
    /// Represents an attribute in a tag.
    /// </summary>
    public class TagTokenAttribute
    {
        /// <summary>
        /// Gets or sets the attribute name.
        /// </summary>
        /// <value>The attribute name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the attribute value.
        /// </summary>
        /// <value>The attribute value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Returns the string representation of the <see cref="TagTokenAttribute"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="TagTokenAttribute"/> object.
        /// </returns>
        public override string ToString()
        {
            if (Name != null)
                return $"{Name}=\"{Value}\"";
            else
                return string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagTokenAttribute"/> class.
        /// </summary>
        public TagTokenAttribute()
        { }
    }
}
