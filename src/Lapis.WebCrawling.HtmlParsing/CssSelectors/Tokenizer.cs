/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Tokenizer
 * Description : Represents a tokenizer for parsing css selectors.
 * Created     : 2016/1/7
 * Note        : http://www.w3.org/TR/css-syntax-3
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.CssSelectors
{
    /// <summary>
    /// Represents a tokenizer for parsing css selectors.
    /// </summary>
    public partial class Tokenizer
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="Tokenizer"/> class with the 
        ///   string to parse.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        public Tokenizer(string s)
        {
            _inputStream = new StringBuilder(s);
        }
        /// <summary>
        ///   Initializes a new instance of the <see cref="Tokenizer"/> class with the 
        ///   specified parameters.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="isStrictMode">
        ///   A value that indicates whether the <see cref="Tokenizer"/> throws an
        ///   exception when an error occurs.
        /// </param>
        public Tokenizer(string s, bool isStrictMode)
            : this(s)
        {
            IsStrictMode = isStrictMode;
        }

        /// <summary>
        /// Gets the next <see cref="Token"/> but doesn't consume it. 
        /// </summary>
        /// <value>The current <see cref="Token"/>. </value>
        public Token Current { get; private set; }

        /// <summary>
        /// Consumes the next <see cref="Token"/> and returns it.  
        /// </summary>
        /// <returns>The next <see cref="Token"/></returns>
        /// <exception cref="CssSelectorException">
        ///   This exception is thrown when an error occurs if 
        ///   <see cref="IsStrictMode"/> is set <see langword="true"/>.
        /// </exception>
        public Token Next()
        {
            return Current = ConsumeToken();
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="Tokenizer"/> throws an
        ///   exception when an error occurs.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the <see cref="Tokenizer"/> throws an
        ///   exception when an error occurs; otherwise, <see langword="true"/>.
        /// </value>
        public bool IsStrictMode { get; }

        private StringBuilder _inputStream;

        private int _currentCodePointPosition;

        private int _currentTokenPosition;

        private void ParseError()
        {
            string message;
            if (!IsEOFCodePoint())
                message = $"(Char {((int)_currentCodePoint).ToString("X4")}'{_currentCodePoint}')";
            else
                message = $"(EOF)";
            if (IsStrictMode)
                throw new CssSelectorException(_currentCodePointPosition,
                    ExceptionResource.TokenizerError + message);
            System.Diagnostics.Debug.WriteLine("Position " + _currentCodePointPosition + " " + message);
        }


        #region 3.3.  Preprocessing the input stream

        private string PreprocessInput(string input)
        {
            return input.Replace("\u000D\u000A", "\u000A")
                .Replace('\u000D', '\u000A')
                .Replace('\u000C', '\u000A')
                .Replace('\u0000', '\uFFFD');
        }

        #endregion


        #region 4.2.  Definitions

        private char ConsumeNextCodePoint()
        {
            if (_inputStream.Length > 0)
            {
                _currentCodePoint = _inputStream[0];
                _inputStream.Remove(0, 1);
                _currentCodePointPosition += 1;
            }
            else
            {
                _currentCodePoint = '\0';
            }
            return _currentCodePoint;
        }

        private char PeekNextCodePoint(int index = 1)
        {
            if (_inputStream.Length >= index)
            {
                return _inputStream[index - 1];
            }
            else
            {
                return '\0';
            }
        }

        private char _currentCodePoint;

        private void PushBackCurrentCodePoint()
        {
            _inputStream.Insert(0, _currentCodePoint.ToString());
            _currentCodePointPosition -= 1;
        }

        private bool IsEOFCodePoint()
        {
            return _inputStream.Length == 0;
        }

        private bool IsDigit(char c)
        {
            return c >= '\u0030' && c <= '\u0039';
        }

        private bool IsHexDigit(char c)
        {
            return IsDigit(c) ||
                c >= '\u0041' && c <= '\u0046' ||
                c >= '\u0061' && c <= '\u0066';
        }

        private bool IsUppercaseLetter(char c)
        {
            return c >= '\u0041' && c <= '\u005A';
        }

        private bool IsLowercaseLetter(char c)
        {
            return c >= '\u0061' && c <= '\u007A';
        }

        private bool IsLetter(char c)
        {
            return IsUppercaseLetter(c) || IsLowercaseLetter(c);
        }

        private bool IsNonAscii(char c)
        {
            return c >= '\u0080';
        }

        private bool IsNameStart(char c)
        {
            return IsLetter(c) || IsNonAscii(c) || c == '\u005F';
        }

        private bool IsName(char c)
        {
            return IsNameStart(c) || IsDigit(c) || c == '\u002D';
        }

        private bool IsNonPrintable(char c)
        {
            return c >= '\u0000' && c <= '\u000B';
        }

        private bool IsNewline(char c)
        {
            return c == '\u000A';
        }

        private bool IsWhitespace(char c)
        {
            return IsNewline(c) || c == '\u0009' || c == '\u0020';
        }

        private bool IsSurrogate(char c)
        {
            return c >= '\uD800' || c <= '\uDFFF';
        }

        private const char MaxAllowedCodePoint = '\uFFFF';


        #endregion


        #region 4.3.  Tokenizer Algorithms

        // 4.3.1.  Consume a token
        private Token ConsumeToken()
        {
            if (IsEOFCodePoint())
            {
                return new Token(_currentCodePointPosition, TokenType.EOF, null);
            }
            char c = ConsumeNextCodePoint();
            _currentTokenPosition = _currentCodePointPosition;
            if (IsWhitespace(c))
            {
                while (IsWhitespace(PeekNextCodePoint()))
                {
                    c = ConsumeNextCodePoint();
                }
                return new Token(_currentTokenPosition, TokenType.Whitespace, c.ToString());
            }
            if (c == '\u0022')
            {
                return ConsumeStringToken('\u0022');
            }
            if (c == '\u0023')
            {
                char next = PeekNextCodePoint();
                if (IsName(next) ||
                    IsValidEscape(next, PeekNextCodePoint(2)))
                {
                    bool isId = false;
                    if (CanStartIdentifier(PeekNextCodePoint(), PeekNextCodePoint(2), PeekNextCodePoint(3)))
                        isId = true;
                    var name = ConsumeName();
                    return new HashToken(_currentTokenPosition, name, isId);
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u0024')
            {
                if (PeekNextCodePoint() == '\u003D')
                {
                    ConsumeNextCodePoint();
                    return new Token(_currentTokenPosition, TokenType.SuffixMatch, c.ToString());
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u0027')
            {
                return ConsumeStringToken('\u0027');
            }
            if (c == '\u0028')
            {
                return new Token(_currentTokenPosition, TokenType.LeftParen, c.ToString());
            }
            if (c == '\u0029')
            {
                return new Token(_currentTokenPosition, TokenType.RightParen, c.ToString());
            }
            if (c == '\u002A')
            {
                if (PeekNextCodePoint() == '\u003D')
                {
                    ConsumeNextCodePoint();
                    return new Token(_currentTokenPosition, TokenType.SubstringMatch, c.ToString());
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u002B')
            {
                if (CanStartNumber(c, PeekNextCodePoint(), PeekNextCodePoint(2)))
                {
                    PushBackCurrentCodePoint();
                    return ConsumeNumericToken();
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u002C')
            {
                return new Token(_currentTokenPosition, TokenType.Comma, c.ToString());
            }
            if (c == '\u002D')
            {
                if (CanStartNumber(c, PeekNextCodePoint(), PeekNextCodePoint(2)))
                {
                    PushBackCurrentCodePoint();
                    return ConsumeNumericToken();
                }
                else if (CanStartIdentifier(c, PeekNextCodePoint(), PeekNextCodePoint(2)))
                {
                    PushBackCurrentCodePoint();
                    return ConsumeIdentLikeToken();
                }
                else if (PeekNextCodePoint() == '\u002D' &&
                    PeekNextCodePoint(2) == '\u003E')
                {
                    return new Token(_currentTokenPosition, TokenType.CDC,
                        ConsumeNextCodePoint().ToString() + ConsumeNextCodePoint());
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u002E')
            {
                if (CanStartNumber(c, PeekNextCodePoint(), PeekNextCodePoint(2)))
                {
                    PushBackCurrentCodePoint();
                    return ConsumeNumericToken();
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u002F')
            {
                if (PeekNextCodePoint() == '\u002A')
                {
                    ConsumeNextCodePoint();
                    while (true)
                    {
                        if (IsEOFCodePoint())
                            return ConsumeToken();
                        if (PeekNextCodePoint() == '\u002A' &&
                            PeekNextCodePoint(2) == '\u002F')
                        {
                            ConsumeNextCodePoint();
                            ConsumeNextCodePoint();
                            return ConsumeToken();
                        }
                        ConsumeNextCodePoint();
                    }
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u003A')
            {
                return new Token(_currentTokenPosition, TokenType.Colon, c.ToString());
            }
            if (c == '\u003B')
            {
                return new Token(_currentTokenPosition, TokenType.Semicolon, c.ToString());
            }
            if (c == '\u003C')
            {
                if (PeekNextCodePoint() == '\u0021' &&
                    PeekNextCodePoint(2) == '\u002D' &&
                    PeekNextCodePoint(3) == '\u002D')
                {
                    return new Token(_currentTokenPosition, TokenType.CDO,
                        ConsumeNextCodePoint().ToString() +
                        ConsumeNextCodePoint() + ConsumeNextCodePoint());
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u0040')
            {
                if (CanStartIdentifier(PeekNextCodePoint(), PeekNextCodePoint(2), PeekNextCodePoint(3)))
                {
                    var name = ConsumeName();
                    return new Token(_currentTokenPosition, TokenType.AtKeyword, name);
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u005B')
            {
                return new Token(_currentTokenPosition, TokenType.LeftSquare, c.ToString());
            }
            if (c == '\u005C')
            {
                if (IsValidEscape(c, PeekNextCodePoint()))
                {
                    PushBackCurrentCodePoint();
                    return ConsumeIdentLikeToken();
                }
                else
                {
                    ParseError();
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u005D')
            {
                return new Token(_currentTokenPosition, TokenType.RightSquare, c.ToString());
            }
            if (c == '\u005E')
            {
                if (PeekNextCodePoint() == '\u003D')
                    return new Token(_currentTokenPosition, TokenType.PrefixMatch,
                        c.ToString() + ConsumeNextCodePoint());
                else
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
            }
            if (c == '\u007B')
            {
                return new Token(_currentTokenPosition, TokenType.LeftCurlyBracket, c.ToString());
            }
            if (c == '\u007D')
            {
                return new Token(_currentTokenPosition, TokenType.RightCurlyBracket, c.ToString());
            }
            if (IsDigit(c))
            {
                PushBackCurrentCodePoint();
                return ConsumeNumericToken();
            }
            if (c.IsOneOf('\u0055', '\u0075'))
            {
                if (PeekNextCodePoint() == '\u002B' &&
                    (IsHexDigit(PeekNextCodePoint(2)) || PeekNextCodePoint(2) == '\u003F'))
                {
                    ConsumeNextCodePoint();
                    return ConsumeUnicodeRangeToken();
                }
                else
                {
                    PushBackCurrentCodePoint();
                    return ConsumeIdentLikeToken();
                }
            }
            if (IsNameStart(c))
            {
                PushBackCurrentCodePoint();
                return ConsumeIdentLikeToken();
            }
            if (c == '\u007C')
            {
                if (PeekNextCodePoint() == '\u003D')
                {
                    return new Token(_currentTokenPosition, TokenType.DashMatch, c.ToString() + ConsumeNextCodePoint());
                }
                else if (PeekNextCodePoint() == '\u0073')
                {
                    return new Token(_currentTokenPosition, TokenType.Column, c.ToString() + ConsumeNextCodePoint());
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            if (c == '\u007E')
            {
                if (PeekNextCodePoint() == '\u003D')
                {
                    return new Token(_currentTokenPosition, TokenType.IncludeMatch, c.ToString() + ConsumeNextCodePoint());
                }
                else
                {
                    return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
                }
            }
            return new Token(_currentTokenPosition, TokenType.Delim, c.ToString());
        }

        // 4.3.2.  Consume a numeric token
        private Token ConsumeNumericToken()
        {
            bool isInteger;
            string number = ConsumeNumber(out isInteger);
            if (CanStartIdentifier(PeekNextCodePoint(), PeekNextCodePoint(2), PeekNextCodePoint(3)))
            {
                var unit = ConsumeName();
                return new DimensionToken(_currentTokenPosition, number, unit, isInteger);
            }
            else if (PeekNextCodePoint() == '\u0025')
            {
                ConsumeNextCodePoint();
                return new NumberToken(_currentTokenPosition, TokenType.Percentage, number, isInteger);
            }
            else
            {
                return new NumberToken(_currentTokenPosition, number, isInteger);
            }
        }

        // 4.3.3.  Consume an ident-like token
        private Token ConsumeIdentLikeToken()
        {
            var name = ConsumeName();
            if (name.Equals("url", StringComparison.CurrentCultureIgnoreCase) &&
                PeekNextCodePoint() == '\u0028')
            {
                ConsumeNextCodePoint();
                return ConsumeUrlToken();
            }
            else if (PeekNextCodePoint() == '\u0028')
            {
                ConsumeNextCodePoint();
                return new Token(_currentTokenPosition, TokenType.Function, name);
            }
            else
            {
                return new Token(_currentTokenPosition, TokenType.Ident, name);
            }
        }

        // 4.3.4.  Consume a string token
        private Token ConsumeStringToken(char end)
        {
            var sb = new StringBuilder();
            char c;
            while (true)
            {
                if (IsEOFCodePoint())
                {
                    return new Token(_currentTokenPosition, TokenType.String, sb.ToString());
                }
                c = ConsumeNextCodePoint();
                if (c == end)
                {
                    return new Token(_currentTokenPosition, TokenType.String, sb.ToString());
                }
                if (IsNewline(c))
                {
                    ParseError();
                    PushBackCurrentCodePoint();
                    return new Token(_currentTokenPosition, TokenType.BadString, sb.ToString());
                }
                if (c == '\u005C')
                {
                    if (IsEOFCodePoint())
                    {
                    }
                    else if (IsNewline(PeekNextCodePoint()))
                    {
                        ConsumeNextCodePoint();
                    }
                    else if (IsValidEscape(c, PeekNextCodePoint()))
                    {
                        sb.Append(ConsumeEscapedCodePoint());
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
        }

        // 4.3.5.  Consume a url token 
        private Token ConsumeUrlToken()
        {
            var sb = new StringBuilder();
            while (IsWhitespace(PeekNextCodePoint()))
                ConsumeNextCodePoint();
            if (IsEOFCodePoint())
            {
                return new Token(_currentTokenPosition, TokenType.Url, sb.ToString());
            }
            if (PeekNextCodePoint().IsOneOf('\u0022', '\u0027'))
            {
                var stringToken = ConsumeStringToken(PeekNextCodePoint());
                if (stringToken.Type == TokenType.BadString)
                {
                    var remnants = ConsumeRemnantsOfBadUrl();
                    return new Token(_currentTokenPosition, TokenType.BadUrl, stringToken.Value);
                }
                sb.Append(stringToken.Value);
                while (IsWhitespace(PeekNextCodePoint()))
                    ConsumeNextCodePoint();
                if (IsEOFCodePoint())
                    return new Token(_currentTokenPosition, TokenType.Url, stringToken.Value);
                else if (PeekNextCodePoint() == '\u0029')
                {
                    ConsumeNextCodePoint();
                    return new Token(_currentTokenPosition, TokenType.Url, stringToken.Value);
                }
                else
                {
                    var remnants = ConsumeRemnantsOfBadUrl();
                    return new Token(_currentTokenPosition, TokenType.BadUrl, stringToken.Value);
                }
            }
            char c;
            while (true)
            {
                if (IsEOFCodePoint())
                    return new Token(_currentTokenPosition, TokenType.Url, sb.ToString());
                c = ConsumeNextCodePoint();
                if (IsWhitespace(c))
                {
                    while (IsWhitespace(PeekNextCodePoint()))
                        ConsumeNextCodePoint();
                    if (IsEOFCodePoint())
                        return new Token(_currentTokenPosition, TokenType.Url, sb.ToString());
                    else if (PeekNextCodePoint() == '\u0029')
                    {
                        ConsumeNextCodePoint();
                        return new Token(_currentTokenPosition, TokenType.Url, sb.ToString());
                    }
                    else
                    {
                        var remnants = ConsumeRemnantsOfBadUrl();
                        return new Token(_currentTokenPosition, TokenType.BadUrl, sb.ToString());
                    }
                }
                if (c.IsOneOf('\u0022', '\u0027', '\u0028') ||
                    IsNonPrintable(c))
                {
                    ParseError();
                    var remnants = ConsumeRemnantsOfBadUrl();
                    return new Token(_currentTokenPosition, TokenType.BadUrl, sb.ToString());
                }
                if (c == '\u005C')
                {
                    if (IsValidEscape(c, PeekNextCodePoint()))
                    {
                        sb.Append(ConsumeEscapedCodePoint());
                    }
                    else
                    {
                        ParseError();
                        var remnants = ConsumeRemnantsOfBadUrl();
                        return new Token(_currentTokenPosition, TokenType.BadUrl, sb.ToString());
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
        }

        // 4.3.6.  Consume a unicode-range token
        private Token ConsumeUnicodeRangeToken()
        {
            var sb = new StringBuilder();
            bool hasQuestionMark = false;
            int i = 0;
            for (; i < 6; i++)
            {
                if (IsHexDigit(PeekNextCodePoint()))
                {
                    sb.Append(ConsumeNextCodePoint());
                }
                else
                {
                    break;
                }
            }
            for (; i < 6; i++)
            {
                if (PeekNextCodePoint() == '\u003F')
                {
                    sb.Append(ConsumeNextCodePoint());
                    if (!hasQuestionMark)
                        hasQuestionMark = true;
                }
                else
                {
                    break;
                }
            }
            int start, end;
            if (hasQuestionMark)
            {
                start = int.Parse(sb.ToString().Replace('\u003F', '\u0030'), System.Globalization.NumberStyles.HexNumber);
                end = int.Parse(sb.ToString().Replace('\u003F', '\u0046'), System.Globalization.NumberStyles.HexNumber);
                return new UnicodeRangeToken(_currentTokenPosition, start, end);
            }
            else
            {
                start = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber);
            }
            if (PeekNextCodePoint() == '\u002D' &&
                IsHexDigit(PeekNextCodePoint(2)))
            {
                sb.Clear();
                i = 0;
                ConsumeNextCodePoint();
                for (; i < 6; i++)
                {
                    if (IsHexDigit(PeekNextCodePoint()))
                    {
                        sb.Append(ConsumeNextCodePoint());
                    }
                    else
                    {
                        break;
                    }
                }
                end = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                end = start;
            }
            return new UnicodeRangeToken(_currentTokenPosition, start, end);
        }

        // 4.3.7.  Consume an escaped code point
        private char ConsumeEscapedCodePoint()
        {
            if (IsEOFCodePoint())
            {
                return '\uFFFD';
            }
            var c = ConsumeNextCodePoint();
            if (IsHexDigit(c))
            {
                var sb = new StringBuilder();
                sb.Append(c);
                for (int i = 0; i < 5; i++)
                {
                    c = PeekNextCodePoint();
                    if (IsHexDigit(c))
                    {
                        ConsumeNextCodePoint();
                        sb.Append(c);
                    }
                    else
                    {
                        break;
                    }
                }
                c = PeekNextCodePoint();
                if (IsWhitespace(c))
                {
                    ConsumeNextCodePoint();
                }
                int number = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber);
                if (number == 0 || number > MaxAllowedCodePoint || IsSurrogate((char)number))
                {
                    return '\uFFFD';
                }
                else
                {
                    return (char)number;
                }
            }
            else
            {
                return c;
            }
        }

        // 4.3.8.  Check if two code points are a valid escape
        private bool IsValidEscape(char c1, char c2)
        {
            if (c1 != '\u005C')
                return false;
            else if (IsNewline(c2))
                return false;
            else
                return true;
        }

        // 4.3.9.  Check if three code points would start an identifier
        private bool CanStartIdentifier(char c1, char c2, char c3)
        {
            if (c1 == '\u002D')
                if (IsNameStart(c2) || IsValidEscape(c2, c3))
                    return true;
                else
                    return false;
            else if (IsNameStart(c1))
                return true;
            else if (c1 == '\u005C')
                if (IsValidEscape(c1, c2))
                    return true;
                else
                    return false;
            else
                return false;
        }

        // 4.3.10.  Check if three code points would start a number
        private bool CanStartNumber(char c1, char c2, char c3)
        {
            if (c1 != '\u002B' || c1 == '\u002D')
                if (IsDigit(c2))
                    return true;
                else if (c2 == '\u002E' && IsDigit(c3))
                    return true;
                else
                    return false;
            else if (c1 == '\u002E')
                if (IsDigit(c2))
                    return true;
                else
                    return false;
            else if (IsDigit(c1))
                return true;
            else
                return false;
        }

        // 4.3.11.  Consume a name
        private string ConsumeName()
        {
            var sb = new StringBuilder();
            char c;
            while (true)
            {
                if (IsEOFCodePoint())
                {
                    return sb.ToString();
                }
                c = ConsumeNextCodePoint();
                if (IsName(c))
                {
                    sb.Append(c);
                }
                else if (IsValidEscape(c, PeekNextCodePoint()))
                {
                    sb.Append(ConsumeEscapedCodePoint());
                }
                else
                {
                    PushBackCurrentCodePoint();
                    return sb.ToString();
                }
            }
        }

        // 4.3.12.  Consume a number
        private string ConsumeNumber(out bool isInteger)
        {
            var sb = new StringBuilder();
            isInteger = true;
            if (PeekNextCodePoint().IsOneOf('\u002B', '\u002D'))
            {
                sb.Append(ConsumeNextCodePoint());
            }
            while (IsDigit(PeekNextCodePoint()))
            {
                sb.Append(ConsumeNextCodePoint());
            }
            if (PeekNextCodePoint() == '\u002E' && IsDigit(PeekNextCodePoint(2)))
            {
                sb.Append(ConsumeNextCodePoint())
                    .Append(ConsumeNextCodePoint());
                isInteger = false;
                while (IsDigit(PeekNextCodePoint()))
                {
                    sb.Append(ConsumeNextCodePoint());
                }
            }
            if (PeekNextCodePoint(2).IsOneOf('\u0045', '\u0065'))
            {
                if (IsDigit(PeekNextCodePoint(3)))
                {
                    sb.Append(ConsumeNextCodePoint());
                }
                else if (PeekNextCodePoint(3).IsOneOf('\u002D', '\u002B') &&
                    IsDigit(PeekNextCodePoint(4)))
                {
                    sb.Append(ConsumeNextCodePoint())
                        .Append(ConsumeNextCodePoint());
                }
                isInteger = false;
                while (IsDigit(PeekNextCodePoint()))
                {
                    sb.Append(ConsumeNextCodePoint());
                }
            }
            return sb.ToString();
        }

        // 4.3.13.  Convert a string to a number

        // 4.3.14.  Consume the remnants of a bad url
        private string ConsumeRemnantsOfBadUrl()
        {
            var sb = new StringBuilder();
            char c;
            while (true)
            {
                if (IsEOFCodePoint())
                    return sb.ToString();
                c = ConsumeNextCodePoint();
                if (c == '\u0029')
                    return sb.ToString();
                if (IsValidEscape(c, PeekNextCodePoint()))
                    ConsumeEscapedCodePoint();
            }
        }

        #endregion
    }
}
