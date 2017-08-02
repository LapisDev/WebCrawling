/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : DelegateParser
 * Description : Implements an IParser that wraps a delegate.
 * Created     : 2015/8/27
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Parsing
{
    /// <summary>
    ///   Implements an <see cref="IParser{TSource, TResult}"/> that wraps a 
    ///   delegate.
    /// </summary>
    /// <typeparam name="TSource">The type of source to parse.</typeparam>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class DelegateParser<TSource, TResult> : IParser<TSource, TResult>
    {
        /// <summary>
        ///  Gets the delegate 
        ///  that the <see cref="DelegateParser{TSource, TResult}"/> wraps.
        /// </summary>
        /// <value>
        ///  The delegate that the <see cref="DelegateParser{TSource, TResult}"/> wraps.
        /// </value>
        public Func<TSource, IEnumerable<TResult>> Func { get; private set; }

        /// <summary>
        ///   Inintialize a new instance of the <see cref="DelegateParser{TSource, TResult}"/> 
        ///   class with a delegate.
        /// </summary>
        /// <param name="func">
        ///  The delegate that the <see cref="DelegateParser{TSource, TResult}"/> wraps.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public DelegateParser(Func<TSource, IEnumerable<TResult>> func)
        {
            if (func == null)
                throw new ArgumentNullException();
            Func = func;
        }

        /// <inheritdoc/>
        public IEnumerable<TResult> Parse(TSource input)
        {
            return Func(input);
        }     
    }
}
