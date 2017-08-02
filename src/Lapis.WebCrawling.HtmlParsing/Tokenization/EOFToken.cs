/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : EOFToken
 * Description : Represents a end of file token.
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
    /// Represents a end of file token.
    /// </summary>
    public class EOFToken : Token
    {
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Location} : EOF";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EOFToken"/> class with the specified parameters.
        /// </summary>
        /// <param name="location">The position of the token.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="location"/> is <see langword="null"/>.
        /// </exception>
        public EOFToken(Location location) : base(location) { }
    }
}
