/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : HtmlDownloader
 * Description : Provides methods for downloading the HTML content of web resources.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Lapis.WebCrawling.Downloading
{
    /// <summary>
    /// Provides methods for downloading the HTML content of web resources.
    /// </summary>
    public class HtmlDownloader
    {
        /// <summary>
        /// Gets the <see cref="IDecompressionProvider"/> used when downloading web resources.
        /// </summary>
        /// <value>
        ///   The <see cref="IDecompressionProvider"/> used when downloading web resources.
        /// </value>
        public IDecompressionProvider DecompressionProvider { get; }       

        /// <summary>
        /// Gets the settings related to the HTTP request.
        /// </summary>
        /// <value>The settings related to the HTTP request.</value>
        public HttpRequestConfig HttpRequestConfig { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlDownloader"/> class.
        /// </summary>
        public HtmlDownloader()
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HtmlDownloader"/> class with a
        ///   <see cref="HttpRequestConfig"/> object.
        /// </summary>
        /// <param name="httpRequestConfig">The settings related to the HTTP request.</param>
        public HtmlDownloader(HttpRequestConfig httpRequestConfig)
        {
            HttpRequestConfig = httpRequestConfig;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HtmlDownloader"/> class with a
        ///   <see cref="IDecompressionProvider"/> object.
        /// </summary>
        /// <param name="decompressionProvider">
        ///   The <see cref="IDecompressionProvider"/> used when downloading web resources.
        /// </param>
        public HtmlDownloader(IDecompressionProvider decompressionProvider)
        {
            DecompressionProvider = decompressionProvider;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HtmlDownloader"/> class with 
        ///   the specified parameters.
        /// </summary>
        /// <param name="httpRequestConfig">The settings related to the HTTP request.</param>
        /// <param name="decompressionProvider">
        ///   The <see cref="IDecompressionProvider"/> used when downloading web resources.
        /// </param>
        public HtmlDownloader(HttpRequestConfig httpRequestConfig, IDecompressionProvider decompressionProvider)
        {
            HttpRequestConfig = httpRequestConfig;
            DecompressionProvider = decompressionProvider;
        }

        /// <summary>
        ///  Downloads the resource identified by the specified URI. This method do not 
        ///  block the calling thread.
        /// </summary>
        /// <param name="uri">The URI of the resource to download.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///   <para>
        ///     To receive notification when the operation completes, add an event handler to the
        ///     <see cref="DownloadCompleted"/> event.
        ///   </para>
        ///   <para>
        ///     Asynchronous operations that have not completed can be canceled using the 
        ///     <see cref="DownloadAsyncCancel"/> method.
        ///   </para>
        /// </remarks>
        public void DownloadAsync(Uri uri)
        {
            DownloadAsync(uri, new object());
        }    

        /// <summary>
        ///  Downloads the resource identified by the specified URI. This method do not 
        ///  block the calling thread, and allows the caller to pass an object to the method 
        ///  that is invoked when the operation completes.
        /// </summary>
        /// <param name="uri">The URI of the resource to download.</param>
        /// <param name="userState">
        ///   A user-defined object that is passed to the method invoked when the asynchronous 
        ///   operation completes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///   <para>
        ///     To receive notification when the operation completes, add an event handler to the
        ///     <see cref="DownloadCompleted"/> event.
        ///   </para>
        ///   <para>
        ///     Asynchronous operations that have not completed can be canceled using the 
        ///     <see cref="DownloadAsyncCancel"/> method.
        ///   </para>
        /// </remarks>
        public void DownloadAsync(Uri uri, object userState)
        {
            if (uri == null || userState == null)
                throw new ArgumentNullException();
            lock (_syncRoot)
            {
                if (_userStates.ContainsKey(userState))
                {
                    throw new ArgumentException(string.Format(ExceptionResource.MustBeUnique, "userState"));
                }
                _userStates.Add(userState, false);
            }
            Request(uri, userState);
        }

        /// <summary>
        /// Occurs when an asynchronous download operation completes.
        /// </summary>
        public event DownloadCompletedEventHandler DownloadCompleted;

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        /// <param name="userState">
        ///   A user-defined object that is passed to the asynchronous operation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///   When you call <see cref="DownloadAsyncCancel"/> , your application still receives 
        ///   the completion event associated with the operation. If you have specified an event 
        ///   handler for the <see cref="DownloadCompleted"/> event, your event handler receives 
        ///   notification that the operation has ended. To learn whether the operation completed 
        ///   successfully, check the <see cref="AsyncCompletedEventArgs.Cancelled"/> property on 
        ///   the base class of <see cref="AsyncCompletedEventArgs"/> in the event data object 
        ///   passed to the event handler.
        /// </remarks>
        public void DownloadAsyncCancel(object userState)
        {
            if (userState == null)
                throw new ArgumentNullException();
            lock (_syncRoot)
            {
                if (_userStates.ContainsKey(userState))
                {
                    _userStates[userState] = true;
                }
            }
        }

        private void OnDownloadCompleted(DownloadCompletedEventArgs e)
        {
            var handler = DownloadCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        private void Request(Uri uri, object userState)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.CookieContainer = HttpRequestConfig.CookieContainer;
                request.Headers["UserAgent"] = HttpRequestConfig.UserAgent;
                RequestState state = new RequestState(uri, userState);
                state.Request = request;
                state.Encoding = HttpRequestConfig.Encoding;
                if (IsCanceled(state.UserState))
                {
                    RaiseDownloadCompleted(uri, null, true, state.UserState);
                    return;
                }
                IAsyncResult result = request.BeginGetResponse(GetResponseCallback, state);
            }
            catch (WebException ex)
            {
                RaiseDownloadCompleted(uri, ex, false, userState);
            }
            catch (ProtocolViolationException ex)
            {
                RaiseDownloadCompleted(uri, ex, false, userState);
            }
        }

        private void GetResponseCallback(IAsyncResult ar)
        {
            RequestState state = (RequestState)ar.AsyncState;
            try
            {
                HttpWebRequest request = state.Request;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
                if (IsCanceled(state.UserState))
                {
                    RaiseDownloadCompleted(state.Uri, null, true, state.UserState);
                    return;
                }
                state.Response = response;
                if (state.Encoding == null)
                {
                    var charset = _regexCharset.Match(response.ContentType);
                    if (charset.Success)
                    {
                        Encoding encoding;
                        try
                        {
                            encoding = Encoding.GetEncoding(charset.Value);
                        }
                        catch (ArgumentException)
                        {
                            encoding = Encoding.UTF8;
                        }
                        state.Encoding = encoding;
                    }
                    else
                    {
                        state.Encoding = Encoding.UTF8;
                    }
                }
                Stream stream = response.GetResponseStream();
                if (DecompressionProvider != null)
                {
                    var contentEncoding = response.Headers["Content-Encoding"]?.ToLower() ?? "";
                    if (contentEncoding.Contains("gzip"))
                    {
                        stream = DecompressionProvider.DecompressGZip(stream) ?? stream;
                    }
                    else if (contentEncoding.Contains("deflate"))
                    {
                        stream = DecompressionProvider.DecompressDeflate(stream) ?? stream;
                    }
                }
                state.Stream = stream;
                IAsyncResult result = stream.BeginRead(state.Buffer, 0, state.BufferSize,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (WebException ex)
            {
                RaiseDownloadCompleted(state.Uri, ex, false, state.UserState);
            }
            catch (ProtocolViolationException ex)
            {
                RaiseDownloadCompleted(state.Uri, ex, false, state.UserState);
            }
            catch (IOException ex)
            {
                state.Response.Dispose();
                RaiseDownloadCompleted(state.Uri, ex, false, state.UserState);
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            RequestState state = (RequestState)ar.AsyncState;
            try
            {
                Stream stream = state.Stream;
                int read = stream.EndRead(ar);
                if (IsCanceled(state.UserState))
                {
                    RaiseDownloadCompleted(state.Uri, null, true, state.UserState);
                    return;
                }
                if (read > 0)
                {
                    MemoryStream memory = new MemoryStream(state.Buffer, 0, read);
                    StreamReader reader = new StreamReader(memory, state.Encoding);
                    string str = reader.ReadToEnd();
                    state.StringBuilder.Append(str);
                    var result = stream.BeginRead(state.Buffer, 0, state.BufferSize,
                        new AsyncCallback(ReadCallback), state);
                    return;
                }
                else
                {
                    var result = state.StringBuilder.ToString();
                    stream.Dispose();
                    state.Response.Dispose();
                    RaiseDownloadCompleted(state.Uri, state.Encoding, result, state.UserState);
                }
            }
            catch (WebException ex)
            {
                RaiseDownloadCompleted(state.Uri, ex, false, state.UserState);
            }
            catch (ProtocolViolationException ex)
            {
                RaiseDownloadCompleted(state.Uri, ex, false, state.UserState);
            }
            catch (IOException ex)
            {
                state.Response.Dispose();
                RaiseDownloadCompleted(state.Uri, ex, false, state.UserState);
            }
        }

        private void RaiseDownloadCompleted(Uri uri, Encoding encoding, string result, object userState)
        {
            lock (_syncRoot)
            {
                _userStates.Remove(userState);
            }
            var e = new DownloadCompletedEventArgs(uri, encoding, result, userState);
            OnDownloadCompleted(e);
        }

        private void RaiseDownloadCompleted(Uri uri, Exception error, bool cancelled, object userState)
        {
            lock (_syncRoot)
            {
                _userStates.Remove(userState);
            }
            var e = new DownloadCompletedEventArgs(uri, error, cancelled, userState);
            OnDownloadCompleted(e);
        }

        private bool IsCanceled(object userState)
        {
            lock (_syncRoot)
            {
                return (_userStates[userState] == true);
            }
        }

        private Dictionary<object, bool> _userStates = new Dictionary<object, bool>();

        private object _syncRoot = new object();


        private Regex _regexCharset = new Regex(@"(?is)(?<=char\-?set[:=])\S+");
    }
}
