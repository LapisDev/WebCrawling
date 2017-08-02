/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : SpiderCallback
 * Description : References a method to be called when a <see cref="Spider"/> raises an event.
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
    /// References a method to be called when a <see cref="Spider"/> downloads a web page.
    /// </summary>
    /// <param name="uri">The URI of the web page.</param>
    /// <param name="html">The HTML of the web page.</param>   
    public delegate void SpiderCallback(Uri uri, string html);

    /// <summary>
    /// References a method to be called when an error occurs in a <see cref="Spider"/>.
    /// </summary>
    /// <param name="uri">The URI of the web page to download.</param>
    /// <param name="ex">The exception thrown by the <see cref="Spider"/>.</param>   
    public delegate void ErrorCallback(Uri uri, Exception ex);
}
