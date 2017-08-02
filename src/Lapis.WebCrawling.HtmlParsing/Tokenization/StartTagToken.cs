/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : StartTagToken
 * Description : Represents a start tag token.
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
    /// Represents a start tag token.
    /// </summary>
    public class StartTagToken : TagToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndTagToken"/> class with the specified parameters.
        /// </summary>
        /// <param name="location">The position of the token.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="location"/> is <see langword="null"/>.
        /// </exception>
        public StartTagToken(Location location)
            : base(location)
        { }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (IsSelfClosing)
                return $"{Location} : <{Name} {Attributes}/>";
            else
                return $"{Location} : <{Name} {Attributes}>";
        }
    }
}
