/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : CssSelectorException
 * Description : The exception that is thrown when parsing a css selector.
 * Created     : 2016/1/7
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.CssSelectors
{
    /// <summary>
    /// The exception that is thrown when parsing a css selector.
    /// </summary>
    public class CssSelectorException : Exception
    {  
        /// <summary>
        /// Gets the position where the exception is thrown.
        /// </summary>
        /// <value>The position where the exception is thrown.</value>
        public int Position { get; }

        internal CssSelectorException(int position, string message)
            : base(position + ": " + message)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            Position = position;
        }
    }
}
