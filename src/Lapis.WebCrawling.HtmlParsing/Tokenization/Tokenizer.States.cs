/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Tokenizer
 * Description : Represents a tokenizer for parsing HTML.
 * Created     : 2015/8/24
 * Note        : http://www.w3.org/TR/html5/syntax.html
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Lapis.WebCrawling.HtmlParsing.Tokenization.TokenizerState;

namespace Lapis.WebCrawling.HtmlParsing.Tokenization
{
    public partial class Tokenizer
    {
        /// <summary>
        /// Switches the <see cref="Tokenizer"/> to the RAWTEXT state.
        /// </summary>
        public void SwitchToRAWTEXTState()
        {
            State = TokenizerState.RAWTEXT;
        }

        /// <summary>
        /// Switches the <see cref="Tokenizer"/> to the RCDATA state.
        /// </summary>
        public void SwitchToRCDATAState()
        {
            State = TokenizerState.RCDATA;
        }

        /// <summary>
        /// Switches the <see cref="Tokenizer"/> to the script data state.
        /// </summary>
        public void SwitchToScriptDataState()
        {
            State = TokenizerState.ScriptData;
        }

        /// <summary>
        /// Switches the <see cref="Tokenizer"/> to the PLAINTEXT state.
        /// </summary>
        public void SwitchToPLAINTEXTState()
        {
            State = TokenizerState.PLAINTEXT;
        }


        private TokenizerState State { get; set; } = Data;

        private void ApplyRule()
        {
            if (State == Data)
                ApplyRuleForDataState();
            else if (State == CharacterReferenceInData)
                ApplyRuleForCharacterReferenceInDataState();
            else if (State == RCDATA)
                ApplyRuleForRCDATAState();
            else if (State == CharacterReferenceInRCDATA)
                ApplyRuleForCharacterReferenceInRCDATAState();
            else if (State == RAWTEXT)
                ApplyRuleForRAWTEXTState();
            else if (State == ScriptData)
                ApplyRuleForScriptDataState();
            else if (State == PLAINTEXT)
                ApplyRuleForPLAINTEXTState();
            else if (State == TagOpen)
                ApplyRuleForTagOpenState();
            else if (State == EndTagOpen)
                ApplyRuleForEndTagOpenState();
            else if (State == TagName)
                ApplyRuleForTagNameState();
            else if (State == RCDATALessThanSign)
                ApplyRuleForRCDATALessThanSignState();
            else if (State == RCDATAEndTagOpen)
                ApplyRuleForRCDATAEndTagOpenState();
            else if (State == RCDATAEndTagName)
                ApplyRuleForRCDATAEndTagNameState();
            else if (State == RAWTEXTLessThanSign)
                ApplyRuleForRAWTEXTLessThanSignState();
            else if (State == RAWTEXTEndTagOpen)
                ApplyRuleForRAWTEXTEndTagOpenState();
            else if (State == RAWTEXTEndTagName)
                ApplyRuleForRAWTEXTEndTagNameState();
            else if (State == ScriptDataLessThanSign)
                ApplyRuleForScriptDataLessThanSignState();
            else if (State == ScriptDataEndTagOpen)
                ApplyRuleForScriptDataEndTagOpenState();
            else if (State == ScriptDataEndTagName)
                ApplyRuleForScriptDataEndTagNameState();
            else if (State == ScriptDataEscapedStart)
                ApplyRuleForScriptDataEscapedStartState();
            else if (State == ScriptDataEscapedStartDash)
                ApplyRuleForScriptDataEscapedStartDashState();
            else if (State == ScriptDataEscaped)
                ApplyRuleForScriptDataEscapedState();
            else if (State == ScriptDataEscapedDash)
                ApplyRuleForScriptDataEscapedDashState();
            else if (State == ScriptDataEscapedDashDash)
                ApplyRuleForScriptDataEscapedDashDashState();
            else if (State == ScriptDataEscapedLessThanSign)
                ApplyRuleForScriptDataEscapedLessThanSignState();
            else if (State == ScriptDataEscapedEndTagOpen)
                ApplyRuleForScriptDataEscapedEndTagOpenState();
            else if (State == ScriptDataEscapedEndTagName)
                ApplyRuleForScriptDataEscapedEndTagNameState();
            else if (State == ScriptDataDoubleEscapedStart)
                ApplyRuleForScriptDataDoubleEscapedStartState();
            else if (State == ScriptDataDoubleEscaped)
                ApplyRuleForScriptDataDoubleEscapedState();
            else if (State == ScriptDataDoubleEscapedDash)
                ApplyRuleForScriptDataDoubleEscapedDashState();
            else if (State == ScriptDataDoubleEscapedDashDash)
                ApplyRuleForScriptDataDoubleEscapedDashDashState();
            else if (State == ScriptDataDoubleEscapedLessThanSign)
                ApplyRuleForScriptDataDoubleEscapedLessThanSignState();
            else if (State == ScriptDataDoubleEscapedEnd)
                ApplyRuleForScriptDataDoubleEscapedEndState();
            else if (State == BeforeAttributeName)
                ApplyRuleForBeforeAttributeNameState();
            else if (State == AttributeName)
                ApplyRuleForAttributeNameState();
            else if (State == AfterAttributeName)
                ApplyRuleForAfterAttributeNameState();
            else if (State == BeforeAttributeValue)
                ApplyRuleForBeforeAttributeValueState();
            else if (State == AttributeValueDoubleQuoted)
                ApplyRuleForAttributeValueDoubleQuotedState();
            else if (State == AttributeValueSingleQuoted)
                ApplyRuleForAttributeValueSingleQuotedState();
            else if (State == AttributeValueUnquoted)
                ApplyRuleForAttributeValueUnquotedState();
            else if (State == CharacterReferenceInAttributeValue)
                ApplyRuleForCharacterReferenceInAttributeValueState();
            else if (State == AfterAttributeQuotedValue)
                ApplyRuleForAfterAttributeQuotedValueState();
            else if (State == SelfClosingStartTag)
                ApplyRuleForSelfClosingStartTagState();
            else if (State == BogusComment)
                ApplyRuleForBogusCommentState();
            else if (State == MarkupDeclarationOpen)
                ApplyRuleForMarkupDeclarationOpenState();
            else if (State == CommentStart)
                ApplyRuleForCommentStartState();
            else if (State == CommentStartDash)
                ApplyRuleForCommentStartDashState();
            else if (State == Comment)
                ApplyRuleForCommentState();
            else if (State == CommentEndDash)
                ApplyRuleForCommentEndDashState();
            else if (State == CommentEnd)
                ApplyRuleForCommentEndState();
            else if (State == CommentEndBang)
                ApplyRuleForCommentEndBangState();
            else if (State == DOCTYPE)
                ApplyRuleForDOCTYPEState();
            else if (State == BeforeDOCTYPEName)
                ApplyRuleForBeforeDOCTYPENameState();
            else if (State == DOCTYPEName)
                ApplyRuleForDOCTYPENameState();
            else if (State == AfterDOCTYPEName)
                ApplyRuleForAfterDOCTYPENameState();
            else if (State == AfterDOCTYPEPublicKeyword)
                ApplyRuleForAfterDOCTYPEPublicKeywordState();
            else if (State == BeforeDOCTYPEPublicIdentifier)
                ApplyRuleForBeforeDOCTYPEPublicIdentifierState();
            else if (State == DOCTYPEPublicIdentifierDoubleQuoted)
                ApplyRuleForDOCTYPEPublicIdentifierDoubleQuotedState();
            else if (State == DOCTYPEPublicIdentifierSingleQuoted)
                ApplyRuleForDOCTYPEPublicIdentifierSingleQuotedState();
            else if (State == AfterDOCTYPEPublicIdentifier)
                ApplyRuleForAfterDOCTYPEPublicIdentifierState();
            else if (State == BetweenDOCTYPEPublicAndSystemIdentifiers)
                ApplyRuleForBetweenDOCTYPEPublicAndSystemIdentifiersState();
            else if (State == AfterDOCTYPESystemKeyword)
                ApplyRuleForAfterDOCTYPESystemKeywordState();
            else if (State == BeforeDOCTYPESystemIdentifier)
                ApplyRuleForBeforeDOCTYPESystemIdentifierState();
            else if (State == DOCTYPESystemIdentifierDoubleQuoted)
                ApplyRuleForDOCTYPESystemIdentifierDoubleQuotedState();
            else if (State == DOCTYPESystemIdentifierSingleQuoted)
                ApplyRuleForDOCTYPESystemIdentifierSingleQuotedState();
            else if (State == AfterDOCTYPESystemIdentifier)
                ApplyRuleForAfterDOCTYPESystemIdentifierState();
            else if (State == BogusDOCTYPE)
                ApplyRuleForBogusDOCTYPEState();
            else if (State == CDATASection)
                ApplyRuleForCDATASectionState();
        }


        private void ApplyRuleForDataState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '&':
                    ReadNextChar();
                    State = CharacterReferenceInData;
                    break;
                case '<':
                    ReadNextChar();
                    State = TagOpen;
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
                case null:
                    EmitToken(new EOFToken(new Location(Line, Span)));
                    break;
                default:
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForCharacterReferenceInDataState()
        {
            State = Data;
            char? c = ConsumeCharacterReferences();
            if (c == null)
                EmitToken(new CharacterToken(new Location(Line, Span), '\u0026'));
            else
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
        }

        private void ApplyRuleForRCDATAState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '&':
                    ReadNextChar();
                    State = CharacterReferenceInRCDATA;
                    break;
                case '<':
                    ReadNextChar();
                    State = RCDATALessThanSign;
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    EmitToken(new EOFToken(new Location(Line, Span)));
                    break;
                default:
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForCharacterReferenceInRCDATAState()
        {
            State = RCDATA;
            char? c = ConsumeCharacterReferences();
            if (c == null)
                EmitToken(new CharacterToken(new Location(Line, Span), '\u0026'));
            else
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
        }

        private void ApplyRuleForRAWTEXTState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '<':
                    ReadNextChar();
                    State = RAWTEXTLessThanSign;
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    EmitToken(new EOFToken(new Location(Line, Span)));
                    break;
                default:
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '<':
                    ReadNextChar();
                    State = ScriptDataLessThanSign;
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    EmitToken(new EOFToken(new Location(Line, Span)));
                    break;
                default:
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }

        }

        private void ApplyRuleForPLAINTEXTState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    EmitToken(new EOFToken(new Location(Line, Span)));
                    break;
                default:
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForTagOpenState()
        {
            char? c = PeekNextChar();
            if (c == '!')
            {
                ReadNextChar();
                State = MarkupDeclarationOpen;
            }
            else if (c == '/')
            {
                ReadNextChar();
                State = EndTagOpen;
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new StartTagToken(new Location(Line, Span));
                tag.Name += (char)(c + 0x0020);
                CurrentTagToken = tag;
                State = TagName;
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new StartTagToken(new Location(Line, Span));
                tag.Name += c;
                CurrentTagToken = tag;
                State = TagName;
            }
            else if (c == '?')
            {
                ReadNextChar();
                ParseError(c);
                State = BogusComment;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                State = Data;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
            }
        }

        private void ApplyRuleForEndTagOpenState()
        {
            char? c = PeekNextChar();
            if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += (char)(c + 0x0020);
                CurrentTagToken = tag;
                State = TagName;
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += c;
                CurrentTagToken = tag;
                State = TagName;
            }
            else if (c == '>')
            {
                ReadNextChar();
                ParseError(c);
                State = Data;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
            }
            else
            {
                ParseError(c);
                State = BogusComment;
            }
        }

        private void ApplyRuleForTagNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = BeforeAttributeName;
            }
            else if (c == '/')
            {
                ReadNextChar();
                State = SelfClosingStartTag;
            }
            else if (c == '>')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += (char)(c + 0x0020);
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                ReadNextChar();
                CurrentTagToken.Name += c;
            }
        }

        private void ApplyRuleForRCDATALessThanSignState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '/':
                    ReadNextChar();
                    TemporaryBuffer.Clear();
                    State = RCDATAEndTagOpen; break;
                default:
                    State = RCDATA;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                    break;
            }
        }

        private void ApplyRuleForRCDATAEndTagOpenState()
        {
            char? c = PeekNextChar();
            if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += (char)(c + 0x0020);
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = RCDATAEndTagName;
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += c;
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = RCDATAEndTagName;
            }
            else
            {
                State = RCDATA;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
            }
        }

        private void ApplyRuleForRCDATAEndTagNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = BeforeAttributeName;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '/')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = SelfClosingStartTag;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '>')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = Data;
                    EmitToken(CurrentTagToken);
                    CurrentTagToken = null;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += (char)(c + 0x0020);
                TemporaryBuffer.Append(c.Value);
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += c;
                TemporaryBuffer.Append(c.Value);
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                State = RCDATA;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
                for (int i = 0; i < TemporaryBuffer.Length; i++)
                {
                    EmitToken(new CharacterToken(new Location(Line, Span), TemporaryBuffer[i]));
                }
            }
        }

        private void ApplyRuleForRAWTEXTLessThanSignState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '/':
                    ReadNextChar();
                    TemporaryBuffer.Clear();
                    State = RAWTEXTEndTagOpen; break;
                default:
                    State = RAWTEXT;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                    break;
            }
        }

        private void ApplyRuleForRAWTEXTEndTagOpenState()
        {
            char? c = PeekNextChar();
            if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += (char)(c + 0x0020);
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = RAWTEXTEndTagName;
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += c;
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = RAWTEXTEndTagName;
            }
            else
            {
                State = RAWTEXT;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
            }
        }

        private void ApplyRuleForRAWTEXTEndTagNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = BeforeAttributeName;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '/')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = SelfClosingStartTag;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '>')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = Data;
                    EmitToken(CurrentTagToken);
                    CurrentTagToken = null;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += (char)(c + 0x0020);
                TemporaryBuffer.Append(c.Value);
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += c;
                TemporaryBuffer.Append(c.Value);
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                State = RAWTEXT;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
                for (int i = 0; i < TemporaryBuffer.Length; i++)
                {
                    EmitToken(new CharacterToken(new Location(Line, Span), TemporaryBuffer[i]));
                }
            }
        }

        private void ApplyRuleForScriptDataLessThanSignState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '/':
                    ReadNextChar();
                    TemporaryBuffer.Clear();
                    State = ScriptDataEndTagOpen;
                    break;
                case '!':
                    ReadNextChar();
                    State = ScriptDataEscapedStart;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u0021'));
                    break;
                default:
                    State = ScriptData;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                    break;
            }
        }
        private void ApplyRuleForScriptDataEndTagOpenState()
        {
            char? c = PeekNextChar();
            if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += (char)(c + 0x0020);
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = RAWTEXTEndTagName;
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += c;
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = RAWTEXTEndTagName;
            }
            else
            {
                State = ScriptData;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
            }
        }

        private void ApplyRuleForScriptDataEndTagNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = BeforeAttributeName;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '/')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = SelfClosingStartTag;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '>')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = Data;
                    EmitToken(CurrentTagToken);
                    CurrentTagToken = null;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += (char)(c + 0x0020);
                TemporaryBuffer.Append(c.Value);
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += c;
                TemporaryBuffer.Append(c.Value);
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                State = ScriptData;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
                for (int i = 0; i < TemporaryBuffer.Length; i++)
                {
                    EmitToken(new CharacterToken(new Location(Line, Span), TemporaryBuffer[i]));
                }
            }
        }

        private void ApplyRuleForScriptDataEscapedStartState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    State = ScriptDataEscapedStartDash;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                default:
                    State = ScriptData;
                    break;
            }
        }

        private void ApplyRuleForScriptDataEscapedStartDashState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    State = ScriptDataEscapedDashDash;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                default:
                    State = ScriptData;
                    break;
            }
        }

        private void ApplyRuleForScriptDataEscapedState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    State = ScriptDataEscapedDash;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                case '<':
                    ReadNextChar();
                    State = ScriptDataEscapedLessThanSign;
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    State = Data;
                    break;
                default:
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataEscapedDashState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    State = ScriptDataEscapedDashDash;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                case '<':
                    ReadNextChar();
                    State = ScriptDataEscapedLessThanSign;
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    State = ScriptDataEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    State = Data; break;
                default:
                    ReadNextChar();
                    State = ScriptDataEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataEscapedDashDashState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                case '<':
                    ReadNextChar();
                    State = ScriptDataEscapedLessThanSign;
                    break;
                case '>':
                    ReadNextChar();
                    State = ScriptData;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003E'));
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    State = ScriptDataEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    State = Data;
                    break;
                default:
                    ReadNextChar();
                    State = ScriptDataEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataEscapedLessThanSignState()
        {
            char? c = PeekNextChar();
            if (c == '/')
            {
                ReadNextChar();
                TemporaryBuffer.Clear();
                State = ScriptDataEscapedEndTagOpen;
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                TemporaryBuffer.Clear();
                TemporaryBuffer.Append((char)(c + 0x0020));
                State = ScriptDataDoubleEscapedStart;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                TemporaryBuffer.Clear();
                TemporaryBuffer.Append(c.Value);
                State = ScriptDataDoubleEscapedStart;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else
            {
                State = ScriptDataEscaped;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
            }
        }

        private void ApplyRuleForScriptDataEscapedEndTagOpenState()
        {
            char? c = PeekNextChar();
            if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += (char)(c + 0x0020);
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = ScriptDataEscapedEndTagName;
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                var tag = new EndTagToken(new Location(Line, Span));
                tag.Name += c;
                CurrentTagToken = tag;
                TemporaryBuffer.Append(c.Value);
                State = ScriptDataEscapedEndTagName;
            }
            else
            {
                State = ScriptDataEscaped;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
            }
        }

        private void ApplyRuleForScriptDataEscapedEndTagNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = BeforeAttributeName;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '/')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = SelfClosingStartTag;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c == '>')
            {
                if (IsAppropriateEndTagToken(CurrentTagToken))
                {
                    ReadNextChar();
                    State = Data;
                    EmitToken(CurrentTagToken);
                    CurrentTagToken = null;
                }
                else
                {
                    goto AnythingElse;
                }
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += (char)(c + 0x0020);
                TemporaryBuffer.Append(c.Value);
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Name += c;
                TemporaryBuffer.Append(c.Value);
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                State = ScriptDataEscaped;
                EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
                for (int i = 0; i < TemporaryBuffer.Length; i++)
                {
                    EmitToken(new CharacterToken(new Location(Line, Span), TemporaryBuffer[i]));
                }
            }
        }

        private void ApplyRuleForScriptDataDoubleEscapedStartState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020' ||
                c == '/' ||
                c == '>')
            {
                ReadNextChar();
                if (TemporaryBuffer.ToString() == "script")
                {
                    State = ScriptDataDoubleEscaped;
                }
                else
                {
                    State = ScriptDataEscaped;
                }
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                TemporaryBuffer.Append((char)(c + 0x0020));
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                TemporaryBuffer.Append(c.Value);
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else
            {
                State = ScriptDataEscaped;
            }
        }

        private void ApplyRuleForScriptDataDoubleEscapedState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    State = ScriptDataDoubleEscapedDash;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                case '<':
                    ReadNextChar();
                    State = ScriptDataDoubleEscapedLessThanSign;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    ParseError(c);
                    State = Data; break;
                default:
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataDoubleEscapedDashState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    State = ScriptDataDoubleEscapedDashDash;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                case '<':
                    ReadNextChar();
                    State = ScriptDataDoubleEscapedLessThanSign;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    State = ScriptDataDoubleEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    ParseError(c);
                    State = Data;
                    break;
                default:
                    ReadNextChar();
                    State = ScriptDataDoubleEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataDoubleEscapedDashDashState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '-':
                    ReadNextChar();
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002D'));
                    break;
                case '<':
                    ReadNextChar();
                    State = ScriptDataDoubleEscapedLessThanSign;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003C'));
                    break;
                case '>':
                    ReadNextChar();
                    State = ScriptData;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u003E'));
                    break;
                case '\0':
                    ReadNextChar();
                    ParseError(c);
                    State = ScriptDataDoubleEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\uFFFD'));
                    break;
                case null:
                    ParseError(c);
                    State = Data;
                    break;
                default:
                    ReadNextChar();
                    State = ScriptDataDoubleEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataDoubleEscapedLessThanSignState()
        {
            char? c = PeekNextChar();
            switch (c)
            {
                case '/':
                    ReadNextChar();
                    TemporaryBuffer.Clear();
                    State = ScriptDataDoubleEscapedEnd;
                    EmitToken(new CharacterToken(new Location(Line, Span), '\u002F'));
                    break;
                default:
                    ReadNextChar();
                    State = ScriptDataDoubleEscaped;
                    EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
                    break;
            }
        }

        private void ApplyRuleForScriptDataDoubleEscapedEndState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020' ||
                c == '\u002F' ||
                c == '\u003E')
            {
                ReadNextChar();
                if (TemporaryBuffer.ToString() == "script")
                {
                    State = ScriptDataEscaped;
                }
                else
                {
                    State = ScriptDataDoubleEscaped;
                }
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                TemporaryBuffer.Append((char)(c + 0x0020));
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else if (c.IsLowercaseAsciiLetter())
            {
                ReadNextChar();
                TemporaryBuffer.Append(c.Value);
                EmitToken(new CharacterToken(new Location(Line, Span), c.Value));
            }
            else
            {
                State = ScriptDataDoubleEscaped;
            }
        }

        private void ApplyRuleForBeforeAttributeNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u002F')
            {
                ReadNextChar();
                State = SelfClosingStartTag;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var attr = new TagTokenAttribute();
                attr.Name += (char)(c + 0x0020);
                CurrentTagToken.Attributes.Add(attr);
                State = AttributeName;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                var attr = new TagTokenAttribute();
                attr.Name += '\uFFFD';
                CurrentTagToken.Attributes.Add(attr);
                State = AttributeName;
            }
            else if (c == '\"' ||
                     c == '\'' ||
                     c == '<' ||
                     c == '=')
            {
                ReadNextChar();
                ParseError(c);
                goto AnythingElse;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                ReadNextChar();
                var attr = new TagTokenAttribute();
                attr.Name += c;
                CurrentTagToken.Attributes.Add(attr);
                State = AttributeName;
            }
        }

        private void ApplyRuleForAttributeNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = AfterAttributeName;
            }
            else if (c == '\u002F')
            {
                ReadNextChar();
                State = SelfClosingStartTag;
            }
            else if (c == '\u003D')
            {
                ReadNextChar();
                State = BeforeAttributeValue;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentTagToken.Attributes.Last().Name += (char)(c + 0x0020);
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentTagToken.Attributes.Last().Name += '\uFFFD';
            }
            else if (c == '\"' ||
                     c == '\'' ||
                     c == '<')
            {
                ReadNextChar();
                ParseError(c);
                goto AnythingElse;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                ReadNextChar();
                CurrentTagToken.Attributes.Last().Name += c;
            }
        }

        private void ApplyRuleForAfterAttributeNameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u002F')
            {
                ReadNextChar();
                State = SelfClosingStartTag;
            }
            else if (c == '\u003D')
            {
                ReadNextChar();
                State = BeforeAttributeValue;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var attr = new TagTokenAttribute();
                attr.Name += (char)(c + 0x0020);
                CurrentTagToken.Attributes.Add(attr);
                State = AttributeName;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                ReadNextChar();
                var attr = new TagTokenAttribute();
                attr.Name += '\uFFFD';
                CurrentTagToken.Attributes.Add(attr);
                State = AttributeName;
            }
            else if (c == '\"' ||
                     c == '\'' ||
                     c == '<')
            {
                ReadNextChar();
                ParseError(c);
                goto AnythingElse;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                ReadNextChar();
                var attr = new TagTokenAttribute();
                attr.Name += c;
                CurrentTagToken.Attributes.Add(attr);
                State = AttributeName;
            }
        }

        private void ApplyRuleForBeforeAttributeValueState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u0022')
            {
                ReadNextChar();
                State = AttributeValueDoubleQuoted;
            }
            else if (c == '\u0026')
            {
                State = AttributeValueUnquoted;
            }
            else if (c == '\u0027')
            {
                ReadNextChar();
                State = AttributeValueSingleQuoted;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentTagToken.Attributes.Last().Value += '\uFFFD';
                State = AttributeValueUnquoted;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c == '\u003C' ||
                     c == '\u003D' ||
                     c == '\u0060')
            {
                ReadNextChar();
                ParseError(c);
                goto AnythingElse;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                ReadNextChar();
                CurrentTagToken.Attributes.Last().Value += '\uFFFD';
                State = AttributeValueUnquoted;
            }
        }

        private void ApplyRuleForAttributeValueDoubleQuotedState()
        {
            char? c = PeekNextChar();
            if (c == '\u0022')
            {
                ReadNextChar();
                State = AfterAttributeQuotedValue;
            }
            else if (c == '\u0026')
            {
                ReadNextChar();
                _tempAttributeValueState = State;
                _tempAdditionalAllowedCharacter = '\u0022';
                State = CharacterReferenceInAttributeValue;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentTagToken.Attributes.Last().Value += '\uFFFD';
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                ReadNextChar();
                CurrentTagToken.Attributes.Last().Value += c;
            }
        }

        private void ApplyRuleForAttributeValueSingleQuotedState()
        {
            char? c = PeekNextChar();
            if (c == '\u0027')
            {
                ReadNextChar();
                State = AfterAttributeQuotedValue;
            }
            else if (c == '\u0026')
            {
                ReadNextChar();
                _tempAttributeValueState = State;
                _tempAdditionalAllowedCharacter = '\u0027';
                State = CharacterReferenceInAttributeValue;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentTagToken.Attributes.Last().Value += '\uFFFD';
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                ReadNextChar();
                CurrentTagToken.Attributes.Last().Value += c;
            }
        }

        private void ApplyRuleForAttributeValueUnquotedState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = BeforeAttributeName;
            }
            else if (c == '\u0026')
            {
                ReadNextChar();
                _tempAttributeValueState = State;
                _tempAdditionalAllowedCharacter = '>';
                State = CharacterReferenceInAttributeValue;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentTagToken.Attributes.Last().Value += '\uFFFD';
            }
            else if (c == '\u0027' ||
                     c == '\u003C' ||
                     c == '\u003D' ||
                     c == '\u0060')
            {
                ReadNextChar();
                ParseError(c);
                goto AnythingElse;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                goto AnythingElse;
            }
            return;
            AnythingElse:
            {
                ReadNextChar();
                CurrentTagToken.Attributes.Last().Value += c;
            }
        }

        private TokenizerState? _tempAttributeValueState;
        private char? _tempAdditionalAllowedCharacter;
        private void ApplyRuleForCharacterReferenceInAttributeValueState()
        {
            char? c;
            if (_tempAdditionalAllowedCharacter == null)
                c = ConsumeCharacterReferences();
            else
                c = ConsumeCharacterReferences(_tempAdditionalAllowedCharacter.Value);
            if (c == null)
            {
                CurrentTagToken.Attributes.Last().Value += '&';
            }
            else
            {
                CurrentTagToken.Attributes.Last().Value += c;
            }
            State = _tempAttributeValueState.Value;
            _tempAttributeValueState = null;
            _tempAdditionalAllowedCharacter = null;
        }

        private void ApplyRuleForAfterAttributeQuotedValueState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = BeforeAttributeName;
            }
            else if (c == '\u002F')
            {
                ReadNextChar();
                State = SelfClosingStartTag;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                State = BeforeAttributeName;
            }
        }

        private void ApplyRuleForSelfClosingStartTagState()
        {
            char? c = PeekNextChar();
            if (c == '\u003E')
            {
                ReadNextChar();
                CurrentTagToken.IsSelfClosing = true;
                State = Data;
                EmitToken(CurrentTagToken);
                CurrentTagToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
            }
            else
            {
                State = BeforeAttributeName;
            }
        }

        private void ApplyRuleForBogusCommentState()
        {
            Location location = new Location(Line, Span);
            var sb = new StringBuilder();
            char? c;
            while ((c = PeekNextChar()) != EOF)
            {
                ReadNextChar();
                if (c == '\0')
                    c = '\uFFFD';
                sb.Append(c);
                int length = sb.Length;
                if (length >= 1 &&
                    sb[length - 1] == '>')
                {
                    sb.Length -= 1;
                    break;
                }
            }
            EmitToken(new CommentToken(location, sb.ToString()));
            State = Data;
        }

        private void ApplyRuleForMarkupDeclarationOpenState()
        {
            Location location = new Location(Line, Span);
            string s;
            s = PeekNextChars(2);
            if (s == "--")
            {
                ReadNextChars(2);
                CurrentCommentToken = new CommentToken(location);
                State = CommentStart;
                return;
            }
            s = PeekNextChars(7);
            if (s.ToUpper() == "DOCTYPE")
            {
                ReadNextChars(7);
                State = DOCTYPE;
                return;
            }          
            if (s == "[CDATA[")
            {
                ReadNextChars(7);
                State = CDATASection;
                return;
            }
            else
            {
                ParseError(PeekNextChar());
                State = BogusComment;
            }
        }

        private void ApplyRuleForCommentStartState()
        {
            char? c = PeekNextChar();
            if (c == '\u002D')
            {
                ReadNextChar();
                State = CommentStartDash;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += '\uFFFD';
                State = Comment;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentCommentToken.Data += c;
                State = Comment;
            }
        }

        private void ApplyRuleForCommentStartDashState()
        {
            char? c = PeekNextChar();
            if (c == '\u002D')
            {
                ReadNextChar();
                State = CommentEnd;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\uFFFD');
                State = Comment;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += (c);
                State = Comment;
            }
        }

        private void ApplyRuleForCommentState()
        {
            char? c = PeekNextChar();
            if (c == '\u002D')
            {
                ReadNextChar();
                State = CommentEndDash;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += ('\uFFFD');
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentCommentToken.Data += (c);
            }
        }

        private void ApplyRuleForCommentEndDashState()
        {
            char? c = PeekNextChar();
            if (c == '\u002D')
            {
                ReadNextChar();
                State = CommentEnd;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\uFFFD');
                State = Comment;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += (c);
                State = Comment;
            }
        }

        private void ApplyRuleForCommentEndState()
        {
            char? c = PeekNextChar();
            if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\uFFFD');
                State = Comment;
            }
            else if (c == '\u0021')
            {
                ReadNextChar();
                ParseError(c);
                State = CommentEndBang;
            }
            else if (c == '\u002D')
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += ('\u002D');
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += (c);
                State = Comment;
            }
        }

        private void ApplyRuleForCommentEndBangState()
        {
            char? c = PeekNextChar();
            if (c == '\u002D')
            {
                ReadNextChar();
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\u002D');
                State = CommentEndDash;
            }
            if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\u0021');
                CurrentCommentToken.Data += ('\uFFFD');
                State = Comment;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                EmitToken(CurrentCommentToken);
                CurrentCommentToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\u002D');
                CurrentCommentToken.Data += ('\u0021');
                CurrentCommentToken.Data += (c);
                State = Comment;
            }
        }

        private void ApplyRuleForDOCTYPEState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = BeforeDOCTYPEName;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                var doctype = new DOCTYPEToken(new Location(Line, Span));
                doctype.ForceQuirksFlag = true;
                EmitToken(doctype);
            }
            else
            {
                ParseError(c);
                State = BeforeDOCTYPEName;
            }
        }

        private void ApplyRuleForBeforeDOCTYPENameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                var doctype = new DOCTYPEToken(new Location(Line, Span));
                doctype.Name += (char)(c + 0x0020);
                CurrentDOCTYPEToken = doctype;
                State = DOCTYPEName;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                var doctype = new DOCTYPEToken(new Location(Line, Span));
                doctype.Name += '\uFFFD';
                CurrentDOCTYPEToken = doctype;
                State = DOCTYPEName;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                var doctype = new DOCTYPEToken(new Location(Line, Span));
                doctype.ForceQuirksFlag = true;
                State = Data;
                EmitToken(doctype);
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                var doctype = new DOCTYPEToken(new Location(Line, Span));
                doctype.ForceQuirksFlag = true;
                EmitToken(doctype);
            }
            else
            {
                ReadNextChar();
                var doctype = new DOCTYPEToken(new Location(Line, Span));
                doctype.Name += c;
                CurrentDOCTYPEToken = doctype;
                State = DOCTYPEName;
            }
        }

        private void ApplyRuleForDOCTYPENameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = AfterDOCTYPEName;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c.IsUppercaseAsciiLetter())
            {
                ReadNextChar();
                CurrentDOCTYPEToken.Name += (char)(c + 0x0020);
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.Name += '\uFFFD';
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentDOCTYPEToken.Name += c;
            }
        }

        private void ApplyRuleForAfterDOCTYPENameState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                var s = PeekNextChars(6);
                if (s.ToUpper() == "PUBLIC")
                {
                    ReadNextChars(6);
                    State = AfterDOCTYPEPublicKeyword;
                }
                else if (s.ToUpper() == "SYSTEM")
                {
                    ReadNextChars(6);
                    State = AfterDOCTYPESystemKeyword;
                }
                else
                {
                    ReadNextChar();
                    ParseError(c);
                    CurrentDOCTYPEToken.ForceQuirksFlag = true;
                    State = BogusDOCTYPE;
                }
            }
        }

        private void ApplyRuleForAfterDOCTYPEPublicKeywordState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = BeforeDOCTYPEPublicIdentifier;
            }
            else if (c == '\u0022')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.PublicIdentifier = string.Empty;
                State = DOCTYPEPublicIdentifierDoubleQuoted;
            }
            else if (c == '\u0027')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.PublicIdentifier = string.Empty;
                State = DOCTYPEPublicIdentifierSingleQuoted;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = BogusDOCTYPE;
            }
        }

        private void ApplyRuleForBeforeDOCTYPEPublicIdentifierState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u0022')
            {
                ReadNextChar();
                CurrentDOCTYPEToken.PublicIdentifier = string.Empty;
                State = DOCTYPEPublicIdentifierDoubleQuoted;
            }
            else if (c == '\u0027')
            {
                ReadNextChar();
                CurrentDOCTYPEToken.PublicIdentifier = string.Empty;
                State = DOCTYPEPublicIdentifierSingleQuoted;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = BogusDOCTYPE;
            }
        }

        private void ApplyRuleForDOCTYPEPublicIdentifierDoubleQuotedState()
        {
            char? c = PeekNextChar();
            if (c == '\u0022')
            {
                ReadNextChar();
                State = AfterDOCTYPEPublicIdentifier;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.PublicIdentifier += '\uFFFD';
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentDOCTYPEToken.PublicIdentifier += c;
            }
        }

        private void ApplyRuleForDOCTYPEPublicIdentifierSingleQuotedState()
        {
            char? c = PeekNextChar();
            if (c == '\u0027')
            {
                ReadNextChar();
                State = AfterDOCTYPEPublicIdentifier;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.PublicIdentifier += '\uFFFD';
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentDOCTYPEToken.PublicIdentifier += c;
            }
        }

        private void ApplyRuleForAfterDOCTYPEPublicIdentifierState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = BetweenDOCTYPEPublicAndSystemIdentifiers;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == '\u0022')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierDoubleQuoted;
            }
            else if (c == '\u0027')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierSingleQuoted;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = BogusDOCTYPE;
            }
        }

        private void ApplyRuleForBetweenDOCTYPEPublicAndSystemIdentifiersState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == '\u0022')
            {
                ReadNextChar();
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierDoubleQuoted;
            }
            else if (c == '\u0027')
            {
                ReadNextChar();
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierSingleQuoted;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = BogusDOCTYPE;
            }
        }

        private void ApplyRuleForAfterDOCTYPESystemKeywordState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
                State = BeforeDOCTYPESystemIdentifier;
            }
            else if (c == '\u0022')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierDoubleQuoted;
            }
            else if (c == '\u0027')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierSingleQuoted;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = BogusDOCTYPE;
            }
        }

        private void ApplyRuleForBeforeDOCTYPESystemIdentifierState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u0022')
            {
                ReadNextChar();
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierDoubleQuoted;
            }
            else if (c == '\u0027')
            {
                ReadNextChar();
                CurrentDOCTYPEToken.SystemIdentifier = string.Empty;
                State = DOCTYPESystemIdentifierSingleQuoted;
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = BogusDOCTYPE;
            }
        }

        private void ApplyRuleForDOCTYPESystemIdentifierDoubleQuotedState()
        {
            char? c = PeekNextChar();
            if (c == '\u0022')
            {
                ReadNextChar();
                State = AfterDOCTYPESystemIdentifier;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.SystemIdentifier += '\uFFFD';
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentDOCTYPEToken.SystemIdentifier += c;
            }
        }

        private void ApplyRuleForDOCTYPESystemIdentifierSingleQuotedState()
        {
            char? c = PeekNextChar();
            if (c == '\u0027')
            {
                ReadNextChar();
                State = AfterDOCTYPESystemIdentifier;
            }
            else if (c == '\0')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.SystemIdentifier += '\uFFFD';
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                ParseError(c);
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                CurrentDOCTYPEToken.SystemIdentifier += c;
            }
        }

        private void ApplyRuleForAfterDOCTYPESystemIdentifierState()
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020')
            {
                ReadNextChar();
            }
            else if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                ParseError(c);
                State = Data;
                CurrentDOCTYPEToken.ForceQuirksFlag = true;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
                ParseError(c);
                State = BogusDOCTYPE;
            }
        }

        private void ApplyRuleForBogusDOCTYPEState()
        {
            char? c = PeekNextChar();
            if (c == '\u003E')
            {
                ReadNextChar();
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else if (c == EOF)
            {
                State = Data;
                EmitToken(CurrentDOCTYPEToken);
                CurrentDOCTYPEToken = null;
            }
            else
            {
                ReadNextChar();
            }
        }

        private void ApplyRuleForCDATASectionState()
        {
            State = Data;
            var sb = new StringBuilder();
            char? c;
            while ((c = PeekNextChar()) != EOF)
            {
                ReadNextChar();
                sb.Append(c);
                int length = sb.Length;
                if (length >= 3 &&
                    sb[length - 1] == '>' &&
                    sb[length - 2] == ']' &&
                    sb[length - 3] == ']')
                {
                    sb.Length -= 3;
                    break;
                }
            }
            for (int i = 0; i < sb.Length; i++)
            {
                EmitToken(new CharacterToken(new Location(Line, Span), sb[i]));
            }
        }


        private char? ConsumeCharacterReferences(params char[] additionalAllowedCharacter)
        {
            char? c = PeekNextChar();
            if (c == '\u0009' ||
                c == '\u000A' ||
                c == '\u000C' ||
                c == '\u0020' ||
                c == '\u003C' ||
                c == '\u0026' ||
                c == EOF ||
                additionalAllowedCharacter.Contains(c.Value))
            {
                return null;
            }
            else if (c == '\u0023')
            {
                char? ch = PeekNextChar(2);
                if (ch == 'x' || ch == 'X')
                {
                    // ASCII hex digits.
                }
                else
                {
                    // ASCII digits
                }
            }
            else
            {
                // named character references
            }
            {
                // TODO
                ReadNextChar();
                return c;
            }
        }

    }
}
