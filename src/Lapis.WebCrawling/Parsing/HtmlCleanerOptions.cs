/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : HtmlCleanerOptions
 * Description : Represents the options used by a HtmlCleaner.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Parsing
{
    /// <summary>
    /// epresents the options used by a <see cref="HtmlCleaner"/>.
    /// </summary>
    [Flags]
    [Obsolete]
    public enum HtmlCleanerOptions
    {
        /// <summary>
        /// The default option.
        /// </summary>
        None,
        /// <summary>
        /// The value indicates that the script nodes in the HTML are to be removed.
        /// </summary>
        RemoveScripts,
        /// <summary>
        /// The value indicates that the style nodes in the HTML are to be removed.
        /// </summary>
        RemoveStyles
    }
}
