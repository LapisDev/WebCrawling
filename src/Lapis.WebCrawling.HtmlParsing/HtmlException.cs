/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlException
 * Description : The exception that is thrown when parsing HTML.
 * Created     : 2016/1/7
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.Tokenization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing
{
    /// <summary>
    /// The exception that is thrown when parsing HTML.
    /// </summary>
    public class HtmlException : Exception
    {
        /// <summary>
        /// Gets the position where the exception is thrown.
        /// </summary>
        /// <value>The position where the exception is thrown.</value>
        public Location Location { get; }

        internal HtmlException(Location location, string message)
            : base(location + ": " + message)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));
            Location = location;
        }
    }
}
