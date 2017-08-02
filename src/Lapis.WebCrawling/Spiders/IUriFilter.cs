/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : IUriFilter
 * Description : Provides methods for filtering the URIs.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;

namespace Lapis.WebCrawling.Spiders
{
    /// <summary>
    /// Provides methods for filtering the URIs.
    /// </summary>
    public interface IUriFilter
    {
        /// <summary>
        /// Returns a value indicating whether the URI satisfies the condition.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        ///   <see langword="true"/> if <paramref name="uri"/> satisfies the condition; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        bool Filter(Uri uri);
    }
}