/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : StartEndParser
 * Description : Implements an IParser that matches strings with the specified prefix and postfix.
 * Created     : 2016/1/21
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
    ///   Implements an <see cref="IParser{String, String}"/> that matches strings 
    ///   with the specified prefix and postfix.
    /// </summary>
    public class StartEndParser : IParser<string, string>
    {
        /// <summary>
        /// Gets the prefix of the string to match.
        /// </summary>
        /// <value>The prefix of the string to match.</value>
        public string Start { get; private set; }

        /// <summary>
        /// Gets the postfix of the string to match.
        /// </summary>
        /// <value>The postfix of the string to match.</value>
        public string End { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the match is case-sensitive.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the match is case-sensitive; otherwise, 
        ///   <see langword="false"/>.
        /// </value>
        public bool IgnoreCase { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether the result contains the prefix and 
        ///   postfix.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the match the result contains the prefix and
        ///   postfix; otherwise, <see langword="false"/>.
        /// </value>
        public bool IncludeStartEnd { get; private set; }


        /// <summary>
        ///   Inintialize a new instance of the <see cref="StartEndParser"/> class 
        ///   with the specified parameters.
        /// </summary>
        /// <param name="start">The prefix of the string to match.</param>
        /// <param name="end">The postfix of the string to match.</param>
        /// <param name="ignoreCase">Whether the match is case-sensitive.</param>
        /// <param name="includeStartEnd">
        ///   Whether the result contains the prefix and postfix.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public StartEndParser(string start, string end, bool ignoreCase = false, bool includeStartEnd = false)
        {
            if (start == null || end == null)
                throw new ArgumentNullException();
            Start = start;
            End = end;
            IgnoreCase = ignoreCase;
        }

        /// <inheritdoc/>
        public IEnumerable<string> Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            var comparison = IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            int startPosition = input.IndexOf(Start, 0 , comparison);
            while (true)
            {               
                if (startPosition < 0)
                    yield break;
                if (startPosition + Start.Length == input.Length)
                    yield break;
                int endPosition = input.IndexOf(End, startPosition + Start.Length, comparison);
                if (endPosition < 0)
                    yield break;
                int nextStartPosition = input.IndexOf(Start, startPosition + Start.Length, comparison);
                if (nextStartPosition < 0 || nextStartPosition + Start.Length > endPosition)
                {                   
                    if (IncludeStartEnd)
                        yield return input.Substring(startPosition, endPosition + End.Length - startPosition);
                    else
                        yield return input.Substring(startPosition + Start.Length, endPosition - startPosition - Start.Length);
                    if (endPosition + End.Length < input.Length)
                        startPosition = input.IndexOf(Start, endPosition + End.Length, comparison);
                    else
                        yield break;
                }
                else
                {
                    startPosition = nextStartPosition;
                }
            }
        }
    }
}
