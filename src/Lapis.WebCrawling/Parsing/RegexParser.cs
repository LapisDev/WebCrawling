/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : RegexParser
 * Description : Implements an IParser that wraps a Regex.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lapis.WebCrawling.Parsing
{
    /// <summary>
    ///   Implements an <see cref="IParser{String, TResult}"/> that wraps a 
    ///   <see cref="Regex"/> object.
    /// </summary>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class RegexParser<TResult> : IParser<string, TResult>
    {
        /// <summary>
        ///   Gets the <see cref="Regex"/> object wrapped by the 
        ///   <see cref="RegexParser{TResult}"/> object.
        /// </summary>
        /// <value>
        ///   The <see cref="Regex"/> wrapped by the <see cref="RegexParser{TResult}"/>.
        /// </value>
        public Regex Regex { get; private set; }

        /// <summary>
        /// Gets the selector that converts the <see cref="Match"/> to the result.
        /// </summary>
        /// <value>The selector that converts the <see cref="Match"/> to the result.</value>
        public Func<Match, TResult> Selector { get; private set; }

        /// <inheritdoc/>
        public IEnumerable<TResult> Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            var match = Regex.Match(input);
            while (match.Success)
            {
                yield return Selector(match);
                match = match.NextMatch();
            }
        }

        /// <summary>
        ///   Inintialize a new instance of the <see cref="RegexParser{TResult}"/> class 
        ///   with the specified parameters.
        /// </summary>
        /// <param name="regex">
        ///   The <see cref="Regex"/> wrapped by the <see cref="RegexParser{TResult}"/>.
        /// </param>
        /// <param name="selector">
        ///   The selector that converts the <see cref="Match"/> to the result.
        /// </param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public RegexParser(Regex regex, Func<Match, TResult> selector)
        {
            if (regex == null || selector == null)
                throw new ArgumentNullException();
            Regex = regex;
            Selector = selector;
        }        

    }

    /// <summary>
    ///   Implements an <see cref="IParser{String, String}"/> that wraps a <see cref="Regex"/>
    ///   object. For a specified input string, the parser replaces all strings that match the 
    ///   specified regular expression with a specified replacement string, and returns the results.
    /// </summary>
    public class RegexParser : RegexParser<string>, IParser<string, string>
    {
        /// <summary>
        ///   Inintialize a new instance of the <see cref="RegexParser"/> class using a specified 
        ///   regular expression and a specified replacement string. 
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public RegexParser(string pattern, string replacement)
            : base(CheckPattern(pattern), CheckReplacement(replacement))
        {
            Replacement = replacement;
        }

        /// <summary>
        ///   Inintialize a new instance of the <see cref="RegexParser"/> class using a specified 
        ///   regular expression, a specified replacement string, and specified options that modify 
        ///   the matching operation.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="options">
        ///   A bitwise combination of the enumeration values that provide options for matching.
        /// </param>
        /// <param name="replacement">The replacement string.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public RegexParser(string pattern, RegexOptions options, string replacement)
            : base(CheckPattern(pattern, options), CheckReplacement(replacement))
        {
            Replacement = replacement;
        }

        /// <summary>
        /// Gets the replacement string.
        /// </summary>
        /// <value>The replacement string.</value>
        public string Replacement { get; private set; }

        private static Regex CheckPattern(string pattern, RegexOptions options = RegexOptions.None)
        {
            if (pattern == null)
                throw new ArgumentNullException();
            return new Regex(pattern, options);
        }
        private static Func<Match, string> CheckReplacement(string replacement)
        {
            if (replacement == null)
                throw new ArgumentNullException();
            return m => m.Result(replacement);
        }
    }
}
