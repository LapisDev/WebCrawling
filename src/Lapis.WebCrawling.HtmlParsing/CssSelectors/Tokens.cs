/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Token
 * Description : Represents a token for parsing css selectors.
 * Created     : 2016/1/4
 * Note        : http://www.w3.org/TR/css-syntax-3
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.CssSelectors
{
    /// <summary>
    /// Represents a token for parsing css selectors.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Type}: {Value}")]
    public class Token
    {
        /// <summary>
        /// Gets the text in the token.
        /// </summary>
        /// <value>The text in the token.</value>
        public string Value { get; }

        /// <summary>
        /// Gets the position of the token.
        /// </summary>
        /// <value>The position of the token.</value>
        public int Position { get; }

        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        /// <value>The type of the token.</value>
        public TokenType Type { get; }
        

        internal Token(int position, TokenType type, string value)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            Position = position;
            Type = type;
            Value = value;
        }
    }

    /// <summary>
    /// Represents a Unicode range token.
    /// </summary>
    public class UnicodeRangeToken : Token
    {
        /// <summary>
        /// Gets the start of the Unicode range.
        /// </summary>
        /// <value>The start of the Unicode range.</value>
        public int Start { get; }

        /// <summary>
        /// Gets the end of the Unicode range.
        /// </summary>
        /// <value>The end of the Unicode range.</value>
        public int End { get; }

        internal UnicodeRangeToken(int position, int start, int end)
           : base(position, TokenType.UnicodeRange, $"U+{start.ToString("X4")}+U+{end.ToString("X4")}")
        {
            if (start < 0 || start > 0xFFFF)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (end < 0 || end > 0xFFFF)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (start > end)
                throw new ArgumentException($"{nameof(start)} > {nameof(end)}");
            Start = start;
            End = end;
        }
    }

    /// <summary>
    /// Represents a hash token.
    /// </summary>
    public class HashToken : Token
    {
        /// <summary>
        /// Gets a value indicating whether the type flag is "id".
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the type flag is "id"; otherwise, 
        ///   <see langword="false"/>.
        /// </value>
        public bool IsId { get; }

        internal HashToken(int position, string value, bool isId = false)
           : base(position, TokenType.Hash, value)
        {
            IsId = isId;
        }
    }

    /// <summary>
    /// Represents a number token.
    /// </summary>
    public class NumberToken : Token
    {
        /// <summary>
        /// Gets a value indicating whether the type flag is "integer".
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the type flag is "integer"; 
        ///   otherwise, <see langword="false"/>.
        /// </value>
        public bool IsInteger { get; } = true;

        internal NumberToken(int position, string value, bool isInteger = true)
            : this(position, TokenType.Number, value, isInteger)
        {
        }

        internal NumberToken(int position, TokenType type, string value, bool isInteger = true)
           : base(position, type, value)
        {
            IsInteger = isInteger;
        }
    }

    /// <summary>
    /// Represents a dimension token.
    /// </summary>
    public class DimensionToken : NumberToken
    {
        /// <summary>
        /// Gets the unit of the token.
        /// </summary>
        /// <value>The unit of the token.</value>
        public string Unit { get; }

        internal DimensionToken(int position, string value, string unit, bool isInteger = true)
            : base(position, TokenType.Dimension, value, isInteger)
        {
            Unit = unit;
        }
    }


    /// <summary>
    /// Represents the type of a token.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The value indicates that the token is a ident token.
        /// </summary>
        Ident,
        /// <summary>
        /// The value indicates that the token is a function token.
        /// </summary>
        Function,
        /// <summary>
        /// The value indicates that the token is a at-keyword token.
        /// </summary>
        AtKeyword,
        /// <summary>
        /// The value indicates that the token is a hash token.
        /// </summary>
        Hash,
        /// <summary>
        /// The value indicates that the token is a string token.
        /// </summary>
        String,
        /// <summary>
        /// The value indicates that the token is a bad-string token.
        /// </summary>
        BadString,
        /// <summary>
        /// The value indicates that the token is a url token.
        /// </summary>
        Url,
        /// <summary>
        /// The value indicates that the token is a bad-url token.
        /// </summary>
        BadUrl,
        /// <summary>
        /// The value indicates that the token is a delim token.
        /// </summary>
        Delim,
        /// <summary>
        /// The value indicates that the token is a number token.
        /// </summary>
        Number,
        /// <summary>
        /// The value indicates that the token is a percentage token.
        /// </summary>
        Percentage,
        /// <summary>
        /// The value indicates that the token is a dimension token.
        /// </summary>
        Dimension,
        /// <summary>
        /// The value indicates that the token is a unicode-range token.
        /// </summary>
        UnicodeRange,
        /// <summary>
        /// The value indicates that the token is a include-match token.
        /// </summary>
        IncludeMatch,
        /// <summary>
        /// The value indicates that the token is a dash-match token.
        /// </summary>
        DashMatch,
        /// <summary>
        /// The value indicates that the token is a prefix-match token.
        /// </summary>
        PrefixMatch,
        /// <summary>
        /// The value indicates that the token is a suffix-match token.
        /// </summary>
        SuffixMatch,
        /// <summary>
        /// The value indicates that the token is a substring-match token.
        /// </summary>
        SubstringMatch,
        /// <summary>
        /// The value indicates that the token is a column token.
        /// </summary>
        Column,
        /// <summary>
        /// The value indicates that the token is a whitespace token.
        /// </summary>
        Whitespace,
        /// <summary>
        /// The value indicates that the token is a CDO token.
        /// </summary>
        CDO,
        /// <summary>
        /// The value indicates that the token is a CDC token.
        /// </summary>
        CDC,
        /// <summary>
        /// The value indicates that the token is a colon token.
        /// </summary>
        Colon,
        /// <summary>
        /// The value indicates that the token is a semicolon token.
        /// </summary>
        Semicolon,
        /// <summary>
        /// The value indicates that the token is a comma token.
        /// </summary>
        Comma,
        /// <summary>
        /// The value indicates that the token is a [ token.
        /// </summary>
        LeftSquare,
        /// <summary>
        /// The value indicates that the token is a ] token.
        /// </summary>
        RightSquare,
        /// <summary>
        /// The value indicates that the token is a ( token.
        /// </summary>
        LeftParen,
        /// <summary>
        /// The value indicates that the token is a ) token.
        /// </summary>
        RightParen,
        /// <summary>
        /// The value indicates that the token is a { token.
        /// </summary>
        LeftCurlyBracket,
        /// <summary>
        /// The value indicates that the token is a } token.
        /// </summary>
        RightCurlyBracket,
        /// <summary>
        /// The value indicates that the token is a EOF token.
        /// </summary>
        EOF
    }
}
