/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : CharacterToken
 * Description : Represents a character token.
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
    /// Represents a character token.
    /// </summary>
    public class CharacterToken : Token
    {
        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        /// <value>The character.</value>
        public char Char { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CharacterToken"/> class 
        ///   with the specified parameters.
        /// </summary>
        /// <param name="location">The position of the token.</param>
        /// <param name="c">The character.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="location"/> is <see langword="null"/>.
        /// </exception>
        public CharacterToken(Location location, char c)
            : base(location)
        {
            Char = c;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Location} : {Char.ToString()}";
        }
    }
}
