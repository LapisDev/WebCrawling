/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : EndTagToken
 * Description : Represents an end tag token.
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
    /// Represents an end tag token.
    /// </summary>
    public class EndTagToken : TagToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndTagToken"/> class with the specified parameters.
        /// </summary>
        /// <param name="location">The position of the token.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="location"/> is <see langword="null"/>.
        /// </exception>
        public EndTagToken(Location location)
            : base(location)
        { }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Location} : </{Name}>";
        }
    }
}
