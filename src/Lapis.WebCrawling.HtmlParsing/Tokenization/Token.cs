/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Token
 * Description : Represents a token for parsing HTML.
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
    /// Represents a token for parsing HTML.
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// Gets the position of the token.
        /// </summary>
        /// <value>The position of the token.</value>
        public Location Location { get; }

        /// <summary>
        /// Returns the string representation of the <see cref="Token"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="Token"/> object.
        /// </returns>
        public abstract override string ToString();

        internal Token(Location location)
        {
            if (location == null)
                throw new ArgumentNullException();
            Location = location;
        }
    }
}
