/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : TreeConstructor
 * Description : Provides methods for construction of HTML trees.
 * Created     : 2015/8/25
 * Note        : http://www.w3.org/TR/html5/syntax.html
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.Tokenization;
using static Lapis.WebCrawling.HtmlParsing.TreeConstruction.InsertionMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.TreeConstruction
{
    public partial class TreeConstructor
    {
        private void ApplyRule(InsertionMode insertionMode)
        {
            if (insertionMode == Initial)
                ApplyRuleForInitialInsertionMode();
            else if (insertionMode == BeforeHtml)
                ApplyRuleForBeforeHtmlInsertionMode();
            else if (insertionMode == BeforeHead)
                ApplyRuleForBeforeHeadInsertionMode();
            else if (insertionMode == InHead)
                ApplyRuleForInHeadInsertionMode();
            else if (insertionMode == InHeadNoScript)
                ApplyRuleForInHeadNoScriptInsertionMode();
            else if (insertionMode == AfterHead)
                ApplyRuleForAfterHeadInsertionMode();
            else if (insertionMode == InBody)
                ApplyRuleForInBodyInsertionMode();
            else if (insertionMode == Text)
                ApplyRuleForTextInsertionMode();
            else if (insertionMode == InTable)
                ApplyRuleForInTableInsertionMode();
            else if (insertionMode == InTableText)
                ApplyRuleForInTableTextInsertionMode();
            else if (insertionMode == InCaption)
                ApplyRuleForInCaptionInsertionMode();
            else if (insertionMode == InColumnGroup)
                ApplyRuleForInColumnGroupInsertionMode();
            else if (insertionMode == InTableBody)
                ApplyRuleForInTableBodyInsertionMode();
            else if (insertionMode == InRow)
                ApplyRuleForInRowInsertionMode();
            else if (insertionMode == InCell)
                ApplyRuleForInCellInsertionMode();
            else if (insertionMode == InSelect)
                ApplyRuleForInSelectInsertionMode();
            else if (insertionMode == InSelectInTable)
                ApplyRuleForInSelectInTableInsertionMode();
            else if (insertionMode == InTemplate)
                ApplyRuleForInTemplateInsertionMode();
            else if (insertionMode == AfterBody)
                ApplyRuleForAfterBodyInsertionMode();
            else if (insertionMode == InFrameset)
                ApplyRuleForInFramesetInsertionMode();
            else if (insertionMode == AfterFrameset)
                ApplyRuleForAfterFramesetInsertionMode();
            else if (insertionMode == AfterAfterBody)
                ApplyRuleForAfterAfterBodyInsertionMode();
            else if (insertionMode == AfterAfterFrameset)
                ApplyRuleForAfterAfterFramesetInsertionMode();
            // System.Diagnostics.Debug.WriteLine(InsertionMode);
        }


        #region 8.2.5.4 The rules for parsing tokens in HTML content

        private void ApplyRuleForInitialInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                var comment = new HtmlComment(com.Data);
                Document.Children.Add(comment);
                return;
            }
            var type = t as DOCTYPEToken;
            if (type != null)
            {
                ReadNextToken();
                var doctype = new HtmlDocumentType(type.Name, type.PublicIdentifier, type.SystemIdentifier);
                Document.DocumentType = doctype;
                return;
            }
            else
            {
                InsertionMode = BeforeHtml;
                return;
            }
        }

        private void ApplyRuleForBeforeHtmlInsertionMode()
        {
            var t = PeekNextToken();
            var type = t as DOCTYPEToken;
            if (type != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                var comment = new HtmlComment(com.Data);
                Document.Children.Add(comment);
                return;
            }
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ReadNextToken();
                var html = CreateElementForToken(start);
                Document.Children.Add(html);
                StackOfOpenElements.Add(html);
                InsertionMode = BeforeHead;
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name.IsOneOf("head", "body", "html", "br"))
            {
                goto AnythingElse;
            }
            if (end != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }            
            AnythingElse:
            {
                var html = new HtmlElement("html");
                Document.Children.Add(html);
                StackOfOpenElements.Add(html);
                InsertionMode = BeforeHead;
                return;
            }
        }

        private void ApplyRuleForBeforeHeadInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                var comment = new HtmlComment(com.Data);
                Document.Children.Add(comment);
                return;
            }
            var type = t as DOCTYPEToken;
            if (type != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (start != null && start.Name == "head")
            {
                ReadNextToken();
                var head = CreateElementForToken(start);
                InsertHtmlElement(head);
                HeadElementPointer = head;
                InsertionMode = InHead;
                return;
            }
            var end = t as EndTagToken;
            if (end != null &&
                end.Name.IsOneOf("head", "body", "html", "br"))
            {
                goto AnythingElse;
            }
            if (end != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            AnythingElse:
            {
                var head = new HtmlElement("head");
                InsertHtmlElement(head);
                HeadElementPointer = head;
                InsertionMode = InHead;
                return;
            }
        }

        private void ApplyRuleForInHeadInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                InsertCharacter(c.Char);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            var type = t as DOCTYPEToken;
            if (type != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("base", "basefont", "bgsound", "link"))
            {
                ReadNextToken();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                return;
            }
            if (start != null && start.Name == "meta")
            {
                ReadNextToken();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                return;
            }
            if (start != null && start.Name == "title")
            {
                ReadNextToken();
                ParseGenericRCDATAElement(start);
                return;
            }
            if (start != null &&
               ((start.Name == "noscript" && ScriptingFlag) ||
                start.Name.IsOneOf("noframes", "style")))
            {
                ReadNextToken();
                ParseGenericRawTextElement(start);
                return;
            }
            if (start != null &&
               (start.Name == "noscript" && !ScriptingFlag))
            {
                ReadNextToken();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                InsertionMode = InHeadNoScript;
                return;
            }
            if (start != null && start.Name == "script")
            {
                ReadNextToken();
                var script = CreateElementForToken(start);
                InsertHtmlElement(script);
                Tokenizer.SwitchToScriptDataState();
                OriginalInsertionMode = InsertionMode;
                InsertionMode = Text;
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "head")
            {
                ReadNextToken();
                PopCurrentNodeOffStackOfOpenElements();
                InsertionMode = AfterHead;
                return;
            }
            if (end != null &&
                end.Name.IsOneOf("body", "html", "br"))
            {
                goto AnythingElse;
            }
            if (start != null && start.Name == "template")
            {
                ReadNextToken();
                var template = CreateElementForToken(start);
                InsertHtmlElement(template);
                ListOfActiveFormattingElements.Add(ScopeMarker);
                FramesetOkFlag = false;
                InsertionMode = InTemplate;
                StackOfTemplateInsertionModes.Push(InTemplate);
                return;
            }
            if (end != null && end.Name == "template")
            {
                ReadNextToken();
                if (!StackOfOpenElements.Any(e => e.Name == "template"))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    GenerateImpliedEndTags();
                    if (CurrentNode.Name != "template")
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "template");
                    ClearActiveFormattingElementsToLastMarker();
                    StackOfTemplateInsertionModes.Pop();
                    ResetInsertionModeAppropriately();
                    return;
                }
            }
            if ((start != null && start.Name == "head") ||
                end != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            AnythingElse:
            {
                PopCurrentNodeOffStackOfOpenElements();
                InsertionMode = AfterHead;
                return;
            }
        }

        private void ApplyRuleForInHeadNoScriptInsertionMode()
        {
            var t = PeekNextToken();
            var type = t as DOCTYPEToken;
            if (type != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "noscript")
            {
                ReadNextToken();
                PopCurrentNodeOffStackOfOpenElements();
                InsertionMode = InHead;
                return;
            }
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ApplyRule(InHead);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ApplyRule(InHead);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("basefont", "bgsound", "link", "meta", "noframes", "style"))
            {
                ApplyRule(InHead);
                return;
            }
            if (end != null && end.Name == "br")
            {
                goto AnythingElse;
            }
            if (start != null &&
                start.Name.IsOneOf("head", "noscript"))
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            if (end != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            AnythingElse:
            {
                ParseError(t);
                PopCurrentNodeOffStackOfOpenElements();
                InsertionMode = InHead;
                return;
            }
        }

        private void ApplyRuleForAfterHeadInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                InsertCharacter(c.Char);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            var type = t as DOCTYPEToken;
            if (type != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (start != null && start.Name == "body")
            {
                ReadNextToken();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                FramesetOkFlag = false;
                InsertionMode = InBody;
                return;
            }
            if (start != null && start.Name == "frameset")
            {
                ReadNextToken();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                InsertionMode = InFrameset;
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("base", "basefont", "bgsound", "link",
                "meta", "noframes", "script", "style", "template", "title"))
            {
                ParseError(t);
                StackOfOpenElements.Add(HeadElementPointer);
                ApplyRule(InHead);
                StackOfOpenElements.Remove(HeadElementPointer);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "template")
            {
                ApplyRule(InHead);
                return;
            }
            if (end != null &&
                end.Name.IsOneOf("body", "html", "br"))
            {
                goto AnythingElse;
            }
            if ((start != null && start.Name == "head") || end != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            AnythingElse:
            {
                var body = new HtmlElement("body");
                InsertHtmlElement(body);
                InsertionMode = InBody;
                return;
            }
        }

        private void ApplyRuleForInBodyInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null && c.Char == '\0')
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                InsertCharacter(c.Char);
                return;
            }
            if (c != null)
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                InsertCharacter(c.Char);
                FramesetOkFlag = false;
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            var type = t as DOCTYPEToken;
            if (type != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ReadNextToken();
                ParseError(t);
                if (StackOfOpenElements.Any(token => token.Name == "template"))
                {
                    return;
                }
                else
                {
                    var top = StackOfOpenElements.First();
                    foreach (var attr in start.Attributes)
                    {
                        if (!top.Attributes.Any(a => a.Name == attr.Name))
                        {
                            top.Attributes.Add(new HtmlAttribute(attr.Name, attr.Value));
                        }
                    }
                    return;
                }
            }
            if (start != null &&
                start.Name.IsOneOf("base", "basefont", "bgsound", "link", "meta",
                "noframes", "script", "style", "template", "title"))
            {
                ApplyRule(InHead);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "template")
            {
                ApplyRule(InHead);
                return;
            }
            if (start != null && start.Name == "body")
            {
                ReadNextToken();
                ParseError(t);
                if (StackOfOpenElements.Count == 1 ||
                    StackOfOpenElements[1].Name != "body" ||
                    StackOfOpenElements.Any(e => e.Name == "template"))
                {
                    return;
                }
                else
                {
                    FramesetOkFlag = false;
                    var body = StackOfOpenElements[1];
                    foreach (var attr in start.Attributes)
                    {
                        if (!body.Attributes.Any(a => a.Name == attr.Name))
                        {
                            body.Attributes.Add(new HtmlAttribute(attr.Name, attr.Value));
                        }
                    }
                    return;
                }
            }
            if (start != null && start.Name == "frameset")
            {
                ReadNextToken();
                ParseError(t);
                if (StackOfOpenElements.Count == 1 ||
                    StackOfOpenElements[1].Name != "body")
                {
                    return;
                }
                if (!FramesetOkFlag)
                {
                    return;
                }
                else
                {
                    var parent = StackOfOpenElements[1].Parent;
                    if (parent != null)
                        parent.Children.Remove(StackOfOpenElements[1]);
                    while (CurrentNode.Name != "html")
                        PopCurrentNodeOffStackOfOpenElements();
                    var ele = CreateElementForToken(start);
                    InsertHtmlElement(ele);
                    InsertionMode = InFrameset;
                    return;
                }
            }
            if (t is EOFToken)
            {
                if (StackOfOpenElements.Any(e =>
                    !e.Name.IsOneOf("dd", "dt", "li", "p", "tdoby",
                    "td", "tfoot", "th", "thead", "tr", "body", "html")))
                {
                    ParseError(t);
                }
                if (StackOfTemplateInsertionModes.Count > 0)
                {
                    ApplyRule(InTemplate);
                    return;
                }
                else
                {
                    StopParsing();
                    return;
                }
            }
            if (end != null && end.Name == "body")
            {
                ReadNextToken();
                if (!HaveElementInScope("body"))
                {
                    ParseError(t);
                    return;
                }
                if (StackOfOpenElements.Any(e =>
                   !e.Name.IsOneOf("dd", "dt", "li", "optgroup", "option",
                   "p", "rb", "rp", "rt", "rtc", "tdoby", "td", "tfoot",
                   "th", "thead", "tr", "body", "html")))
                {
                    ParseError(t);
                }
                InsertionMode = AfterBody;
                return;
            }
            if (end != null && end.Name == "html")
            {
                ReadNextToken();
                if (!HaveElementInScope("body"))
                {
                    ParseError(t);
                    return;
                }
                if (StackOfOpenElements.Any(e =>
                   !e.Name.IsOneOf("dd", "dt", "li", "optgroup", "option",
                   "p", "rb", "rp", "rt", "rtc", "tdoby", "td", "tfoot",
                   "th", "thead", "tr", "body", "html")))
                {
                    ParseError(t);
                }
                InsertionMode = AfterBody;
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("address", "article", "blockquote",
                "center", "details", "dialog", "dir", "div", "dl",
                "fieldset", "figcaption", "figure", "footer", "header",
                "hgroup", "main", "nav", "ol", "p", "section",
                "summary", "ul"))
            {
                ReadNextToken();
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("h1", "h2", "h3", "h4", "h5", "h6"))
            {
                ReadNextToken();
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                if (CurrentNode.Name.IsOneOf("h1", "h2", "h3", "h4", "h5", "h6"))
                {
                    ParseError(t);
                    PopCurrentNodeOffStackOfOpenElements();
                }
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("pre", "listing"))
            {
                ReadNextToken();
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                var next = PeekNextToken() as CharacterToken;
                if (next != null && next.Char == '\u000A')
                    ReadNextToken();
                FramesetOkFlag = false;
                return;
            }
            if (start != null && start.Name == "form")
            {
                ReadNextToken();
                if (FormElementPointer != null &&
                    !StackOfOpenElements.Any(e => e.Name == "template"))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    if (HaveElementInButtonScope("p"))
                        ClosePElement();
                    var ele = CreateElementForToken(start);
                    InsertHtmlElement(ele);
                    if (!StackOfOpenElements.Any(e => e.Name == "template"))
                        FormElementPointer = ele;
                    return;
                }
            }
            if (start != null && start.Name == "li")
            {
                ReadNextToken();
                FramesetOkFlag = false;
                var node = CurrentNode;
                Loop:
                if (node.Name == "li")
                {
                    GenerateImpliedEndTags("li");
                    if (CurrentNode.Name != "li")
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "li");
                    goto Done;
                }
                if (!node.Name.IsOneOf("address", "div", "p") &&
                    SpecialCategory.Contains(node.Name))
                {
                    goto Done;
                }
                else
                {
                    node = StackOfOpenElements[StackOfOpenElements.IndexOf(node) - 1];
                    goto Loop;
                }
                Done:
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null && start.Name.IsOneOf("dd", "dt"))
            {
                ReadNextToken();
                FramesetOkFlag = false;
                var node = CurrentNode;
                Loop:
                if (node.Name == "dd")
                {
                    GenerateImpliedEndTags("dd");
                    if (CurrentNode.Name != "dd")
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "dd");
                    goto Done;
                }
                if (node.Name == "dt")
                {
                    GenerateImpliedEndTags("dt");
                    if (CurrentNode.Name != "dt")
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "dt");
                    goto Done;
                }
                if (!node.Name.IsOneOf("address", "div", "p") &&
                    SpecialCategory.Contains(node.Name))
                {
                    goto Done;
                }
                else
                {
                    node = StackOfOpenElements[StackOfOpenElements.IndexOf(node) - 1];
                    goto Loop;
                }
                Done:
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null && start.Name == "plaintext")
            {
                ReadNextToken();
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                Tokenizer.SwitchToPLAINTEXTState();
                return;
            }
            if (start != null && start.Name == "button")
            {
                ReadNextToken();
                if (HaveElementInScope("button"))
                {
                    ParseError(t);
                    GenerateImpliedEndTags();
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "button");
                }
                ReconstructActiveFormattingElements();
                var ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                FramesetOkFlag = false;
                return;
            }
            if (end != null &&
                end.Name.IsOneOf("address", "article", "aside",
                "blockquote", "button", "center", "details", "dialog",
                "dir", "div", "dl", "fieldset", "figcaption", "figure",
                "footer", "header", "hgroup", "listing", "main",
                "nav", "ol", "pre", "section", "summary", "ul"))
            {
                ReadNextToken();
                if (!HaveElementInScope(end.Name))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    GenerateImpliedEndTags();
                    if (CurrentNode.Name != end.Name)
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != end.Name);
                }
                return;
            }
            if (end != null && end.Name == "form")
            {
                ReadNextToken();
                if (!StackOfOpenElements.Any(e => e.Name == "template"))
                {
                    var node = FormElementPointer;
                    FormElementPointer = null;
                    if (node == null || !HaveElementInScope(node))
                    {
                        ParseError(t);
                        return;
                    }
                    GenerateImpliedEndTags();
                    if (CurrentNode != node)
                        ParseError(t);
                    StackOfOpenElements.Remove(node);
                }
                else
                {
                    if (!HaveElementInScope("form"))
                    {
                        ParseError(t);
                        return;
                    }
                    GenerateImpliedEndTags();
                    if (CurrentNode.Name != "form")
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "form");
                }
                return;
            }
            if (end != null && end.Name == "p")
            {
                ReadNextToken();
                if (!HaveElementInButtonScope("p"))
                {
                    ParseError(t);
                    InsertHtmlElement(new HtmlElement("p"));
                }
                ClosePElement();
            }
            if (end != null && end.Name == "li")
            {
                ReadNextToken();
                if (!HaveElementInListItemScope("li"))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    GenerateImpliedEndTags("li");
                    if (CurrentNode.Name != "li")
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "li");
                    return;
                }
            }
            if (end != null &&
                end.Name.IsOneOf("dd", "dt"))
            {
                ReadNextToken();
                if (!HaveElementInScope(end.Name))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    GenerateImpliedEndTags(end.Name);
                    if (CurrentNode.Name != end.Name)
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != end.Name);
                    return;
                }
            }
            if (end != null &&
                end.Name.IsOneOf("h1", "h2", "h3", "h4", "h5", "h6"))
            {
                ReadNextToken();
                if (!HaveElementInScope(end.Name))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    GenerateImpliedEndTags();
                    if (CurrentNode.Name != end.Name)
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (!last.Name.IsOneOf("h1", "h2", "h3", "h4", "h5", "h6"));
                    return;
                }
            }
            if (end != null && end.Name == "sarcsm")
            {
                goto AnyOtherEndTag;
            }
            if (start != null && start.Name == "a")
            {
                ReadNextToken();
                HtmlElement a;
                for (int i = ListOfActiveFormattingElements.Count - 1; i >= 0; i--)
                {
                    a = ListOfActiveFormattingElements[i];
                    if (a == ScopeMarker)
                        break;
                    if (a.Name == "a")
                    {
                        ParseError(t);
                        AdoptionAgency("a");
                        if (ListOfActiveFormattingElements.Contains(a))
                            ListOfActiveFormattingElements.Remove(a);
                        if (StackOfOpenElements.Contains(a))
                            StackOfOpenElements.Remove(a);
                        break;
                    }
                }
                ReconstructActiveFormattingElements();
                a = CreateElementForToken(start);
                InsertHtmlElement(a);
                PushOntoListOfActiveFormattingElements(a);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("b", "big", "code", "em", "font",
                "i", "s", "small", "strike", "strong", "tt", "u"))
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PushOntoListOfActiveFormattingElements(ele);
                return;
            }
            if (start != null && start.Name == "nobr")
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                if (HaveElementInScope("nobr"))
                {
                    ParseError(t);
                    AdoptionAgency("nobr");
                    ReconstructActiveFormattingElements();
                }
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PushOntoListOfActiveFormattingElements(ele);
                return;
            }
            if (end != null &&
                end.Name.IsOneOf("a", "b", "big", "code", "em", "font", "i",
                "nobr", "s", "small", "strike", "strong", "tt", "u"))
            {
                ReadNextToken();
                AdoptionAgency(end.Name);
                return;
            }
            if (start != null && start.Name.IsOneOf("applet", "marquee", "object"))
            {
                ReadNextToken();
                if (!HaveElementInScope(start.Name))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    GenerateImpliedEndTags();
                    if (CurrentNode.Name != start.Name)
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != end.Name);
                    ClearActiveFormattingElementsToLastMarker();
                    return;
                }
            }
            if (start != null && start.Name == "table")
            {
                ReadNextToken();
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                FramesetOkFlag = false;
                InsertionMode = InTable;
                return;
            }
            if (end != null && end.Name == "br")
            {
                ParseError(t);
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                FramesetOkFlag = false;
                return;
            }
            if (start != null && start.Name.IsOneOf("area", "br", "embed",
                "img", "keygen", "wbr"))
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                FramesetOkFlag = false;
                return;
            }
            if (start != null && start.Name == "input")
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                if (!start.Attributes.Any(a =>
                    a.Name == "type" && a.Value.ToLower() == "hidden"))
                    FramesetOkFlag = false;
                return;
            }
            if (start != null && start.Name.IsOneOf("param", "source", "track"))
            {
                ReadNextToken();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                return;
            }
            if (start != null && start.Name == "hr")
            {
                ReadNextToken();
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                FramesetOkFlag = false;
                return;
            }
            if (start != null && start.Name == "image")
            {
                ParseError(t);
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                FramesetOkFlag = false;
                return;
            }
            if (start != null && start.Name == "isindex")
            {
                ParseError(t);
                ReadNextToken();
                if (FormElementPointer != null &&
                    !StackOfOpenElements.Any(e => e.Name == "template"))
                    return;
                FramesetOkFlag = false;
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                HtmlElement form = new HtmlElement("form");
                InsertHtmlElement(form);
                if (!StackOfOpenElements.Any(e => e.Name == "template"))
                    FormElementPointer = form;
                TagTokenAttribute action = start.Attributes.FirstOrDefault(a => a.Name == "action");
                if (action != null)
                {
                    HtmlAttribute attr = form.Attributes.FirstOrDefault(a => a.Name == "action");
                    if (attr != null)
                        attr.Value = action.Value;
                    else
                        form.Attributes.Add(new HtmlAttribute("action", action.Value));
                }
                HtmlElement hr = new HtmlElement("hr");
                InsertHtmlElement(hr);
                PopCurrentNodeOffStackOfOpenElements();
                ReconstructActiveFormattingElements();
                HtmlElement label = new HtmlElement("label");
                InsertHtmlElement(label);
                TagTokenAttribute prompt = start.Attributes.FirstOrDefault(a => a.Name == "prompt");
                if (prompt != null)
                {
                    foreach (char ch in prompt.Value)
                        InsertCharacter(ch);
                }
                else
                {
                    foreach (char ch in "This is a searchable index. Enter search keywords:")
                        InsertCharacter(ch);
                }
                HtmlElement input = new HtmlElement("input");
                foreach (var attr in start.Attributes)
                {
                    if (!attr.Name.IsOneOf("name", "action", "prompt"))
                        input.Attributes.Add(new HtmlAttribute(attr.Name, attr.Value));
                }
                input.Attributes.Add(new HtmlAttribute("name", "isindex"));
                InsertHtmlElement(input);
                PopCurrentNodeOffStackOfOpenElements();
                PopCurrentNodeOffStackOfOpenElements();
                hr = new HtmlElement("hr");
                InsertHtmlElement(hr);
                PopCurrentNodeOffStackOfOpenElements();
                PopCurrentNodeOffStackOfOpenElements();
                if (!StackOfOpenElements.Any(e => e.Name == "template"))
                    FormElementPointer = null;
                return;
            }
            if (start != null && start.Name == "textarea")
            {
                ReadNextToken();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                var ch = PeekNextToken() as CharacterToken;
                if (ch != null && ch.Char == '\u000A')
                    ReadNextToken();
                Tokenizer.SwitchToRCDATAState();
                OriginalInsertionMode = InsertionMode;
                FramesetOkFlag = false;
                InsertionMode = Text;
                return;
            }
            if (start != null && start.Name == "xmp")
            {
                ReadNextToken();
                if (HaveElementInButtonScope("p"))
                    ClosePElement();
                ReconstructActiveFormattingElements();
                FramesetOkFlag = false;
                ParseGenericRawTextElement(start);
                return;
            }
            if (start != null && start.Name == "iframe")
            {
                ReadNextToken();
                FramesetOkFlag = false;
                ParseGenericRawTextElement(start);
                return;
            }
            if (start != null &&
                (start.Name == "noembed" ||
                (start.Name == "noscript" && ScriptingFlag)))
            {
                ReadNextToken();
                ParseGenericRawTextElement(start);
                return;
            }
            if (start != null && start.Name == "select")
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                FramesetOkFlag = false;
                if (InsertionMode == InTable ||
                    InsertionMode == InCaption ||
                    InsertionMode == InTableBody ||
                    InsertionMode == InRow ||
                    InsertionMode == InCell)
                    InsertionMode = InSelectInTable;
                else
                    InsertionMode = InSelect;
                return;
            }
            if (start != null && start.Name.IsOneOf("optgroup", "option"))
            {
                ReadNextToken();
                if (CurrentNode.Name == "option")
                    PopCurrentNodeOffStackOfOpenElements();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("rb", "rp", "rtc"))
            {
                ReadNextToken();
                if (CurrentNode.Name == "option")
                    PopCurrentNodeOffStackOfOpenElements();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null && start.Name == "rt")
            {
                ReadNextToken();
                if (HaveElementInScope("ruby"))
                {
                    GenerateImpliedEndTags();
                    if (CurrentNode.Name != "ruby")
                        ParseError(t);
                }
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null && start.Name == "math")
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                if (start.IsSelfClosing)
                    PopCurrentNodeOffStackOfOpenElements();
                return;
            }
            if (start != null && start.Name == "svg")
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                if (start.IsSelfClosing)
                    PopCurrentNodeOffStackOfOpenElements();
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("caption", "col", "colgroup",
                "frame", "head", "tbody", "td", "tfoot", "th", "thead", "tr"))
            {
                ReadNextToken();
                return;
            }
            if (start != null)
            {
                ReadNextToken();
                ReconstructActiveFormattingElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            AnyOtherEndTag:
            if (end != null)
            {
                ReadNextToken();
                HtmlElement node = CurrentNode;
                Loop:
                if (node.Name == end.Name)
                {
                    GenerateImpliedEndTags(end.Name);
                    if (node != CurrentNode)
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last != node);
                    return;
                }
                else if (SpecialCategory.Contains(node.Name))
                {
                    ParseError(t);
                    return;
                }
                node = StackOfOpenElements[StackOfOpenElements.IndexOf(node) - 1];
                goto Loop;
            }
        }

        private void ClosePElement()
        {
            GenerateImpliedEndTags("p");
            if (CurrentNode.Name != "p")
                ParseError($"Parse Error in {InsertionMode} insertion mode. (Close p element)");
            HtmlElement last;
            do
            {
                last = CurrentNode;
                PopCurrentNodeOffStackOfOpenElements();
            } while (last.Name != "p");
        }

        private void AdoptionAgency(string subject)
        {
            if (CurrentNode.Name == subject)
            {
                var ele = CurrentNode;
                PopCurrentNodeOffStackOfOpenElements();
                if (ListOfActiveFormattingElements.Contains(ele))
                    ListOfActiveFormattingElements.Remove(ele);
                return;
            }
            int outerLoopCounter = 0;
            OuterLoop:
            if (outerLoopCounter >= 8)
                return;
            outerLoopCounter++;
            HtmlElement formattingElement = null;
            for (int i = ListOfActiveFormattingElements.Count - 1; i >= 0; i--)
            {
                formattingElement = ListOfActiveFormattingElements[i];
                if (formattingElement == ScopeMarker)
                    break;
                if (formattingElement.Name == subject)
                {
                    break;
                }
                formattingElement = null;
            }
            if (formattingElement == null)
            {
                return;
            }
            if (!StackOfOpenElements.Contains(formattingElement))
            {
                ParseError($"Parse Error in {InsertionMode} insertion mode. (Adoption agency)");
                ListOfActiveFormattingElements.Remove(formattingElement);
                return;
            }
            if (!HaveElementInScope(formattingElement))
            {
                ParseError($"Parse Error in {InsertionMode} insertion mode. (Adoption agency)");
                return;
            }
            if (formattingElement != CurrentNode)
                ParseError($"Parse Error in {InsertionMode} insertion mode. (Adoption agency)");
            HtmlElement furthestBlock = null;
            for (int i = StackOfOpenElements.IndexOf(formattingElement) + 1; i < StackOfOpenElements.Count; i++)
            {
                if (SpecialCategory.Contains(StackOfOpenElements[i].Name))
                {
                    furthestBlock = StackOfOpenElements[i];
                    break;
                }
            }
            if (furthestBlock == null)
            {
                HtmlElement last;
                do
                {
                    last = CurrentNode;
                    PopCurrentNodeOffStackOfOpenElements();
                } while (last != formattingElement);
                return;
            }
            HtmlElement commonAncestor = StackOfOpenElements[StackOfOpenElements.IndexOf(formattingElement) - 1];
            int bookmarkIndex = ListOfActiveFormattingElements.IndexOf(formattingElement);
            HtmlElement bookmark;
            if (bookmarkIndex < ListOfActiveFormattingElements.Count - 1)
                bookmark = ListOfActiveFormattingElements[bookmarkIndex + 1];
            else
                bookmark = null;
            HtmlElement node = furthestBlock;
            HtmlElement lastNode = furthestBlock;
            HtmlElement aboveNode = null;
            {
                int innerLoopCounter = 0;
                InnerLoop:
                innerLoopCounter++;
                if (StackOfOpenElements.Contains(node))
                    node = StackOfOpenElements[StackOfOpenElements.IndexOf(node) - 1];
                else
                    node = aboveNode;
                if (node == formattingElement)
                    goto Step14;
                if (innerLoopCounter > 3 && ListOfActiveFormattingElements.Contains(node))
                    ListOfActiveFormattingElements.Remove(node);
                if (!ListOfActiveFormattingElements.Contains(node))
                {
                    aboveNode = StackOfOpenElements[StackOfOpenElements.IndexOf(node) - 1];
                    StackOfOpenElements.Remove(node);
                    goto InnerLoop;
                }
                HtmlElement newElement = new HtmlElement(node.Name);
                foreach (var attr in node.Attributes)
                    newElement.Attributes.Add(new HtmlAttribute(attr.Name, attr.Value));
                ListOfActiveFormattingElements[ListOfActiveFormattingElements.IndexOf(node)] = newElement;
                StackOfOpenElements[StackOfOpenElements.IndexOf(node)] = newElement;
                node = newElement;
                if (lastNode == furthestBlock)
                {
                    bookmarkIndex = ListOfActiveFormattingElements.IndexOf(node);
                    if (bookmarkIndex < ListOfActiveFormattingElements.Count - 1)
                        bookmark = ListOfActiveFormattingElements[bookmarkIndex + 1];
                    else
                        bookmark = null;
                }
                if (lastNode.Parent != null)
                    lastNode.Parent.Children.Remove(lastNode);
                node.Children.Add(lastNode);
                lastNode = node;
                goto InnerLoop;
            }
            Step14:
            if (lastNode.Parent != null)
                lastNode.Parent.Children.Remove(lastNode);
            InsertNodeAtAppropriatePlace(lastNode, commonAncestor);
            HtmlElement element = new HtmlElement(node.Name);
            foreach (var attr in node.Attributes)
                element.Attributes.Add(new HtmlAttribute(attr.Name, attr.Value));
            for (int i = furthestBlock.Children.Count - 1; i >= 0; i--)
            {
                var child = furthestBlock.Children[i];
                furthestBlock.Children.RemoveAt(i);
                element.Children.Add(child);
            }
            furthestBlock.Children.Add(element);
            ListOfActiveFormattingElements.Remove(formattingElement);
            if (bookmark == null)
            {
                ListOfActiveFormattingElements.Add(element);
                bookmark = element;
            }
            else
                ListOfActiveFormattingElements.Insert(ListOfActiveFormattingElements.IndexOf(bookmark), element);
            StackOfOpenElements.Remove(formattingElement);
            StackOfOpenElements.Insert(StackOfOpenElements.IndexOf(furthestBlock) + 1, element);
            goto OuterLoop;
        }

        private void ApplyRuleForTextInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null)
            {
                ReadNextToken();
                InsertCharacter(c.Char);
                return;
            }
            if (t is EOFToken)
            {
                ParseError(t);
                PopCurrentNodeOffStackOfOpenElements();
                InsertionMode = OriginalInsertionMode;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "script")
            {
                ReadNextToken();
                var script = CurrentNode;
                PopCurrentNodeOffStackOfOpenElements();
                InsertionMode = OriginalInsertionMode;
                return;
            }
            if (end != null)
            {
                ReadNextToken();
                PopCurrentNodeOffStackOfOpenElements();
                InsertionMode = OriginalInsertionMode;
                return;
            }
        }

        private List<CharacterToken> PendingTableCharacterTokens { get; } = new List<CharacterToken>();

        private void ApplyRuleForInTableInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                CurrentNode.Name.IsOneOf("table", "tbody", "tfoot", "thead", "tr"))
            {
                PendingTableCharacterTokens.Clear();
                OriginalInsertionMode = InsertionMode;
                InsertionMode = InTableText;
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            if (t is DOCTYPEToken)
            {
                ReadNextToken();
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "caption")
            {
                ReadNextToken();
                ClearStackBackToTableContext();
                ListOfActiveFormattingElements.Add(ScopeMarker);
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                InsertionMode = InCaption;
                return;
            }
            if (start != null && start.Name == "colgroup")
            {
                ReadNextToken();
                ClearStackBackToTableContext();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                InsertionMode = InColumnGroup;
                return;
            }
            if (start != null && start.Name == "column")
            {
                ClearStackBackToTableContext();
                InsertHtmlElement(new HtmlElement("colgroup"));
                InsertionMode = InColumnGroup;
                return;
            }
            if (start != null && start.Name.IsOneOf("tbody", "tfoot", "thead"))
            {
                ReadNextToken();
                ClearStackBackToTableContext();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                InsertionMode = InTableBody;
                return;
            }
            if (start != null && start.Name.IsOneOf("td", "th", "tr"))
            {
                ClearStackBackToTableContext();
                InsertHtmlElement(new HtmlElement("tbody"));
                InsertionMode = InTableBody;
                return;
            }
            if (start != null && start.Name == "table")
            {
                ParseError(t);
                if (!HaveElementInTableScope("table"))
                {
                    ReadNextToken();
                    return;
                }
                else
                {
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "table");
                    ResetInsertionModeAppropriately();
                    return;
                }
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "table")
            {
                ReadNextToken();
                if (!HaveElementInTableScope("table"))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "table");
                    ResetInsertionModeAppropriately();
                    return;
                }
            }
            if (end != null &&
                end.Name.IsOneOf("body", "caption", "col", "colgroup", "html",
                "tbody", "td", "tfoot", "th", "thead", "tr"))
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("style", "script", "template"))
            {
                ApplyRule(InHead);
                return;
            }
            if (end != null && end.Name == "template")
            {
                ApplyRule(InHead);
                return;
            }
            if (start != null && start.Name == "input")
            {
                if (!start.Attributes.Any(a =>
                    a.Name == "type" && a.Value.ToLower() == "hidden"))
                    goto AnythingElse;
                else
                {
                    ReadNextToken();
                    ParseError(t);
                    HtmlElement ele = CreateElementForToken(start);
                    InsertHtmlElement(ele);
                    PopCurrentNodeOffStackOfOpenElements();
                    return;
                }
            }
            if (start != null && start.Name == "form")
            {
                ReadNextToken();
                ParseError(t);
                if (FormElementPointer != null ||
                    StackOfOpenElements.Any(e => e.Name == "template"))
                    return;
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                FormElementPointer = ele;
                PopCurrentNodeOffStackOfOpenElements();
                return;

            }
            if (t is EOFToken)
            {
                ApplyRule(InBody);
                return;
            }
            AnythingElse:
            {
                ParseError(t);
                FosterParenting = true;
                ApplyRule(InBody);
                FosterParenting = false;
                return;
            }
        }

        private void ClearStackBackToTableContext()
        {
            while (!CurrentNode.Name.IsOneOf("table", "template", "html"))
                PopCurrentNodeOffStackOfOpenElements();
        }

        private void ApplyRuleForInTableTextInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null && c.Char == '\0')
            {
                ReadNextToken();
                return;
            }
            if (c != null)
            {
                ReadNextToken();
                PendingTableCharacterTokens.Add(c);
                return;
            }
            else
            {
                if (PendingTableCharacterTokens.Any(ch =>
                    !ch.Char.IsOneOf('\u0020', '\u0009', '\u000A', '\u000C', '\u000D')))
                {
                    ParseError(t);
                    FosterParenting = true;
                    foreach (var ch in PendingTableCharacterTokens)
                    {

                        if (ch != null && ch.Char == '\0')
                        {
                            ParseError(t);
                            continue;
                        }
                        if (ch != null &&
                            ch.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
                        {
                            ReconstructActiveFormattingElements();
                            InsertCharacter(ch.Char);
                            continue;
                        }
                        if (ch != null)
                        {
                            ReconstructActiveFormattingElements();
                            InsertCharacter(ch.Char);
                            FramesetOkFlag = false;
                            continue;
                        }
                    }
                    FosterParenting = false;
                }
                else
                {
                    foreach (var ch in PendingTableCharacterTokens)
                    {
                        InsertCharacter(ch.Char);
                    }
                }
                InsertionMode = OriginalInsertionMode;
                return;
            }
        }

        private void ApplyRuleForInCaptionInsertionMode()
        {
            var t = PeekNextToken();
            var end = t as EndTagToken;
            if (end != null && end.Name == "caption")
            {
                ReadNextToken();
                if (!HaveElementInTableScope("caption"))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    GenerateImpliedEndTags();
                    if (CurrentNode.Name != "caption")
                        ParseError(t);
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "caption");
                    ClearActiveFormattingElementsToLastMarker();
                    InsertionMode = InTable;
                    return;
                }
            }
            var start = t as StartTagToken;
            if ((start != null &&
                 start.Name.IsOneOf("caption", "col", "colgroup", "tbody",
                 "td", "tfoot", "th", "thead", "tr")) ||
                (end != null && end.Name == "table"))
            {
                ParseError(t);
                if (!HaveElementInTableScope("caption"))
                {
                    ReadNextToken();
                    return;
                }
                else
                {
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "caption");
                    ClearActiveFormattingElementsToLastMarker();
                    InsertionMode = InTable;
                    return;
                }
            }
            if (end != null &&
                end.Name.IsOneOf("body", "col", "colgroup", "html", "tbody",
                "td", "tfoot", "th", "thead", "tr"))
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            else
            {
                ApplyRule(InBody);
                return;
            }
        }

        private void ApplyRuleForInColumnGroupInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                InsertCharacter(c.Char);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            if (t is DOCTYPEToken)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (start != null && start.Name == "col")
            {
                ReadNextToken();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "colgroup")
            {
                ReadNextToken();
                if (CurrentNode.Name != "colgroup")
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    PopCurrentNodeOffStackOfOpenElements();
                    InsertionMode = InTable;
                    return;
                }
            }
            if (end != null && end.Name == "col")
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            if (start != null && start.Name == "template")
            {
                ApplyRule(InHead);
                return;
            }
            if (end != null && end.Name == "template")
            {
                ApplyRule(InBody);
                return;
            }
            else
            {
                if (CurrentNode.Name != "colgroup")
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    PopCurrentNodeOffStackOfOpenElements();
                }
                InsertionMode = InTable;
                return;
            }
        }

        private void ApplyRuleForInTableBodyInsertionMode()
        {
            var t = PeekNextToken();
            var start = t as StartTagToken;
            if (start != null && start.Name == "tr")
            {
                ReadNextToken();
                ClearStackBackToTableBodyContext();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                InsertionMode = InRow;
                return;
            }
            if (start != null && start.Name.IsOneOf("th", "td"))
            {
                ParseError(t);
                ClearStackBackToTableBodyContext();
                InsertHtmlElement(new HtmlElement("tr"));
                InsertionMode = InRow;
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name.IsOneOf("tbody", "tfoot", "thead"))
            {
                ReadNextToken();
                if (!HaveElementInTableScope(end.Name))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    ClearStackBackToTableBodyContext();
                    PopCurrentNodeOffStackOfOpenElements();
                    InsertionMode = InTable;
                    return;
                }
            }
            if ((start != null &&
                 start.Name.IsOneOf("caption", "col", "colgroup", "tbody", "tfoot", "thead")) ||
                (end != null && end.Name == "table"))
            {
                if (!HaveElementInTableScope("tbody") &&
                    !HaveElementInTableScope("thead") &&
                    !HaveElementInTableScope("tfoot"))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                else
                {
                    ClearStackBackToTableBodyContext();
                    PopCurrentNodeOffStackOfOpenElements();
                    InsertionMode = InTable;
                    return;
                }
            }
            if (end != null &&
                end.Name.IsOneOf("body", "caption", "col", "colgroup", "html", "td", "th", "tr"))
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            else
            {
                ApplyRule(InTable);
                return;
            }
        }

        private void ClearStackBackToTableBodyContext()
        {
            while (!CurrentNode.Name.IsOneOf("tbody", "tfoot", "thead", "template", "html"))
                PopCurrentNodeOffStackOfOpenElements();
        }

        private void ApplyRuleForInRowInsertionMode()
        {
            var t = PeekNextToken();
            var start = t as StartTagToken;
            if (start != null && start.Name.IsOneOf("th", "td"))
            {
                ReadNextToken();
                ClearStackBackToTableRowContext();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                InsertionMode = InCell;
                ListOfActiveFormattingElements.Add(ScopeMarker);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "tr")
            {
                ReadNextToken();
                if (!HaveElementInTableScope("tr"))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    ClearStackBackToTableRowContext();
                    PopCurrentNodeOffStackOfOpenElements();
                    InsertionMode = InTableBody;
                    return;
                }
            }

            if ((start != null &&
                start.Name.IsOneOf("caption", "col", "colgroup", "tbody", "tfoot", "thead", "tr")) ||
                (end != null && end.Name == "table"))
            {
                if (!HaveElementInTableScope("tr"))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                else
                {
                    ClearStackBackToTableRowContext();
                    PopCurrentNodeOffStackOfOpenElements();
                    InsertionMode = InTableBody;
                    return;
                }
            }
            if (end != null && end.Name.IsOneOf("tbody", "tfoot", "thead"))
            {
                if (!HaveElementInTableScope(end.Name))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                if (!HaveElementInTableScope("tr"))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                else
                {
                    ClearStackBackToTableRowContext();
                    PopCurrentNodeOffStackOfOpenElements();
                    InsertionMode = InTableBody;
                    return;
                }
            }
            if (end != null &&
                end.Name.IsOneOf("body", "caption", "col", "colgroup", "html", "td", "th"))
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            else
            {
                ApplyRule(InTable);
                return;
            }
        }

        private void ClearStackBackToTableRowContext()
        {
            while (!CurrentNode.Name.IsOneOf("tr", "template", "html"))
                PopCurrentNodeOffStackOfOpenElements();
        }

        private void ApplyRuleForInCellInsertionMode()
        {
            var t = PeekNextToken();
            var end = t as EndTagToken;
            if (end != null && end.Name.IsOneOf("td", "th"))
            {
                if (!HaveElementInTableScope(end.Name))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                GenerateImpliedEndTags();
                if (CurrentNode.Name != end.Name)
                    ParseError(t);
                HtmlElement last;
                do
                {
                    last = CurrentNode;
                    PopCurrentNodeOffStackOfOpenElements();
                } while (last.Name != end.Name);
                ClearActiveFormattingElementsToLastMarker();
                InsertionMode = InRow;
                return;
            }
            var start = t as StartTagToken;
            if (start != null &&
                start.Name.IsOneOf("caption", "col", "colgroup", "tbody", "td", "tfoot", "th", "thead", "tr"))

            {
                if (!HaveElementInTableScope("tr") &&
                    !HaveElementInTableScope("th"))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                else
                {
                    CloseCell();
                    return;
                }
            }
            if (end != null && end.Name.IsOneOf("body", "caption", "col", "colgroup", "html"))
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            if (end != null && end.Name.IsOneOf("table", "tbody", "tfoot", "thead", "tr"))
            {
                if (!HaveElementInTableScope(end.Name))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                else
                {
                    CloseCell();
                    return;
                }
            }
            else
            {
                ApplyRule(InBody);
                return;
            }
        }

        private void CloseCell()
        {
            GenerateImpliedEndTags();
            if (!CurrentNode.Name.IsOneOf("td", "th"))
                ParseError($"Parse Error in {InsertionMode} insertion mode. (Close cell)");
            HtmlElement last;
            do
            {
                last = CurrentNode;
                PopCurrentNodeOffStackOfOpenElements();
            } while (!last.Name.IsOneOf("td", "th"));
            ClearActiveFormattingElementsToLastMarker();
            InsertionMode = InRow;
        }

        private void ApplyRuleForInSelectInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null && c.Char == '\0')
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            if (c != null)
            {
                ReadNextToken();
                InsertCharacter(c.Char);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            if (t is DOCTYPEToken)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (start != null && start.Name == "option")
            {
                ReadNextToken();
                if (CurrentNode.Name == "option")
                    PopCurrentNodeOffStackOfOpenElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            if (start != null && start.Name == "optgroup")
            {
                ReadNextToken();
                if (CurrentNode.Name == "option")
                    PopCurrentNodeOffStackOfOpenElements();
                if (CurrentNode.Name == "optgroup")
                    PopCurrentNodeOffStackOfOpenElements();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "optgroup")
            {
                ReadNextToken();
                if (CurrentNode.Name == "option" &&
                    StackOfOpenElements[StackOfOpenElements.Count - 2].Name == "optgroup")
                {
                    PopCurrentNodeOffStackOfOpenElements();
                }
                if (CurrentNode.Name == "optgroup")
                {
                    PopCurrentNodeOffStackOfOpenElements();
                }
                else
                {
                    ParseError(t);
                }
                return;
            }
            if (end != null && end.Name == "option")
            {
                ReadNextToken();
                if (CurrentNode.Name == "option")
                {
                    PopCurrentNodeOffStackOfOpenElements();
                }
                else
                {
                    ParseError(t);
                }
                return;
            }
            if (end != null && end.Name == "select")
            {
                ReadNextToken();
                if (!HaveElementInSelectScope("select"))
                {
                    ParseError(t);
                    return;
                }
                else
                {
                    HtmlElement last;
                    do
                    {
                        last = CurrentNode;
                        PopCurrentNodeOffStackOfOpenElements();
                    } while (last.Name != "select");
                }
                ResetInsertionModeAppropriately();
                return;
            }
            if (start != null && start.Name.IsOneOf("input", "keygen", "textarea"))
            {
                ParseError(t);
                if (!HaveElementInSelectScope("select"))
                {
                    ReadNextToken();
                    return;
                }
                HtmlElement last;
                do
                {
                    last = CurrentNode;
                    PopCurrentNodeOffStackOfOpenElements();
                } while (last.Name != "select");
                ResetInsertionModeAppropriately();
                return;
            }
            if ((start != null && start.Name.IsOneOf("script", "template")) ||
                (end != null && end.Name == "template"))
            {
                ApplyRule(InHead);
                return;
            }
            if (t is EOFToken)
            {
                ApplyRule(InBody);
                return;
            }
            else
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
        }

        private void ApplyRuleForInSelectInTableInsertionMode()
        {
            var t = PeekNextToken();
            var start = t as StartTagToken;
            if (start != null &&
                start.Name.IsOneOf("caption", "table", "tbody", "tfoot", "thead", "tr", "td", "th"))
            {
                ParseError(t);
                HtmlElement last;
                do
                {
                    last = CurrentNode;
                    PopCurrentNodeOffStackOfOpenElements();
                } while (last.Name != "select");
                ResetInsertionModeAppropriately();
                return;
            }
            var end = t as EndTagToken;
            if (end != null &&
                end.Name.IsOneOf("caption", "table", "tbody", "tfoot", "thead", "tr", "td", "th"))
            {
                ParseError(t);
                if (!HaveElementInTableScope(end.Name))
                {
                    ReadNextToken();
                    ParseError(t);
                    return;
                }
                HtmlElement last;
                do
                {
                    last = CurrentNode;
                    PopCurrentNodeOffStackOfOpenElements();
                } while (last.Name != "select");
                ResetInsertionModeAppropriately();
                return;
            }
            else
            {
                ApplyRule(InSelect);
                return;
            }
        }

        private void ApplyRuleForInTemplateInsertionMode()
        {
            var t = PeekNextToken();
            if (t is CharacterToken ||
                t is CommentToken ||
                t is DOCTYPEToken)
            {
                ApplyRule(InBody);
                return;
            }
            var start = t as StartTagToken;
            if (start != null &&
                start.Name.IsOneOf("base", "basefont", "bgsound", "link", "meta",
                "noframes", "script", "style", "template", "title"))
            {
                ApplyRule(InHead);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "template")
            {
                ApplyRule(InHead);
                return;
            }
            if (start != null &&
                start.Name.IsOneOf("caption", "colgroup", "tbody", "tfoot", "thead"))
            {
                StackOfTemplateInsertionModes.Pop();
                StackOfTemplateInsertionModes.Push(InTable);
                InsertionMode = InTable;
                return;
            }
            if (start != null && start.Name == "col")
            {
                StackOfTemplateInsertionModes.Pop();
                StackOfTemplateInsertionModes.Push(InColumnGroup);
                InsertionMode = InColumnGroup;
                return;
            }
            if (start != null && start.Name == "tr")
            {
                StackOfTemplateInsertionModes.Pop();
                StackOfTemplateInsertionModes.Push(InTableBody);
                InsertionMode = InTableBody;
                return;
            }
            if (start != null &&
              start.Name.IsOneOf("td", "th"))
            {
                StackOfTemplateInsertionModes.Pop();
                StackOfTemplateInsertionModes.Push(InRow);
                InsertionMode = InRow;
                return;
            }
            if (start != null)
            {
                StackOfTemplateInsertionModes.Pop();
                StackOfTemplateInsertionModes.Push(InBody);
                InsertionMode = InBody;
                return;
            }
            if (end != null)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            if (t is EOFToken)
            {
                if (!StackOfOpenElements.Any(e => e.Name == "template"))
                {
                    StopParsing();
                    return;
                }
                ParseError(t);
                HtmlElement last;
                do
                {
                    last = CurrentNode;
                    PopCurrentNodeOffStackOfOpenElements();
                } while (last.Name != "template");
                ClearActiveFormattingElementsToLastMarker();
                StackOfTemplateInsertionModes.Pop();
                ResetInsertionModeAppropriately();
                return;
            }
        }

        private void ApplyRuleForAfterBodyInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ApplyRule(InBody);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                StackOfOpenElements.First().Children.Add(new HtmlComment(com.Data));
                return;
            }
            if (t is DOCTYPEToken)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "html")
            {
                ReadNextToken();
                InsertionMode = AfterAfterBody;
                return;
            }
            if (t is EOFToken)
            {
                StopParsing();
                return;
            }
            else
            {
                ParseError(t);
                InsertionMode = InBody;
                return;
            }
        }

        private void ApplyRuleForInFramesetInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                InsertCharacter(c.Char);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            if (t is DOCTYPEToken)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (start != null && start.Name == "frameset")
            {
                ReadNextToken();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "frameset")
            {
                ReadNextToken();
                if (StackOfOpenElements.Count == 1 && CurrentNode.Name == "html")
                {
                    ParseError(t);
                    return;
                }
                PopCurrentNodeOffStackOfOpenElements();
                if (CurrentNode.Name != "frameset")
                    InsertionMode = AfterFrameset;
                return;
            }
            if (start != null && start.Name == "frame")
            {
                ReadNextToken();
                HtmlElement ele = CreateElementForToken(start);
                InsertHtmlElement(ele);
                PopCurrentNodeOffStackOfOpenElements();
                return;
            }
            if (start != null && start.Name == "noframes")
            {
                ApplyRule(InHead);
                return;
            }
            if (t is EOFToken)
            {
                if (StackOfOpenElements.Count != 1 && CurrentNode.Name != "html")
                {
                    ParseError(t);
                }
                StopParsing();
                return;
            }
            else
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
        }

        private void ApplyRuleForAfterFramesetInsertionMode()
        {
            var t = PeekNextToken();
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ReadNextToken();
                InsertCharacter(c.Char);
                return;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                InsertComment(com.Data);
                return;
            }
            if (t is DOCTYPEToken)
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            var end = t as EndTagToken;
            if (end != null && end.Name == "html")
            {
                ReadNextToken();
                InsertionMode = AfterAfterFrameset;
                return;
            }
            if (start != null && start.Name == "noframes")
            {
                ApplyRule(InHead);
                return;
            }
            if (t is EOFToken)
            {
                StopParsing();
                return;
            }
            else
            {
                ReadNextToken();
                ParseError(t);
                return;
            }
        }

        private void ApplyRuleForAfterAfterBodyInsertionMode()
        {
            var t = PeekNextToken();
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                Document.Children.Add(new HtmlComment(com.Data));
                return;
            }
            if (t is DOCTYPEToken)
            {
                ApplyRule(InBody);
                return;
            }
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ApplyRule(InBody);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (t is EOFToken)
            {
                StopParsing();
                return;
            }
            else
            {
                ParseError(t);
                InsertionMode = InBody;
                return;
            }
        }

        private void ApplyRuleForAfterAfterFramesetInsertionMode()
        {
            var t = PeekNextToken();
            var com = t as CommentToken;
            if (com != null)
            {
                ReadNextToken();
                Document.Children.Add(new HtmlComment(com.Data));
                return;
            }
            if (t is DOCTYPEToken)
            {
                ApplyRule(InBody);
                return;
            }
            var c = t as CharacterToken;
            if (c != null &&
                c.Char.IsOneOf('\u0009', '\u000A', '\u000C', '\u000D', '\u0020'))
            {
                ApplyRule(InBody);
                return;
            }
            var start = t as StartTagToken;
            if (start != null && start.Name == "html")
            {
                ApplyRule(InBody);
                return;
            }
            if (t is EOFToken)
            {
                StopParsing();
                return;
            }
            if (start != null && start.Name == "noframes")
            {
                ApplyRule(InHead);
                return;
            }
            else
            {
                ParseError(t);
                InsertionMode = InBody;
                return;
            }
        }

        #endregion
    }
}
