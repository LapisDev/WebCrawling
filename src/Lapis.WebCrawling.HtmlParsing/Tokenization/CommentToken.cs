/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : CommentToken
 * Description : Represents a comment token.
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
    /// Represents a comment token.
    /// </summary>
    public class CommentToken : Token
    {
        /// <summary>
        /// Gets or sets the text of the comment.
        /// </summary>
        /// <value>The text of the comment.</value>
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentToken"/> class with the specified parameters.
        /// </summary>
        /// <param name="location">The position of the token.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="location"/> is <see langword="null"/>.
        /// </exception>
        public CommentToken(Location location)
             : base(location)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentToken"/> class with the specified parameters.
        /// </summary>
        /// <param name="location">The position of the token.</param>
        /// <param name="data">The text of the comment.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="location"/> is <see langword="null"/>.
        /// </exception>
        public CommentToken(Location location, string data)
             : base(location)
        {
            Data = data;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Location} : <!--{Data}-->";
        }
    }
}
