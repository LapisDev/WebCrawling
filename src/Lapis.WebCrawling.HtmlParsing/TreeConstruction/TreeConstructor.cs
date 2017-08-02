/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : TreeConstructor
 * Description : Provides methods for construction of HTML trees.
 * Created     : 2015/8/25
 * Note        : http://www.w3.org/TR/html5/syntax.html
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.Tokenization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.TreeConstruction
{
    /// <summary>
    /// rovides methods for construction of HTML trees.
    /// </summary>
    public partial class TreeConstructor
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="TreeConstructor"/> class with
        ///   a string to parse.
        /// </summary>
        /// <param name="html">A string to parse.</param>
        public TreeConstructor(string html)
        {
            Tokenizer = new Tokenizer(html);
            Document = new HtmlDocument();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TreeConstructor"/> class with
        ///   the specified parameters.
        /// </summary>
        /// <param name="html">A string to parse.</param>
        /// <param name="isStrictMode">
        ///   A value that indicates whether the <see cref="TreeConstructor"/> throws an
        ///   exception when an error occurs.
        /// </param>
        public TreeConstructor(string html, bool isStrictMode)
            : this(html)
        {
            IsStrictMode = isStrictMode;
        }
        
        /// <summary>
        /// Parses the HTML and returns the DOM tree.
        /// </summary>
        /// <returns>The HTML tree constructed.</returns>
        /// <exception cref="HtmlException">
        ///   This exception is thrown when an error occurs if 
        ///   <see cref="IsStrictMode"/> is set <see langword="true"/>.
        /// </exception>
        public HtmlDocument Parse()
        {
            while (!IsEnd)
            {
                ApplyRule(InsertionMode);
            }
            RemoveWhitespaceTextNode(Document.Children);
            return Document;
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="TreeConstructor"/> throws 
        ///   an exception when an error occurs.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the <see cref="TreeConstructor"/> throws an
        ///   exception when an error occurs; otherwise, <see langword="true"/>.
        /// </value>
        public bool IsStrictMode { get; }

        private void RemoveWhitespaceTextNode(HtmlNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count;)
            {
                var text = nodes[i] as HtmlText;
                if (text != null)
                {
                    if (string.IsNullOrWhiteSpace(text.Value))
                    {
                        nodes.RemoveAt(i);
                        continue;
                    }
                    else
                    {
                        text.Value = text.Value.Trim();
                    }
                }
                var container = nodes[i] as HtmlContainer;
                if (container != null)
                {
                    RemoveWhitespaceTextNode(container.Children);
                }
                i += 1;
            }
        }


        private Tokenizer Tokenizer { get; }

        private Token PeekNextToken()
        {
            return Tokenizer.Peek();
        }

        private Token ReadNextToken()
        {
            return Tokenizer.Read();
        }

        
        private HtmlDocument Document { get; set; }


        #region 8.2.3.1 The insertion mode        

        private InsertionMode InsertionMode { get; set; }

        private InsertionMode OriginalInsertionMode { get; set; }

        private Stack<InsertionMode> StackOfTemplateInsertionModes { get; } = new Stack<InsertionMode>();

        private InsertionMode CurrentTemplateInsertionMode
        {
            get { return StackOfTemplateInsertionModes.Peek(); }
        }

        private void ResetInsertionModeAppropriately()
        {
            bool last = false;
            var node = StackOfOpenElements.Last();
            Loop:
            if (StackOfOpenElements.IndexOf(node) == 0)
                last = true;
            if (node.Name == "select")
            {
                if (last)
                    goto Done;
                var ancestor = node;
                Loop1:
                int index = StackOfOpenElements.IndexOf(node);
                if (index == 0)
                    goto Done;
                ancestor = StackOfOpenElements[index - 1];
                if (ancestor.Name == "template")
                    goto Done;
                if (ancestor.Name == "table")
                {
                    InsertionMode = InsertionMode.InSelectInTable;
                    return;
                }
                goto Loop1;
                Done:
                InsertionMode = InsertionMode.InSelect;
                return;
            }
            if ((node.Name == "td" || node.Name == "th") && !last)
            {
                InsertionMode = InsertionMode.InCell;
                return;
            }
            if (node.Name == "tr")
            {
                InsertionMode = InsertionMode.InRow;
                return;
            }
            if (node.Name == "tbody" || node.Name == "thead" || node.Name == "tfoot")
            {
                InsertionMode = InsertionMode.InTableBody;
                return;
            }
            if (node.Name == "caption")
            {
                InsertionMode = InsertionMode.InCaption;
                return;
            }
            if (node.Name == "colgroup")
            {
                InsertionMode = InsertionMode.InColumnGroup;
                return;
            }
            if (node.Name == "table")
            {
                InsertionMode = InsertionMode.InTable;
                return;
            }
            if (node.Name == "template")
            {
                InsertionMode = CurrentTemplateInsertionMode;
                return;
            }
            if (node.Name == "head")
            {
                if (last)
                    InsertionMode = InsertionMode.InBody;
                else
                    InsertionMode = InsertionMode.InHead;
                return;
            }
            if (node.Name == "body")
            {
                InsertionMode = InsertionMode.InBody;
                return;
            }
            if (node.Name == "frameset")
            {
                InsertionMode = InsertionMode.InFrameset;
                return;
            }
            if (node.Name == "html")
            {
                if (HeadElementPointer == null)
                    InsertionMode = InsertionMode.BeforeHead;
                else
                    InsertionMode = InsertionMode.AfterHead;
                return;
            }
            if (last)
            {
                InsertionMode = InsertionMode.InBody;
                return;
            }
            node = StackOfOpenElements[StackOfOpenElements.IndexOf(node) - 1];
            goto Loop;
        }

        #endregion


        #region 8.2.3.2 The stack of open elements

        private List<HtmlElement> StackOfOpenElements { get; } = new List<HtmlElement>();

        private void PopCurrentNodeOffStackOfOpenElements()
        {
            StackOfOpenElements.RemoveAt(StackOfOpenElements.Count - 1);
        }

        private HtmlElement CurrentNode { get { return StackOfOpenElements.Last(); } }

        private HtmlElement AdjustedCurrentNode
        {
            get
            {
                return CurrentNode;
            }
        }

        private static readonly string[] SpecialCategory = new string[90]
        {
            #region
            "address", "applet", "area", "article", "aside",
            "base", "basefont", "bgsound", "blockquote", "body",
            "br", "button", "caption", "center", "col",
            "colgroup", "dd", "details", "dir", "div",
            "dl", "dt", "embed", "fieldset", "figcaption",
            "figure", "footer", "form", "frame", "frameset",
            "h1", "h2", "h3", "h4", "h5",
            "h6", "head", "header", "hgroup", "hr",
            "html", "iframe", " img", "input", "isindex",
            "li", "link", "listing", "main", "marquee",
            "meta", "nav", "noembed", "noframes", "noscript",
            "object", "ol", "p", "param", "plaintext",
            "pre", "script", "section", "select", "source",
            "style", "summary", "table", "tbody", "td",
            "template", "textarea", "tfoot", "th", "thead",
            "title", "tr", "track", "ul", "wbr",
            "xmp", "mi", "mo", "mn", "ms",
            "mtext", "annotation-xml", "foreignObject", "desc", "title"
            #endregion
        };

        private static readonly string[] FormattingCategory = new string[14]
        {
            #region
            "a", "b", "big", "code", "em",
            "font", "i", "nobr", "s", "small",
            "strike", "strong","tt", "u"
             #endregion
        };

        private bool HaveElementInScope(string name)
        {
            HtmlElement node;
            for (int i = StackOfOpenElements.Count - 1; i >= 0; i--)
            {
                node = StackOfOpenElements[i];
                if (node.Name == name)
                    return true;
                if (_inScopeLists.Contains(node.Name))
                    return false;
            }
            return false;
        }

        private bool HaveElementInScope(HtmlElement element)
        {
            HtmlElement node;
            for (int i = StackOfOpenElements.Count - 1; i >= 0; i--)
            {
                node = StackOfOpenElements[i];
                if (node == element)
                    return true;
                if (_inScopeLists.Contains(node.Name))
                    return false;
            }
            return false;
        }

        private bool HaveElementInListItemScope(string name)
        {
            HtmlElement node;
            for (int i = StackOfOpenElements.Count - 1; i >= 0; i--)
            {
                node = StackOfOpenElements[i];
                if (node.Name == name)
                    return true;
                if (node.Name == "ol" || node.Name == "ul" ||
                    _inScopeLists.Contains(node.Name))
                    return false;
            }
            return false;
        }

        private bool HaveElementInButtonScope(string name)
        {
            HtmlElement node;
            for (int i = StackOfOpenElements.Count - 1; i >= 0; i--)
            {
                node = StackOfOpenElements[i];
                if (node.Name == name)
                    return true;
                if (node.Name == "button" ||
                    _inScopeLists.Contains(node.Name))
                    return false;
            }
            return false;
        }

        private static readonly string[] _inScopeLists = new string[18]
        {
            #region
            "applet", "caption", "html", "table", "td",
            "th", "marquee", "object", "template", "mi",
            "mo", "mn", "ms", "mtext", "annotation-xml",
            "foreignObject", "desc", "title"
            #endregion
        };

        private bool HaveElementInTableScope(string name)
        {
            HtmlElement node;
            for (int i = StackOfOpenElements.Count - 1; i >= 0; i--)
            {
                node = StackOfOpenElements[i];
                if (node.Name == name)
                    return true;
                if (node.Name == "html" ||
                    node.Name == "table" ||
                    node.Name == "template")
                    return false;
            }
            return false;
        }

        private bool HaveElementInSelectScope(string name)
        {
            HtmlElement node;
            for (int i = StackOfOpenElements.Count - 1; i >= 0; i--)
            {
                node = StackOfOpenElements[i];
                if (node.Name == name)
                    return true;
                if (node.Name == "optgroup" ||
                    node.Name == "option")
                    return false;
            }
            return false;
        }

        #endregion


        #region 8.2.3.3 The list of active formatting elements

        private List<HtmlElement> ListOfActiveFormattingElements { get; } = new List<HtmlElement>();

        private static readonly HtmlElement ScopeMarker = Marker.Instance;
        private class Marker : HtmlElement
        {
            public static readonly Marker Instance = new Marker();
            private Marker() : base("ScopeMarker") { }
        }

        private void PushOntoListOfActiveFormattingElements(HtmlElement element)
        {
            var dict = new Dictionary<HtmlElement, int>(HtmlElementEqualityComparer.Default);
            for (int i = ListOfActiveFormattingElements.Count - 1; i >= 0; i--)
            {
                var e = ListOfActiveFormattingElements[i];
                if (e == ScopeMarker)
                    break;
                if (dict.ContainsKey(e))
                {
                    if (dict[e] == 2)
                        ListOfActiveFormattingElements.RemoveAt(i);
                    else
                        dict[e]++;
                }
                else
                    dict.Add(e, 1);
            }
            ListOfActiveFormattingElements.Add(element);
        }
        private class HtmlElementEqualityComparer : EqualityComparer<HtmlElement>
        {
            public new static readonly HtmlElementEqualityComparer Default = new HtmlElementEqualityComparer();

            public override bool Equals(HtmlElement x, HtmlElement y)
            {
                if (x.Name != y.Name)
                    return false;
                if (x.Children.Count != y.Children.Count)
                    return false;
                return x.Attributes.SequenceEqual(y.Attributes, HtmlAttributeEqualityComparer.Default);
            }
            public override int GetHashCode(HtmlElement obj)
            {
                return obj.Name.GetHashCode() ^ obj.Attributes.GetHashCode();
            }
        }
        private class HtmlAttributeEqualityComparer : EqualityComparer<HtmlAttribute>
        {
            public new static readonly HtmlAttributeEqualityComparer Default = new HtmlAttributeEqualityComparer();

            public override bool Equals(HtmlAttribute x, HtmlAttribute y)
            {
                return x.Name == y.Name && x.Value == y.Value;
            }

            public override int GetHashCode(HtmlAttribute obj)
            {
                return obj.Name.GetHashCode() ^ obj.Value.GetHashCode();
            }
        }

        private void ReconstructActiveFormattingElements()
        {
            if (ListOfActiveFormattingElements.Count == 0)
                return;
            var last = ListOfActiveFormattingElements.Last();
            if (last == ScopeMarker ||
                StackOfOpenElements.Contains(last))
                return;
            HtmlElement entry = ListOfActiveFormattingElements.Last();
            Rewind:
            int index = ListOfActiveFormattingElements.IndexOf(entry);
            if (index == 0)
                goto Create;
            entry = ListOfActiveFormattingElements[index - 1];
            if (entry != ScopeMarker && !StackOfOpenElements.Contains(entry))
                goto Rewind;
            Advance:
            index = ListOfActiveFormattingElements.IndexOf(entry) + 1;
            entry = ListOfActiveFormattingElements[index];
            Create:
            var newElement = new HtmlElement(entry.Name);
            foreach (var attr in entry.Attributes)
                newElement.Attributes.Add(new HtmlAttribute(attr.Name, attr.Value));
            InsertHtmlElement(newElement);
            ListOfActiveFormattingElements[index] = newElement;
            entry = newElement;
            if (index < ListOfActiveFormattingElements.Count - 1)
                goto Advance;
        }

        private void ClearActiveFormattingElementsToLastMarker()
        {
            HtmlElement entry;
            for (int i = ListOfActiveFormattingElements.Count - 1; i >= 0; i--)
            {
                entry = ListOfActiveFormattingElements[i];
                ListOfActiveFormattingElements.RemoveAt(i);
                if (entry == ScopeMarker)
                    break;
            }
        }

        #endregion


        #region 8.2.3.4 The element pointers

        private HtmlElement HeadElementPointer { get; set; } = null;

        private HtmlElement FormElementPointer { get; set; } = null;

        #endregion


        #region 8.2.3.5 Other parsing state flags

        private bool ScriptingFlag { get; set; } = true;  // true for "enabled", false for "disabled".

        private bool FramesetOkFlag { get; set; } = true; // true for "ok", false for "not ok".

        #endregion




        #region 8.2.5.1 Creating and inserting nodes

        private bool FosterParenting { get; set; } = false;

        private void InsertNodeAtAppropriatePlace(HtmlNode node, HtmlElement overrideTarget = null)
        {
            HtmlElement target;
            if (overrideTarget != null)
                target = overrideTarget;
            else
                target = CurrentNode;
            if (FosterParenting &&
               (target.Name == "table" ||
                target.Name == "tbody" ||
                target.Name == "tfoot" ||
                target.Name == "thead" ||
                target.Name == "tr"))
            {
                var lastTemplate = StackOfOpenElements.LastOrDefault(e => e.Name == "template");
                var lastTable = StackOfOpenElements.LastOrDefault(e => e.Name == "table");
                if (lastTemplate != null)
                {
                    if (lastTable == null ||
                        StackOfOpenElements.LastIndexOf(lastTemplate) > StackOfOpenElements.LastIndexOf(lastTable))
                    {
                        lastTemplate.Children.Add(node);
                        return;
                    }
                }
                if (lastTable == null)
                {
                    var html = StackOfOpenElements.First();
                    html.Children.Add(node);
                    return;
                }
                if (lastTable.Parent != null)
                {
                    var index = lastTable.Parent.Children.IndexOf(lastTable);
                    lastTable.Parent.Children.Insert(index, node);
                    return;
                }
                var previousElement = StackOfOpenElements.ElementAtOrDefault(StackOfOpenElements.LastIndexOf(lastTable) - 1);
                previousElement.Children.Add(node);
            }
            else
            {
                target.Children.Add(node);
                return;
            }
        }

        private HtmlElement CreateElementForToken(StartTagToken token)
        {
            var element = new HtmlElement(token.Name);
            foreach (var attr in token.Attributes)
            {
                element.Attributes.Add(new HtmlAttribute(attr.Name, attr.Value));
            }
            return element;
        }

        private void InsertHtmlElement(HtmlElement element)
        {
            InsertNodeAtAppropriatePlace(element);
            StackOfOpenElements.Add(element);
        }

        private void InsertCharacter(char c)
        {
            var data = c;
            var target = StackOfOpenElements.Last();
            if (target.Name == "table" ||
                target.Name == "tbody" ||
                target.Name == "tfoot" ||
                target.Name == "thead" ||
                target.Name == "tr")
            {
                var lastTemplate = StackOfOpenElements.LastOrDefault(e => e.Name == "template");
                var lastTable = StackOfOpenElements.LastOrDefault(e => e.Name == "table");
                if (lastTemplate != null)
                {
                    if (lastTable == null ||
                        StackOfOpenElements.LastIndexOf(lastTemplate) > StackOfOpenElements.LastIndexOf(lastTable))
                    {
                        var previous = lastTemplate.Children.LastOrDefault() as HtmlText;
                        if (previous != null)
                            previous.Value += c;
                        else
                            lastTemplate.Children.Add(new HtmlText(c.ToString()));
                        return;
                    }
                }
                if (lastTable == null)
                {
                    var html = StackOfOpenElements.First();
                    var previous = html.Children.LastOrDefault() as HtmlText;
                    if (previous != null)
                        previous.Value += c;
                    else
                        html.Children.Add(new HtmlText(c.ToString()));
                    return;
                }
                if (lastTable.Parent != null)
                {
                    var index = lastTable.Parent.Children.IndexOf(lastTable);
                    var previous = lastTable.Parent.Children.ElementAtOrDefault(index - 1) as HtmlText;
                    if (previous != null)
                        previous.Value += c;
                    else
                        lastTable.Parent.Children.Insert(index, new HtmlText(c.ToString()));
                    return;
                }
                var previousElement = StackOfOpenElements.ElementAtOrDefault(StackOfOpenElements.LastIndexOf(lastTable) - 1);
                var text = previousElement.Children.LastOrDefault() as HtmlText;
                if (text != null)
                    text.Value += c;
                else
                    previousElement.Children.Add(new HtmlText(c.ToString()));
            }
            else
            {
                var text = target.Children.LastOrDefault() as HtmlText;
                if (text != null)
                    text.Value += c;
                else
                    target.Children.Add(new HtmlText(c.ToString()));
                return;
            }
        }

        private void InsertComment(string comment)
        {
            HtmlComment com = new HtmlComment(comment);
            InsertNodeAtAppropriatePlace(com);
        }

        #endregion


        #region 8.2.5.2 Parsing elements that contain only text

        private void ParseGenericRawTextElement(StartTagToken token)
        {
            var element = CreateElementForToken(token);
            InsertHtmlElement(element);
            Tokenizer.SwitchToRAWTEXTState();
            OriginalInsertionMode = InsertionMode;
            InsertionMode = InsertionMode.Text;
        }

        private void ParseGenericRCDATAElement(StartTagToken token)
        {
            var element = CreateElementForToken(token);
            InsertHtmlElement(element);
            Tokenizer.SwitchToRCDATAState();
            OriginalInsertionMode = InsertionMode;
            InsertionMode = InsertionMode.Text;
        }

        #endregion


        #region 8.2.5.3 Closing elements that have implied end tags

        private void GenerateImpliedEndTags(params string[] excluding)
        {
            HtmlElement currentNode;
            currentNode = StackOfOpenElements.Last();
            if (excluding.Contains(currentNode.Name))
                return;
            if (currentNode.Name == "dd" ||
                currentNode.Name == "dt" ||
                currentNode.Name == "li" ||
                currentNode.Name == "option" ||
                currentNode.Name == "optgroup" ||
                currentNode.Name == "p" ||
                currentNode.Name == "rb" ||
                currentNode.Name == "rt" ||
                currentNode.Name == "rtc")
            {
                PopCurrentNodeOffStackOfOpenElements();
            }
        }

        #endregion



        private void ParseError(Token t)
        {
            string message;
            var c = t as CharacterToken;
            if (c != null)
            {
                message = $"Character Token '{c.Char}' in {InsertionMode} insertion mode. ";
                goto Done;
            }
            var com = t as CommentToken;
            if (com != null)
            {
                message = $"Comment Token \"{com.Data}\" in {InsertionMode} insertion mode. ";
                goto Done;
            }
            var doctype = t as DOCTYPEToken;
            if (doctype != null)
            {
                message = $"DOCTYPE Token in {InsertionMode} insertion mode. ";
                goto Done;
            }
            var start = t as StartTagToken;
            if (start != null)
            {
                message = $"Start tag Token \"{start.Name}\" in {InsertionMode} insertion mode. ";
                goto Done;
            }
            var end = t as EndTagToken;
            if (end != null)
            {
                message = $"End tag Token \"{end.Name}\" in {InsertionMode} insertion mode. ";
                goto Done;
            }
            var eof = t as EOFToken;
            if (eof != null)
            {
                message = $"EOF Token in {InsertionMode} insertion mode. ";
                goto Done;
            }
            message = $"Token \"{t}\" in {InsertionMode} insertion mode. ";
            Done:
            if (IsStrictMode)
                throw new HtmlException(t.Location,
                    ExceptionResource.ParserError + message);
            else
                System.Diagnostics.Debug.WriteLine(t.Location + ": " + message);

        }
        private void ParseError(string message)
        {
            if (IsStrictMode)
                throw new HtmlException(Tokenizer.Peek().Location,
                    ExceptionResource.ParserError + message);
            else
                System.Diagnostics.Debug.WriteLine(Tokenizer.Peek().Location + ": " + message);
        }


        private bool IsEnd { get; set; } = false;

        private void StopParsing()
        {
            StackOfOpenElements.Clear();
            IsEnd = true;
        }
    }
}

