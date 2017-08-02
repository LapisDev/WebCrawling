/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : SpiderBuilder
 * Description : Provides methods for creating a Spider.
 * Created     : 2015/8/27
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.Downloading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Lapis.WebCrawling.Spiders
{
    /// <summary>
    /// Provides methods for creating a <see cref="Spider"/>.
    /// </summary>
    public class SpiderBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="Spider"/>.
        /// </summary>
        /// <returns>The created <see cref="Spider"/>.</returns>
        public Spider GetSpider()
        {
            var spider = GetSpider(SearchMode);
            var callback = Callback;
            var errorCallback = ErrorCallback;
            if (callback != null && errorCallback != null)
            {
                spider.PageDownloaded += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        callback(e.Uri, e.Result);
                    }
                    else
                    {
                        errorCallback(e.UserState as Uri, e.Error);
                    }
                };
            }
            else if (callback != null)
            {
                spider.PageDownloaded += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        callback(e.Uri, e.Result);
                    }
                };
            }
            else if (errorCallback != null)
            {
                spider.PageDownloaded += (s, e) =>
                {
                    if (e.Error != null)
                    {
                        errorCallback(e.UserState as Uri, e.Error);
                    }
                };
            }
            return spider;
        }
        private Spider GetSpider(SearchMode searchMode)
        {
            if (searchMode == SearchMode.BreadthFirst)
                return new BreadthFirstSpider(
                    seedUris: SeedUris,
                    linkParser: LinkParser,
                    maxDepth: MaxDepth,
                    uriFilter: UriFilter,
                    httpRequestConfig: HttpRequestConfig,
                    decompressionProvider: DecompressionProvider,
                    maxTaskCount: MaxTaskCount);
            else if (searchMode == SearchMode.DepthFirst)
                return new DepthFirstSpider(
                    seedUris: SeedUris,
                    linkParser: LinkParser,
                    maxDepth: MaxDepth,
                    uriFilter: UriFilter,
                    httpRequestConfig: HttpRequestConfig,
                    decompressionProvider: DecompressionProvider,
                    maxTaskCount: MaxTaskCount);
            else
                throw new ArgumentOutOfRangeException(nameof(searchMode));
        }

        /// <summary>
        /// Gets or sets the search mode used by the <see cref="Spider"/>.
        /// </summary>
        /// <value>The search mode used by the <see cref="Spider"/>.</value>
        public SearchMode SearchMode { get; set; }

        /// <summary>
        /// Gets or sets the collection containing the seed URIs.
        /// </summary>
        /// <value>The collection containing the seed URIs.</value>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public IEnumerable<Uri> SeedUris
        {
            get { return _sendUris; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _sendUris = value;
            }
        }
        private IEnumerable<Uri> _sendUris = Enumerable.Empty<Uri>();

        /// <summary>
        /// Gets or sets the <see cref="ILinkParser"/> used by the <see cref="Spider"/>.
        /// </summary>
        /// <value>The <see cref="ILinkParser"/> used by the <see cref="Spider"/>.</value>
        public ILinkParser LinkParser { get; set; } = new LinkParser();

        /// <summary>
        /// Gets or sets the <see cref="IUriFilter"/> used by the <see cref="Spider"/>.
        /// </summary>
        /// <value>The <see cref="IUriFilter"/> used by the <see cref="Spider"/>.</value>
        public IUriFilter UriFilter { get; set; }

        /// <summary>
        /// Gets or sets the maximum depth of the URIs to be crawled.
        /// </summary>
        /// <value>The maximum depth of the URIs to be crawled.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   The parameter is negative.
        /// </exception>
        public int MaxDepth
        {
            get { return _maxDepth; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(ExceptionResource.DepthMustBePositiveInteger);
                _maxDepth = value;
            }
        }
        private int _maxDepth = 0;

        /// <summary>
        /// Gets or sets the maximum number of tasks in progress.
        /// </summary>
        /// <value>The maximum number of tasks in progress.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   The parameter is zero or negative.
        /// </exception>
        public int MaxTaskCount
        {
            get { return _maxTaskCount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(ExceptionResource.TaskCountMustBePositiveInteger);
                _maxTaskCount = value;
            }
        }
        private int _maxTaskCount = 1;

        /// <summary>
        /// Gets or sets the action when a web page is downloaded.
        /// </summary>
        /// <value>The action when a web page is downloaded.</value>
        public SpiderCallback Callback { get; set; }

        /// <summary>
        /// Gets or sets the action when an error occurs.
        /// </summary>
        /// <value>The action when an error occurs.</value>
        public ErrorCallback ErrorCallback { get; set; }


        /// <summary>
        /// Gets or sets the settings related to the HTTP request
        /// </summary>
        /// <value>The settings related to the HTTP request</value>
        public HttpRequestConfig HttpRequestConfig { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref="IDecompressionProvider"/> used when downloading 
        ///   web resources.
        /// </summary>
        /// <value>
        ///   The <see cref="IDecompressionProvider"/> used when downloading web resources.
        /// </value>
        public IDecompressionProvider DecompressionProvider { get; set; }

    }

    /// <summary>
    /// Represents the search mode used by a <see cref="Spider"/>.
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// The breadth first mode.
        /// </summary>
        BreadthFirst,
        /// <summary>
        /// The depth first mode.
        /// </summary>
        DepthFirst
    }
}
