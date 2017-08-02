/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : TagToken
 * Description : Represents a tag token.
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
    /// Represents a tag token. This class is abstract.
    /// </summary>
    public abstract class TagToken : Token
    {
        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        /// <value>The tag name.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the tag is self-closed.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the tag is self-closed; otherwise, 
        ///   <see langword="false"/>.
        /// </value>
        public bool IsSelfClosing { get; set; } = false;

        /// <summary>
        /// Gets the attributes of the tag.
        /// </summary>
        /// <value>The attributes of the tag.</value>
        public TagTokenAttributeCollection Attributes { get; } = new TagTokenAttributeCollection();
            

        internal TagToken(Location location) : base(location) { }
    }
}
