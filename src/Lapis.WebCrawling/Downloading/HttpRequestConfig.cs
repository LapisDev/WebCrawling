/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : HttpRequestConfig
 * Description : Represents the settings related to the HTTP request.
 * Created     : 2016/1/25
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Lapis.WebCrawling.Downloading
{
    /// <summary>
    /// Represents the settings related to the HTTP request.
    /// </summary>
    public struct HttpRequestConfig
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Text.Encoding"/> used to download 
        /// web resources.
        /// </summary>
        /// <value>
        ///   The <see cref="System.Text.Encoding"/> used to download web resources.
        /// </value>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Net.CookieContainer"/> used to download 
        /// web resources.
        /// </summary>
        /// <value>
        ///   The <see cref="System.Net.CookieContainer"/> used to download web resources.
        /// </value>
        public CookieContainer CookieContainer { get; set; }        

        /// <summary>
        /// Gets or sets the User-agent of the HTTP request.
        /// </summary>
        /// <value>The User-agent of the HTTP request.</value>
        public string UserAgent { get; set; }               
    }
}
