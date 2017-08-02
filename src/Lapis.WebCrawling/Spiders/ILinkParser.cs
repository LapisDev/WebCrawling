/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : ILinkParser
 * Description : Provides methods for extracting the URIs in a web page.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Spiders
{
    /// <summary>
    /// Provides methods for extracting the URIs in a web page.
    /// </summary>
    public interface ILinkParser
    {
        /// <summary>
        /// Returns the URIs in the web page.
        /// </summary>
        /// <param name="html">The HTML to parse.</param>
        /// <returns>The URIs in <paramref name="html"/>.</returns>
        IEnumerable<Uri> Parse(string html);
    }
}
