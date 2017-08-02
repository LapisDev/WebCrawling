/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : IDecompressionProvider
 * Description : Provides methods for stream decompression.
 * Created     : 2016/1/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Downloading
{
    /// <summary>
    /// Provides methods for stream decompression.
    /// </summary>
    public interface IDecompressionProvider
    {
        /// <summary>
        /// Returns a decompressing stream using the GZip algorithm.
        /// </summary>
        /// <param name="stream">The stream to decompress.</param>
        /// <returns>The decompressing stream.</returns>
        Stream DecompressGZip(Stream stream);

        /// <summary>
        /// Returns a decompressing stream using the Deflate algorithm.
        /// </summary>
        /// <param name="stream">The stream to decompress.</param>
        /// <returns>The decompressing stream.</returns>
        Stream DecompressDeflate(Stream stream);
    }
}
