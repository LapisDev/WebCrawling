/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : LinkParser
 * Description : Implements a default ILinkParser.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lapis.WebCrawling.Spiders
{
    /// <summary>
    /// Implements a default <see cref="ILinkParser"/>.
    /// </summary>
    public class LinkParser : ILinkParser
    {
        /// <summary>
        /// Inintialize a new instance of the <see cref="LinkParser"/> class.
        /// </summary>
        public LinkParser()            
        { }

        /// <inheritdoc/>
        public IEnumerable<Uri> Parse(string html)
        {        
            List<Uri> links = new List<Uri>();
            if (html == null)
                return links;
            MatchCollection m = _regex.Matches(html);
            for (int i = 0; i < m.Count; i++)
            {
                var link = m[i].Groups[2].Value;
                if (string.IsNullOrWhiteSpace(link))
                    continue;
                int hash = link.IndexOf('#');
                if (hash == 0)
                    continue;
                if (link.Contains("<") ||
                    link.Contains(">") ||
                    link.Contains(" ") ||
                    link.Contains("\"") ||
                    link.Contains("\'") ||
                    link.Contains(" ") ||
                    link.Contains("(") ||
                    link.Contains(")") ||
                    link.Contains("{") ||
                    link.Contains("}") ||
                    link.Contains("~") ||
                    link.Contains("^"))
                    continue;
                if (hash > 0)
                    link = link.Substring(0, hash);
                links.Add(new Uri(link, UriKind.RelativeOrAbsolute));
            }
            return links;
        }


        Regex _regex = new Regex(Pattern, RegexOptions.IgnoreCase);

        private const string Pattern = @"<a\s+(\w+=""\w+""\s+)*href=""(.*?)""";
    }
}
