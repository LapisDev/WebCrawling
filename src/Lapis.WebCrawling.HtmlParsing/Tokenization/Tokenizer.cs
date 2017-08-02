/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Tokenizer
 * Description : Represents a tokenizer for parsing HTML.
 * Created     : 2015/8/24
 * Note        : 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.Tokenization
{
    /// <summary>
    /// Represents a tokenizer for parsing HTML.
    /// </summary>
    public partial class Tokenizer
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="Tokenizer"/> class with 
        ///   the specified parameters.
        /// </summary>
        /// <param name="html">The string to parse.</param>
        public Tokenizer(string html)
        {
            _stringBuilder = new StringBuilder(html);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Tokenizer"/> class with 
        ///   the specified parameters.
        /// </summary>
        /// <param name="html">The string to parse.</param>
        /// <param name="isStrictMode">
        ///   A value that indicates whether the <see cref="Tokenizer"/> throws an
        ///   exception when an error occurs.
        /// </param>
        public Tokenizer(string html, bool isStrictMode)
            : this(html)
        {
            IsStrictMode = isStrictMode;
        }

        /// <summary>
        /// Returns the next <see cref="Token"/> but doesn't consume it. 
        /// </summary>
        /// <returns>The current <see cref="Token"/>. </returns>
        /// <exception cref="HtmlException">
        ///   This exception is thrown when an error occurs if 
        ///   <see cref="IsStrictMode"/> is set <see langword="true"/>.
        /// </exception>
        public Token Peek()
        {
            while (_tokenQueue.Count == 0)
                ApplyRule();
            return _tokenQueue.Peek();
        }

        /// <summary>
        /// Consumes the next <see cref="Token"/> and returns it.  
        /// </summary>
        /// <returns>The next <see cref="Token"/></returns>
        /// <exception cref="HtmlException">
        ///   This exception is thrown when an error occurs if 
        ///   <see cref="IsStrictMode"/> is set <see langword="true"/>.
        /// </exception>
        public Token Read()
        {
            while (_tokenQueue.Count == 0)
                ApplyRule();
            return _tokenQueue.Dequeue();
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


        private Queue<Token> _tokenQueue = new Queue<Token>();


        private void EmitToken(Token token)
        {
            var startToken = token as StartTagToken;
            if (startToken != null)
                LastEmittedStartTagToken = startToken;
            else
            {
                var endToken = token as EndTagToken;
                if (endToken != null)
                {
                    if (endToken.Attributes.Count > 0)
                        ParseError(endToken.Location, $"End tag token with attributes.");
                    if (endToken.IsSelfClosing)
                        ParseError(endToken.Location, $"End tag token with self closing flag.");
                }
            }
            _tokenQueue.Enqueue(token);
        }


        private void ParseError(char? c)
        {
            string message;
            if (c != null)
                message = $"Char {((int)c).ToString("X4")} in {State} state.";
            else
                message = $"EOF in {State} state. ";
            if (IsStrictMode)
                throw new HtmlException(new Location(Line, Span),
                    ExceptionResource.TokenizerError + message);
            else
                System.Diagnostics.Debug.WriteLine(new Location(Line, Span) + ": " + message);
        }
        private void ParseError(Location location, string message)
        {
            if (IsStrictMode)
                throw new HtmlException(location,
                    ExceptionResource.TokenizerError + message);
            else
                System.Diagnostics.Debug.WriteLine(location + ": " + message);
        }


        private StringBuilder TemporaryBuffer { get; } = new StringBuilder();

        private CommentToken CurrentCommentToken { get; set; }

        private DOCTYPEToken CurrentDOCTYPEToken { get; set; }

        private TagToken CurrentTagToken { get; set; }

        private StartTagToken LastEmittedStartTagToken { get; set; }

        private bool IsAppropriateEndTagToken(TagToken tag)
        {
            return LastEmittedStartTagToken != null && LastEmittedStartTagToken.Name == tag.Name;
        }


        #region Charactor Inputing

        private int Line { get; set; } = 1;

        private int Span { get; set; } = 0;

        private StringBuilder _stringBuilder;

        private char? ReadNextChar()
        {
            if (_stringBuilder.Length == 0)
            {
                return null;
            }
            else
            {
                char c = _stringBuilder[0];
                _stringBuilder.Remove(0, 1);
                if (c == '\n')
                {
                    Line++;
                    Span = 0;
                }
                else
                {
                    Span++;
                }
                return c;
            }
        }

        private char? PeekNextChar()
        {
            if (_stringBuilder.Length == 0)
            {
                return null;
            }
            else
            {
                char c = _stringBuilder[0];
                return c;
            }
        }

        private char? PeekNextChar(int count)
        {
            if (_stringBuilder.Length < count)
            {
                return null;
            }
            else
            {
                char c = _stringBuilder[count - 1];
                return c;
            }
        }

        private string PeekNextChars(int count)
        {
            if (_stringBuilder.Length < count)
                count = _stringBuilder.Length;
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(_stringBuilder[i]);
            }
            return sb.ToString();
        }

        private string ReadNextChars(int count)
        {
            if (_stringBuilder.Length < count)
                count = _stringBuilder.Length;
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                char c = _stringBuilder[0];
                if (c == '\n')
                {
                    Line++;
                    Span = 0;
                }
                else
                {
                    Span++;
                }
                sb.Append(c);
                _stringBuilder.Remove(0, 1);
            }
            return sb.ToString();
        }

        private static readonly char? EOF = null;

        #endregion
    }
}
