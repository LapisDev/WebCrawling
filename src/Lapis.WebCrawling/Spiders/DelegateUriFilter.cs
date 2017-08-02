/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : DelegateUriFilter
 * Description : Implements an IUriFilter that wraps a delegate.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Spiders
{
    /// <summary>
    ///   Implements an <see cref="IUriFilter"/> that wraps a 
    ///   <see cref="Predicate{Uri}"/> delegate.
    /// </summary>
    public class DelegateUriFilter : IUriFilter
    {
        /// <summary>
        ///  Gets the <see cref="Predicate{Uri}"/> delegate that the <see cref="DelegateUriFilter"/>
        ///  wraps.
        /// </summary>
        /// <value>
        ///  The <see cref="Predicate{Uri}"/> delegate that the <see cref="DelegateUriFilter"/> wraps.
        /// </value>
        public Predicate<Uri> Predicate { get; private set; }

        /// <summary>
        ///   Inintialize a new instance of the <see cref="DelegateUriFilter"/> class using a
        ///   <see cref="Predicate{Uri}"/> delegate.
        /// </summary>
        /// <param name="predicate">
        ///   The <see cref="Predicate{Uri}"/> delegate that the <see cref="DelegateUriFilter"/> wraps.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
       public DelegateUriFilter(Predicate<Uri> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException();
            Predicate = predicate;
        }

        /// <inheritdoc/>
        public bool Filter(Uri uri)
        {
            return Predicate(uri);
        }
    }
}
