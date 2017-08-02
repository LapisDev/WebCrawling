/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : RequestState
 * Description : Represents a state object used by HtmlDownloader.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Lapis.WebCrawling.Downloading
{
    sealed class RequestState 
    {
        public object UserState { get; private set; }

        public Uri Uri { get; private set; }

        public Encoding Encoding { get; set; }

        public StringBuilder StringBuilder { get; private set; }

        public HttpWebRequest Request { get; set; }

        public RequestState(Uri uri, object userState)
        {
            Uri = uri;
            BufferSize = BUFFER_SIZE;
            Buffer = new byte[BufferSize];
            StringBuilder = new StringBuilder();
            UserState = userState;
        }

        public HttpWebResponse Response { get; set; }

        public Stream Stream { get; set; }

        public byte[] Buffer { get; private set; }

        public int BufferSize { get; private set; }
        

        private const int BUFFER_SIZE = 131072;
    }
}
