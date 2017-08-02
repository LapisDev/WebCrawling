/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : BreadthFirstSpider
 * Description : Implements a Spider that uses the breadth first mode.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.Downloading;
using Lapis.WebCrawling.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace Lapis.WebCrawling.Spiders
{
    /// <summary>
    /// Implements a <see cref="Spider"/> that uses the breadth first mode.
    /// </summary>
    public class BreadthFirstSpider : Spider
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="BreadthFirstSpider"/> class with 
        ///   the specified parameters.
        /// </summary>
        /// <param name="seedUris">A collection containing the seed URIs.</param>
        /// <param name="linkParser">
        ///   The <see cref="ILinkParser"/> used by the <see cref="Spider"/>.
        /// </param>
        /// <param name="maxDepth">The maximum depth of the URIs to be crawled.</param>
        /// <param name="uriFilter">
        ///   The <see cref="IUriFilter"/> used by the <see cref="Spider"/>.
        /// </param>
        /// <param name="maxTaskCount">The maximum number of tasks in progress.</param>
        /// <param name="httpRequestConfig">The settings related to the HTTP request.</param>
        /// <param name="decompressionProvider">
        ///   The <see cref="IDecompressionProvider"/> used when downloading web resources.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="seedUris"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para><paramref name="maxDepth"/> is negative.</para>
        ///   <para>-or-</para>
        ///   <para><paramref name="maxTaskCount"/> is zero or negative.</para>
        /// </exception>
        public BreadthFirstSpider(
            IEnumerable<Uri> seedUris,
            ILinkParser linkParser,
            IUriFilter uriFilter,
            int maxDepth,
            HttpRequestConfig httpRequestConfig,
            IDecompressionProvider decompressionProvider,
            int maxTaskCount)
            : base(
                  seedUris: seedUris,
                  linkParser: linkParser,
                  uriFilter: uriFilter,
                  maxDepth: maxDepth,
                  httpRequestConfig: httpRequestConfig,
                  decompressionProvider: decompressionProvider,
                  maxTaskCount: maxTaskCount)
        { }

        /// <inheritdoc/>
        public override UriCollection UrisToDownload
        {
            get { return _urisToDownload; }
        }

        internal override void PrepareForRunning()
        {
            _urisToDownload.Clear();
            foreach (var uri in SeedUris)
                _urisToDownload.Enqueue(uri);
        }

        internal override Uri NextUri()
        {
            return _urisToDownload.Dequeue();
        }

        internal override void AddUri(Uri uri)
        {
            _urisToDownload.Enqueue(uri);
        }


        private UriQueue _urisToDownload = new UriQueue();
    }
}
