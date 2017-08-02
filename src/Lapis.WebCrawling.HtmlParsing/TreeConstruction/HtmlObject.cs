/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlObject
 * Description : Represents a node or an attribute in an HTML tree.
 * Created     : 2015/8/25
 * Note        : 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.TreeConstruction
{
    /// <summary>
    /// Represents a node or an attribute in an HTML tree.
    /// </summary>
    public abstract class HtmlObject
    {
        /// <summary>
        /// Returns the string representation of the <see cref="HtmlObject"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="HtmlObject"/> object.
        /// </returns>
        public abstract override string ToString();
    }
}
