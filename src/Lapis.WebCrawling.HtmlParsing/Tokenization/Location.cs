/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Location
 * Description : Represents the position of a character.
 * Created     : 2015/8/25
 * Note        : 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.Tokenization
{
    /// <summary>
    /// Represents the position of a character.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets the line number of the character.
        /// </summary>
        /// <value>The line number of the character.</value>
        public int Line { get; }

        /// <summary>
        /// Gets the position of the character in the current line.
        /// </summary>
        /// <value>The position of the character in the current line.</value>
        public int Span { get; }

        /// <summary>
        /// Returns the string representation of the <see cref="Location"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="Location"/> object.
        /// </returns>
        public override string ToString()
        {          
            return "Line " + Line.ToString() + ", " + "Span " + Span.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class with the specified parameters.
        /// </summary>
        /// <param name="line">The line number of the character.</param>
        /// <param name="span">The position of the character in the current line.</param>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is zero or negative.</exception>
        public Location(int line, int span)
        {
            if (line <= 0 || span < 0)
                throw new ArgumentOutOfRangeException();
            Line = line;
            Span = span;
        }               
    }
}
