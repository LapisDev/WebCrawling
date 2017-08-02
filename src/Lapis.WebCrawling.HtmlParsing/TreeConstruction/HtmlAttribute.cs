/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlAttribute
 * Description : Represents a HTML attribute.
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
    /// Represents a HTML attribute.
    /// </summary>
    public class HtmlAttribute : HtmlObject
    {
        /// <summary>
        /// Gets the parent <see cref="HtmlElement"/> of the attribute。
        /// </summary>
        /// <value>The parent element of the attribute.</value>
        public HtmlElement Parent { get; internal set; }

        /// <summary>
        /// Gets the attribute name.
        /// </summary>
        /// <value>The attribute name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the attribute value.
        /// </summary>
        /// <value>The value of the attribute.</value>
        public string Value { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HtmlAttribute"/> class with 
        ///   the specified name and value.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="name"/> is <see langword="null"/>.
        /// </exception>
        public HtmlAttribute(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException();
            Name = name;
            Value = value;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Name != null)
                return $"{Name}=\"{Value}\"";
            else
                return string.Empty;
        }
    }
}
