/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : Spider
 * Description : An abstract base class that provides functionality for the 
                 DepthFirstSpider and "BreadthFirstSpider descended classes.
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
    ///   An abstract base class that provides functionality for the <see cref="DepthFirstSpider"/> 
    ///   and <see cref="BreadthFirstSpider"/> descended classes.
    /// </summary>
    public abstract class Spider
    {
        internal Spider(
            IEnumerable<Uri> seedUris,
            ILinkParser linkParser,
            IUriFilter uriFilter,
            int maxDepth,
            HttpRequestConfig httpRequestConfig,
            IDecompressionProvider decompressionProvider,
            int maxTaskCount)
        {
            if (seedUris == null)
                throw new ArgumentNullException();
            if (maxDepth < 0)
                throw new ArgumentOutOfRangeException(ExceptionResource.DepthMustBePositiveInteger);
            if (maxTaskCount <= 0)
                throw new ArgumentOutOfRangeException(ExceptionResource.TaskCountMustBePositiveInteger);

            SeedUris = new UriList(seedUris.Where(u => u != null).Distinct().ToList());
            LinkParser = linkParser;
            UriFilter = uriFilter;
            MaxDepth = maxDepth;
            HttpRequestConfig = httpRequestConfig;
            DecompressionProvider = decompressionProvider;
            MaxTaskCount = maxTaskCount;
            _downloader = new HtmlDownloader(DecompressionProvider);
            _downloader.DownloadCompleted += OnDownloadCompleted;
            Reset();
        }


        /// <summary>
        /// Gets the collection containing the seed URIs.
        /// </summary>
        /// <value>The collection containing the seed URIs.</value>
        public UriCollection SeedUris { get; }

        /// <summary>
        /// Gets the <see cref="ILinkParser"/> used by the <see cref="Spider"/>.
        /// </summary>
        /// <value>The <see cref="ILinkParser"/> used by the <see cref="Spider"/>.</value>
        public ILinkParser LinkParser { get; }

        /// <summary>
        /// Gets the <see cref="IUriFilter"/> used by the <see cref="Spider"/>.
        /// </summary>
        /// <value>The <see cref="IUriFilter"/> used by the <see cref="Spider"/>.</value>
        public IUriFilter UriFilter { get; }

        /// <summary>
        /// Gets the maximum depth of the URIs to be crawled.
        /// </summary>
        /// <value>The maximum depth of the URIs to be crawled.</value>
        public int MaxDepth { get; }

        /// <summary>
        /// Gets the maximum number of tasks in progress.
        /// </summary>
        /// <value>The maximum number of tasks in progress.</value>
        public int MaxTaskCount { get; }

        /// <summary>
        /// Gets the settings related to the HTTP request
        /// </summary>
        /// <value>The settings related to the HTTP request</value>
        public HttpRequestConfig HttpRequestConfig { get; }

        /// <summary>
        /// Gets the <see cref="IDecompressionProvider"/> used when downloading web resources.
        /// </summary>
        /// <value>The <see cref="IDecompressionProvider"/> used when downloading web resources.</value>
        public IDecompressionProvider DecompressionProvider { get; }


        /// <summary>
        /// Gets the collection containing the downloaded URIs.
        /// </summary>
        /// <value>The collection containing the downloaded URIs.</value>
        public UriCollection UrisDownloaded { get { return _urisDownloaded; } }
        private UriList _urisDownloaded = new UriList();

        /// <summary>
        /// Gets the collection containing the URIs being downloaded.
        /// </summary>
        /// <value>The collection containing the URIs being downloaded.</value>
        public UriCollection UrisDownloading { get { return _urisDownloading; } }
        private UriSet _urisDownloading = new UriSet();

        /// <summary>
        /// Gets the collection containing the URIs to be download.
        /// </summary>
        /// <value>The collection containing the URIs to be download.</value>
        public abstract UriCollection UrisToDownload { get; }

        private object _urisSyncRoot = new object();


        internal abstract void PrepareForRunning();

        internal abstract Uri NextUri();

        internal abstract void AddUri(Uri uri);


        /// <summary>
        /// Starts crawling the web pages. This method do not block the calling thread.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="IsBusy"/> is <see langword="true"/>.
        /// </exception>
        /// <remarks>
        ///   <para>
        ///     To receive notification when the operation completes, add an event handler to the
        ///     <see cref="RunCompleted"/> event.
        ///   </para>
        ///   <para>
        ///     Asynchronous operations that have not completed can be canceled using the 
        ///     <see cref="RunAsyncCancel"/> method.
        ///   </para>
        /// </remarks>
        public void RunAsync()
        {
            if (IsBusy)
            {
                throw new InvalidOperationException(ExceptionResource.SpiderIsBusy);
            }
            IsBusy = true;
            IsCancelling = false;
            _taskCount = 0;
            DownloadAsync();
        }

        /// <summary>
        /// Cancels the pending <see cref="RunAsync"/> asynchronous operation.
        /// </summary>
        /// <remarks>
        ///   When you call <see cref="RunAsyncCancel"/> , your application still receives the 
        ///   completion event associated with the operation. If you have specified an event 
        ///   handler for the <see cref="RunCompleted"/> event, your event handler receives 
        ///   notification that the operation has ended. To learn whether the operation completed 
        ///   successfully, check the <see cref="AsyncCompletedEventArgs.Cancelled"/> property on 
        ///   the base class of <see cref="AsyncCompletedEventArgs"/> in the event data object 
        ///   passed to the event handler.
        /// </remarks>
        public void RunAsyncCancel()
        {
            IsCancelling = true;
        }

        /// <summary>
        /// Resets the <see cref="Spider"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="IsBusy"/> is <see langword="true"/>.
        /// </exception>
        public void Reset()
        {
            if (IsBusy)
            {
                throw new InvalidOperationException(ExceptionResource.SpiderIsBusy);
            }            
            _urisDownloaded.Clear();
            _urisDownloading.Clear();
            _depths.Clear();
            foreach (var uri in SeedUris)
            {
                _depths.Add(uri, 0);
            }
            PrepareForRunning();
        }

        /// <summary>
        /// Occurs when the <see cref="RunAsync"/> asynchronous operation completes.
        /// </summary>
        public event AsyncCompletedEventHandler RunCompleted;
        private void OnRunCompleted(AsyncCompletedEventArgs e)
        {
            IsBusy = false;
            var handler = RunCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Occurs when a web page is downloaded.
        /// </summary>
        public event DownloadCompletedEventHandler PageDownloaded;
        private void OnPageDownloaded(DownloadCompletedEventArgs e)
        {
            var handler = PageDownloaded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="Spider"/> is running.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the <see cref="Spider"/> is running; otherwise, 
        ///   <see langword="false"/>.
        /// </value>
        public bool IsBusy
        {
            get
            {
                lock (_isBusySyncRoot)
                {
                    return _isBusy;
                }
            }
            private set
            {
                lock (_isBusySyncRoot)
                {
                    _isBusy = value;
                }
            }
        }
        private bool _isBusy;
        private object _isBusySyncRoot = new object();

        private bool IsCancelling
        {
            get
            {
                lock (_isCancellingSyncRoot)
                {
                    return _isCancelling;
                }
            }
            set
            {
                lock (_isCancellingSyncRoot)
                {
                    _isCancelling = value;
                }
            }
        }
        private bool _isCancelling;
        private object _isCancellingSyncRoot = new object();

        private void DownloadAsync()
        {
            Uri uri = null;
            lock (_urisSyncRoot)
            {
                while (!IsCancelling && _taskCount < MaxTaskCount && UrisToDownload.Count > 0)
                {
                    _taskCount++;
                    uri = NextUri();
                    _urisDownloading.Add(uri);
                    _downloader.DownloadAsync(uri, uri);
                }
                if (UrisDownloading.Count > 0)
                {
                    return;
                }
                if (UrisToDownload.Count > 0 && !IsCancelling)
                {
                    return;
                }
            }
            if (IsCancelling)
            {
                var e = new AsyncCompletedEventArgs(null, true, null);
                OnRunCompleted(e);
                return;
            }
            else
            {
                var e = new AsyncCompletedEventArgs(null, false, null);
                OnRunCompleted(e);
                return;
            }
        }

        private void OnDownloadCompleted(object sender, DownloadCompletedEventArgs e)
        {
            var uri = e.UserState as Uri;
            if (e.Error == null && !e.Cancelled)
            {
                var html = e.Result;
                if (html != null)
                {
                    int depth = DepthOf(uri);
                    if (LinkParser != null && UriFilter != null && depth < MaxDepth)
                    {
                        var links = LinkParser.Parse(html);
                        foreach (var link in links)
                        {
                            Uri newlink;
                            if (Uri.TryCreate(uri, link, out newlink))
                            {
                                if (UriFilter.Filter(newlink))
                                {
                                    lock (_urisSyncRoot)
                                    {
                                        if (!UrisDownloaded.Contains(newlink) &&
                                            !UrisDownloading.Contains(newlink) &&
                                            !UrisToDownload.Contains(newlink))
                                        {
                                            _depths.Add(newlink, depth + 1);
                                            AddUri(new Uri(uri, link));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            OnPageDownloaded(e);
            lock (_urisSyncRoot)
            {
                _urisDownloading.Remove(uri);
                _urisDownloaded.Add(uri);
                _taskCount--;
            }
            DownloadAsync();
        }

        private Dictionary<Uri, int> _depths = new Dictionary<Uri, int>();
        private int DepthOf(Uri uri)
        {
            int depth = 0;
            _depths.TryGetValue(uri, out depth);
            return depth;
        }

        private HtmlDownloader _downloader;

        private int _taskCount;
    }
}
