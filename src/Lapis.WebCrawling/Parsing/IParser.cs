/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : IParser
 * Description : Provides methods that parse the source and return results.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Parsing
{

    /// <summary>
    /// Provides methods that parse the source and return results.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IParser<in TSource, out TResult>
    {
        /// <summary>
        /// Parses the source and returns the results.
        /// </summary>
        /// <param name="input">The source to parse.</param>
        /// <returns>The results.</returns>
        IEnumerable<TResult> Parse(TSource input);
    }
}
