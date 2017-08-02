/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Util
 * Description : Provides commonly used methods.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing
{
    static class Util
    {
        public static bool IsUppercaseAsciiLetter(this char c)
        {
            return 'A' <= c && c <= 'Z';
        }

        public static bool IsLowercaseAsciiLetter(this char c)
        {
            return 'a' <= c && c <= 'z';
        }

        public static bool IsUppercaseAsciiLetter(this char? c)
        {
            return c.HasValue && c.Value.IsUppercaseAsciiLetter();
        }

        public static bool IsLowercaseAsciiLetter(this char? c)
        {
            return c.HasValue && c.Value.IsLowercaseAsciiLetter();
        }


        public static bool IsOneOf<T>(this T value, params T[] values)
        {
            return values.Contains(value);
        }        
    }
}
