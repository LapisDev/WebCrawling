/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Parser
 * Description : Provides methods for parsing a css selector.
 * Created     : 2016/1/7
 * Note        : http://www.w3.org/TR/selectors/#selector-syntax
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.CssSelectors
{
    /// <summary>
    /// Provides methods for parsing a css selector.
    /// </summary>
    public partial class Parser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="Parser"/> class with the 
        ///   string to parse.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        public Parser(string s)
        {
            _tokenizer = new Tokenizer(s);
        }

        /// <summary>
        /// Returns the parsing result.
        /// </summary>
        /// <returns>The css selector.</returns>
        /// <exception cref="CssSelectorException">
        ///   An error occurs when parsing.
        /// </exception>
        public CssSelector Parse()
        {
            _tokenizer.Next();
            ConsumeWhitespace();
            return ParseSelectorGroup();
        }


        private Tokenizer _tokenizer;

        private void ParseError()
        {
            string message;
            message = $"({_tokenizer.Current.Type} Token {_tokenizer.Current.Value})";
            throw new CssSelectorException(_tokenizer.Current.Position,
                ExceptionResource.TokenizerError + message);
        }

        private void ParseErrorExpected(TokenType type, string value)
        {
            string message;
            message = $"{type} Token {value} expected.";
            throw new CssSelectorException(_tokenizer.Current.Position,
                ExceptionResource.TokenizerError + message);
        }


        // 5. Groups of selectors
        private CssSelector ParseSelectorGroup()
        {
            var list = new List<CssSelector>();
            list.Add(ParseSelector());
            Token t;
            while (true)
            {
                ConsumeWhitespace();
                t = _tokenizer.Current;
                if (t.Type == TokenType.EOF)
                {
                    t = _tokenizer.Next();
                    break;
                }
                else if (t.Type == TokenType.Comma)
                {
                    _tokenizer.Next();
                    list.Add(ParseSelector());
                }
                else
                {
                    ParseError();
                }
            }
            return list.Count > 1 ? new SelectorGroup(list) : list[0];
        }

        private CssSelector ParseSelector()
        {
            var selector = ParseCombinator();
            if (selector != null)
            {
                return selector;
            }
            else
            {
                ParseError();
                return null;
            }
        }

        #region 6.  Simple selectors

        private CssSelector ParseSimpleSelector()
        {
            CssSelector selector;
            if ((selector = ParseIdSelector()) != null ||
                (selector = ParseClassSelector()) != null ||
                (selector = ParseAttributeSelector()) != null ||
                (selector = ParsePseudoClassSelector()) != null)
                return selector;
            else
                return null;
        }
        private CssSelector ParseSimpleSelectorSequence()
        {
            var list = new List<CssSelector>();
            CssSelector selector;
            if ((selector = ParseTypeSelector()) != null ||
                (selector = ParseUniversalSelector()) != null)
            {
                list.Add(selector);
            }
            while ((selector = ParseSimpleSelector()) != null)
            {
                list.Add(selector);
            }
            if (list.Count == 0)
            {
                ParseError();
                return null;
            }
            else if (list.Count == 1)
            {
                return list[0];
            }
            else
            {
                return new SelectorSequence(list);
            }
        }

        // 6.1. Type selector
        // Namespaces are not supported yet.
        private CssSelector ParseTypeSelector()
        {
            Token t = _tokenizer.Current;
            if (t.Type == TokenType.Ident)
            {
                _tokenizer.Next();
                return new TypeSelector(t.Value);
            }
            else
            {
                return null;
            }
        }

        // 6.2. Universal selector
        private CssSelector ParseUniversalSelector()
        {
            Token t = _tokenizer.Current;
            if (t.Type == TokenType.Delim && t.Value == "*")
            {
                _tokenizer.Next();
                return UniversalSelector.Instance;
            }
            else
            {
                return null;
            }
        }

        // 6.3. Attribute selectors
        private CssSelector ParseAttributeSelector()
        {
            Token t = _tokenizer.Current;
            if (t.Type == TokenType.LeftSquare)
            {
                _tokenizer.Next();
                ConsumeWhitespace();
                t = _tokenizer.Current;
                if (t.Type == TokenType.Ident)
                {
                    string att = t.Value;
                    _tokenizer.Next();
                    ConsumeWhitespace();
                    t = _tokenizer.Current;
                    if (t.Type == TokenType.RightSquare)
                    {
                        _tokenizer.Next();
                        return new AttributeSelector(att);
                    }
                    AttributeSelectorType type;
                    if (t.Type == TokenType.Delim && t.Value == "=")
                    {
                        _tokenizer.Next();
                        type = AttributeSelectorType.Equal;
                    }
                    else if (t.Type == TokenType.IncludeMatch)
                    {
                        _tokenizer.Next();
                        type = AttributeSelectorType.ContainWord;
                    }
                    else if (t.Type == TokenType.DashMatch)
                    {
                        _tokenizer.Next();
                        type = AttributeSelectorType.StartWithWord;
                    }
                    else if (t.Type == TokenType.PrefixMatch)
                    {
                        _tokenizer.Next();
                        type = AttributeSelectorType.StartWith;
                    }
                    else if (t.Type == TokenType.SuffixMatch)
                    {
                        _tokenizer.Next();
                        type = AttributeSelectorType.EndWith;
                    }
                    else if (t.Type == TokenType.SubstringMatch)
                    {
                        _tokenizer.Next();
                        type = AttributeSelectorType.Contain;
                    }
                    else
                    {
                        ParseError();
                        return null;
                    }
                    ConsumeWhitespace();
                    t = _tokenizer.Current;
                    if (t.Type == TokenType.Ident || t.Type == TokenType.String)
                    {
                        string val = t.Value;
                        _tokenizer.Next();
                        ConsumeWhitespace();
                        t = _tokenizer.Current;
                        if (t.Type == TokenType.RightSquare)
                        {
                            _tokenizer.Next();
                            return new AttributeSelector(att, val, type);
                        }
                        else
                        {
                            ParseErrorExpected(TokenType.RightSquare, "]");
                            return null;
                        }
                    }
                    else
                    {
                        ParseErrorExpected(TokenType.String, "");
                        return null;
                    }
                }
                else
                {
                    ParseErrorExpected(TokenType.Ident, "");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // 6.4. Class selectors
        private CssSelector ParseClassSelector()
        {
            Token t = _tokenizer.Current;
            if (t.Type == TokenType.Delim && t.Value == ".")
            {
                t = _tokenizer.Next();
                if (t.Type == TokenType.Ident)
                {
                    _tokenizer.Next();
                    return new ClassSelector(t.Value);
                }
                else
                {
                    ParseErrorExpected(TokenType.Ident, "");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // 6.5. ID selectors
        private CssSelector ParseIdSelector()
        {
            Token t = _tokenizer.Current;
            if (t.Type == TokenType.Hash)
            {
                _tokenizer.Next();
                return new IdSelector(t.Value);
            }
            else
            {
                return null;
            }
        }

        // 6.6. Pseudo-classes
        private CssSelector ParsePseudoClassSelector()
        {
            Token t = _tokenizer.Current;
            if (t.Type == TokenType.Colon)
            {
                t = _tokenizer.Next();
                if (t.Type == TokenType.Ident)
                    switch (t.Value)
                    {
                        case "first-child":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.FirstChild);
                        case "last-child":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.LastChild);
                        case "only-child":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.OnlyChild);
                        case "first-of-type":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.FirstOfType);
                        case "last-of-type":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.LastOfType);
                        case "only-of-type":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.OnlyOfType);
                        case "root":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.Root);
                        case "empty":
                            _tokenizer.Next();
                            return new PseudoClassSelector(PseudoClassSelectorType.Empty);
                        default:
                            ParseError(); return null;
                    }
                else if (t.Type == TokenType.Function)
                    switch (t.Value)
                    {
                        case "nth-child":
                            _tokenizer.Next();
                            return ParseNthSelector(PseudoClassNthSelectorType.NthChild);
                        case "nth-last-child":
                            _tokenizer.Next();
                            return ParseNthSelector(PseudoClassNthSelectorType.NthLastChild);
                        case "nth-of-type":
                            _tokenizer.Next();
                            return ParseNthSelector(PseudoClassNthSelectorType.NthOfType);
                        case "nth-last-of-type":
                            _tokenizer.Next();
                            return ParseNthSelector(PseudoClassNthSelectorType.NthLastOfType);
                        case "not":
                            _tokenizer.Next();
                            ConsumeWhitespace();
                            CssSelector selector = null;
                            if ((selector = ParseTypeSelector()) != null || 
                                (selector = ParseSimpleSelector()) != null)
                            {
                                ConsumeWhitespace();
                                if (_tokenizer.Current.Type == TokenType.RightParen)
                                {
                                    _tokenizer.Next();
                                    return new PseudoClassNegationSelector(selector);
                                }
                                else
                                {
                                    ParseErrorExpected(TokenType.RightParen, ")");
                                    return null;
                                }
                            }
                            else
                            {
                                ParseError();
                                return null;
                            }
                        default:
                            ParseError(); return null;
                    }
                else
                {
                    ParseError();
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private CssSelector ParseNthSelector(PseudoClassNthSelectorType type)
        {
            ConsumeWhitespace();
            int a, b;
            Token t = _tokenizer.Current;
            switch (t.Type)
            {
                case TokenType.Number:
                    var number = t as NumberToken;
                    if (!number.IsInteger)
                    {
                        ParseError();
                        return null;
                    }
                    _tokenizer.Next();
                    ConsumeWhitespace();
                    if (_tokenizer.Current.Type == TokenType.RightParen)
                    {
                        _tokenizer.Next();
                        return new PseudoClassNthSelector(type, int.Parse(number.Value));
                    }
                    else
                    {
                        ParseErrorExpected(TokenType.RightParen, ")");
                        return null;
                    }
                case TokenType.Dimension:
                    var dimen = t as DimensionToken;
                    if (!dimen.IsInteger)
                    {
                        ParseError();
                        return null;
                    }
                    a = int.Parse(dimen.Value);
                    b = 0;
                    if (dimen.Unit == "n")
                    {
                        _tokenizer.Next();
                        ConsumeWhitespace();
                        t = _tokenizer.Current;
                        if (t.Type == TokenType.RightParen)
                        {
                            b = 0;
                        }
                        else
                        {
                            b = ParseNumber(true);
                        }
                    }
                    else if (dimen.Unit == "n-")
                    {
                        _tokenizer.Next();
                        ConsumeWhitespace();
                        b = -ParseNumber(false);
                    }
                    else
                    {                        
                        b = -ParseNDashNumber(dimen.Unit);
                        _tokenizer.Next();
                    }
                    ConsumeWhitespace();
                    if (_tokenizer.Current.Type == TokenType.RightParen)
                    {
                        _tokenizer.Next();
                        return new PseudoClassNthSelector(type, a, b);
                    }
                    else
                    {
                        ParseErrorExpected(TokenType.RightParen, ")");
                        return null;
                    }
                case TokenType.Ident:
                    if (t.Value == "even")
                    {
                        _tokenizer.Next();
                        a = 2;
                        b = 0;
                    }
                    else if (t.Value == "odd")
                    {
                        _tokenizer.Next();
                        a = 2;
                        b = 1;
                    }
                    else if (t.Value == "n")
                    {
                        a = 1;
                        _tokenizer.Next();
                        ConsumeWhitespace();
                        t = _tokenizer.Current;
                        if (t.Type == TokenType.RightParen)
                        {
                            b = 0;
                        }
                        else
                        {
                            b = ParseNumber(true);
                        }
                    }
                    else if (t.Value == "-n")
                    {
                        a = -1;
                        _tokenizer.Next();
                        ConsumeWhitespace();
                        t = _tokenizer.Current;
                        if (t.Type == TokenType.RightParen)
                        {
                            b = 0;
                        }
                        else
                        {
                            b = ParseNumber(true);
                        }
                    }
                    else if (t.Value == "n-")
                    {
                        a = 1;
                        _tokenizer.Next();
                        ConsumeWhitespace();
                        b = -ParseNumber(true);
                    }
                    else if (t.Value == "-n-")
                    {
                        a = -1;
                        _tokenizer.Next();
                        ConsumeWhitespace();
                        b = -ParseNumber(true);
                    }
                    else
                    {
                        if (t.Value.StartsWith("-"))
                        {
                            a = -1;
                            b = -ParseNDashNumber(t.Value.Substring(1));
                        }
                        else
                        {
                            a = 1;
                            b = -ParseNDashNumber(t.Value);
                        }
                        _tokenizer.Next();
                    }
                    ConsumeWhitespace();
                    if (_tokenizer.Current.Type == TokenType.RightParen)
                    {
                        _tokenizer.Next();
                        return new PseudoClassNthSelector(type, a, b);
                    }
                    else
                    {
                        ParseErrorExpected(TokenType.RightParen, ")");
                        return null;
                    }
                case TokenType.Delim:
                    if (t.Value == "+")
                    {
                        t = _tokenizer.Next();
                        if (t.Type == TokenType.Ident)
                        {
                            if (t.Value == "n")
                            {
                                a = 1;
                                _tokenizer.Next();
                                ConsumeWhitespace();
                                t = _tokenizer.Current;
                                if (t.Type == TokenType.RightParen)
                                {
                                    b = 0;
                                }
                                else
                                {
                                    b = ParseNumber(true);
                                }
                            }
                            else if (t.Value == "n-")
                            {
                                a = 1;
                                _tokenizer.Next();
                                ConsumeWhitespace();
                                b = -ParseNumber(true);
                            }
                            else
                            {
                                a = 1;
                                b = -ParseNDashNumber(t.Value);
                                _tokenizer.Next();
                            }
                        }
                        else
                        {
                            ParseError();
                            return null;
                        }
                    }
                    else
                    {
                        ParseError();
                        return null;
                    }
                    ConsumeWhitespace();
                    if (_tokenizer.Current.Type == TokenType.RightParen)
                    {
                        _tokenizer.Next();
                        return new PseudoClassNthSelector(type, a, b);
                    }
                    else
                    {
                        ParseErrorExpected(TokenType.RightParen, ")");
                        return null;
                    }
                default:
                    ParseError();
                    return null;
            }
        }

        private int ParseNumber(bool canStartWithSign)
        {
            Token t = _tokenizer.Current;
            if (canStartWithSign)
            {
                if (t.Type == TokenType.Delim && t.Value == "+")
                {
                    _tokenizer.Next();
                    ConsumeWhitespace();
                    return ParseNumber(false);
                }
                else if (t.Type == TokenType.Delim && t.Value == "-")
                {
                    _tokenizer.Next();
                    ConsumeWhitespace();
                    return -ParseNumber(false);
                }
                else if (t.Type == TokenType.Number)
                {
                    var num = t as NumberToken;
                    if (num.IsInteger && num.Value[0].IsOneOf('+', '-'))
                    {
                        _tokenizer.Next();
                        return int.Parse(num.Value);
                    }
                    else
                    {
                        ParseError();
                        return 0;
                    }
                }
                else
                {
                    ParseError();
                    return 0;
                }
            }
            else
            {
                var num = t as NumberToken;
                if (num.IsInteger && !num.Value[0].IsOneOf('+', '-'))
                {
                    _tokenizer.Next();
                    return int.Parse(num.Value);
                }
                else
                {
                    ParseError();
                    return 0;
                }
            }
        }
        private int ParseNDashNumber(string s)
        {
            if (s.Length >= 3 && s.StartsWith("n-"))
            {
                return int.Parse(s.Substring(2));
            }
            else
            {
                ParseError();
                return 0;
            }
        }

        #endregion

        // 8. Combinators
        private CssSelector ParseCombinator()
        {
            var selector = ParseSimpleSelectorSequence();
            while (true)
            {
                Token t = t = _tokenizer.Current;
                bool hasWhitespace = t.Type == TokenType.Whitespace;
                if (hasWhitespace)
                {
                    ConsumeWhitespace();
                    t = _tokenizer.Current;
                }
                SelectorCombinatorType type;
                if (t.Type == TokenType.Delim)
                {
                    if (t.Value == ">")
                    {
                        type = SelectorCombinatorType.Child;
                        _tokenizer.Next();
                    }
                    else if (t.Value == "+")
                    {
                        type = SelectorCombinatorType.AdjacentSibling;
                        _tokenizer.Next();
                    }
                    else if (t.Value == "~")
                    {
                        type = SelectorCombinatorType.GeneralSibling;
                        _tokenizer.Next();
                    }
                    else
                    {
                        return selector;
                    }
                }
                else if (t.Type == TokenType.EOF)
                {
                    return selector;
                }
                else if (hasWhitespace)
                {
                    type = SelectorCombinatorType.Descendant;
                }
                else
                {
                    return selector;
                }
                ConsumeWhitespace();
                selector = new SelectorCombinator(selector, ParseSimpleSelectorSequence(), type);
            }
        }


        private void ConsumeWhitespace()
        {
            Token t = _tokenizer.Current;
            while (t.Type == TokenType.Whitespace)
                t = _tokenizer.Next();
        }
    }
}
