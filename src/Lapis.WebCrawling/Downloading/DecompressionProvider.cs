/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : DecompressionProvider
 * Description : Implements a default IDecompressionProvider.
 * Created     : 2016/1/18
 * Note        :
*********************************************************************************/

//#if SYSTEM_IO_COMPRESSION

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Lapis.WebCrawling.Downloading
{
    /// <summary>
    /// Implements a default <see cref="IDecompressionProvider"/>.
    /// </summary>
    public class DecompressionProvider : IDecompressionProvider
    {
        /// <summary>
        /// Returns a decompressing stream using the Deflate algorithm.
        /// </summary>
        /// <param name="stream">The stream to decompress.</param>
        /// <returns>The decompressing stream.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="stream"/> is <see langword="null"/>.
        /// </exception>
        public Stream DecompressDeflate(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            return new DeflateStream(stream, CompressionMode.Decompress);
        }

        /// <summary>
        /// Returns a decompressing stream using the GZip algorithm.
        /// </summary>
        /// <param name="stream">The stream to decompress.</param>
        /// <returns>The decompressing stream.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="stream"/> is <see langword="null"/>.
        /// </exception>
        public Stream DecompressGZip(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            return new GZipStream(stream, CompressionMode.Decompress);
        }
    }
}

//#endif 