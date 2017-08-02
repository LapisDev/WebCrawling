/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : DownloadCompletedEventArgs
 * Description : Provides data for the DownloadCompleted event.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Downloading
{
    /// <summary>
    /// Provides data for the <see cref="HtmlDownloader.DownloadCompleted"/> event.
    /// </summary>
    public class DownloadCompletedEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the downloaded web resource.
        /// </summary>
        /// <value>The URI of the downloaded web resource.</value>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets the HTML content of the downloaded web resource.
        /// </summary>
        /// <value>The HTML content of the downloaded web resource.</value>
        /// <remark>
        ///   You should check the <see cref="AsyncCompletedEventArgs.Error"/> and 
        ///   <see cref="AsyncCompletedEventArgs.Cancelled"/> properties before using
        ///   the data that is returned by this property. If the 
        ///   <see cref="AsyncCompletedEventArgs.Error"/> property's value is an 
        ///   <see cref="Exception"/> object or the <see cref="AsyncCompletedEventArgs.Cancelled"/> 
        ///   property's value is <see langword="true"/>, the asynchronous operation 
        ///   did not complete correctly and the <see cref="Result"/> property's value 
        ///   will not be valid.
        /// </remark>
        /// <exception cref="InvalidOperationException">
        ///   The value of <see cref="AsyncCompletedEventArgs.Cancelled"/> is <see langword="true"/>.
        /// </exception>
        /// <exception cref="System.Reflection.TargetInvocationException">
        ///   If an error occurred during an asynchronous operation, the property will raise a 
        ///   <see cref="System.Reflection.TargetInvocationException"/> with its 
        ///   <see cref="Exception.InnerException"/> property holding a reference to 
        ///   <see cref="AsyncCompletedEventArgs.Error"/>.
        /// </exception>
        public string Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _result;
            }
        }

        /// <summary>
        /// Gets the encoding of the downloaded web resource.
        /// </summary>
        /// <value>The encoding of the downloaded web resource.</value>
        /// <remark>
        ///   You should check the <see cref="AsyncCompletedEventArgs.Error"/> and 
        ///   <see cref="AsyncCompletedEventArgs.Cancelled"/> properties before using
        ///   the data that is returned by this property. If the 
        ///   <see cref="AsyncCompletedEventArgs.Error"/> property's value is an 
        ///   <see cref="Exception"/> object or the <see cref="AsyncCompletedEventArgs.Cancelled"/> 
        ///   property's value is <see langword="true"/>, the asynchronous operation 
        ///   did not complete correctly and the <see cref="Result"/> property's value 
        ///   will not be valid.
        /// </remark>
        /// <exception cref="InvalidOperationException">
        ///   The value of <see cref="AsyncCompletedEventArgs.Cancelled"/> is <see langword="true"/>.
        /// </exception>
        /// <exception cref="System.Reflection.TargetInvocationException">
        ///   If an error occurred during an asynchronous operation, the property will raise a 
        ///   <see cref="System.Reflection.TargetInvocationException"/> with its 
        ///   <see cref="Exception.InnerException"/> property holding a reference to 
        ///   <see cref="AsyncCompletedEventArgs.Error"/>.
        /// </exception>
        public Encoding Encoding
        {
            get
            {
                if (Error != null || Cancelled)
                    RaiseExceptionIfNecessary();
                return _encoding;
            }
        }

        internal DownloadCompletedEventArgs(Uri uri, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            Uri = uri;
        }

        internal DownloadCompletedEventArgs(Uri uri, Encoding encoding, string result, object userState)
           : base(null, false, userState)
        {
            Uri = uri;
            _encoding = encoding;
            _result = result;
        }

        private string _result;

        private Encoding _encoding;

    }

    /// <summary>
    /// Represents the method that will handle the <see cref="HtmlDownloader.DownloadCompleted"/> event.
    /// </summary>
    /// <param name="sender">The instance of the object for which to invoke this method.</param>
    /// <param name="e">
    ///   The <see cref="DownloadCompletedEventArgs"/> that specifies the reason the event was invoked.
    /// </param>
    public delegate void DownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs e);
}
