/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : CssSelector
 * Description : Represents a css selector.
 * Created     : 2016/1/2
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.CssSelectors
{
    /// <summary>
    /// Represents a css selector. This class is abstract.
    /// </summary>
    public abstract class CssSelector
    {
        /// <summary>
        /// Parses a string of a css selector and returns the result.
        /// </summary>  
        /// <param name="s">The string to parse.</param>
        /// <returns>A css selector.</returns>
        public static CssSelector Parse(string s)
        {
            var parser = new Parser(s);
            return parser.Parse();
        }

        /// <summary>
        /// Initial a new instance of the <see cref="CssSelector"/> class.
        /// </summary>
        protected CssSelector() { }

        /// <summary>
        /// Selects the specified HTML nodes and returns the result.
        /// </summary>
        /// <param name="nodes">The nodes to select.</param>
        /// <returns>The selected nodes.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public abstract IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes);

        /// <summary>
        /// Returns the string representation of the <see cref="CssSelector"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="CssSelector"/> object.
        /// </returns>
        public abstract override string ToString();
    }

    /// <summary>
    /// Represents a type selector. 
    /// </summary>
    public class TypeSelector : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="TypeSelector"/> class using
        ///   the specified tag name.
        /// </summary>
        /// <param name="tagName">The tag name.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public TypeSelector(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                throw new ArgumentException(nameof(tagName));
            TagName = tagName;
        }

        /// <summary>
        /// Gets the tag name.
        /// </summary>
        /// <value>The tag name.</value>
        public string TagName { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return TagName;
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            return nodes.OfType<HtmlElement>()
                .Where(e => e.Name.Equals(TagName, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    /// <summary>
    /// Represents a universal selector, which matches any node.
    /// </summary>
    public class UniversalSelector : CssSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalSelector"/> class.
        /// </summary>
        public UniversalSelector() { }

        /// <summary>
        /// Gets a instance of the <see cref="UniversalSelector"/> class.
        /// </summary>
        /// <value>A <see cref="UniversalSelector"/> object.</value>
        public static readonly UniversalSelector Instance = new UniversalSelector();

        /// <inheritdoc/>
        public override string ToString()
        {
            return "*";
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            return nodes;
        }
    }

    /// <summary>
    /// Represents an attribute selector.
    /// </summary>
    public class AttributeSelector : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="AttributeSelector"/> class with
        ///   the specified attribute name.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public AttributeSelector(string attribute)
        {
            if (string.IsNullOrWhiteSpace(attribute))
                throw new ArgumentException(nameof(attribute));
            Attribute = attribute;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AttributeSelector"/> class with
        ///   the specified attribute name and value.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public AttributeSelector(string attribute, string value)
            : this(attribute)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AttributeSelector"/> class with
        ///   the specified attribute name and value.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        /// <param name="type">The type of the attribute selector.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public AttributeSelector(string attribute, string value, AttributeSelectorType type)
            : this(attribute)
        {
            if (type == AttributeSelectorType.Presence)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(value));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(value));
                Value = value;
                Type = type;
            }
        }

        /// <summary>
        /// Gets the attribute name.
        /// </summary>
        /// <value>The attribute name.</value>
        public string Attribute { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (Type)
            {
                case AttributeSelectorType.Presence:
                    return "[" + Attribute + "]";
                case AttributeSelectorType.Equal:
                    return "[" + Attribute + "=\"" + Value + "\"]";
                case AttributeSelectorType.ContainWord:
                    return "[" + Attribute + "~=\"" + Value + "\"]";
                case AttributeSelectorType.StartWithWord:
                    return "[" + Attribute + "|=\"" + Value + "\"]";
                case AttributeSelectorType.StartWith:
                    return "[" + Attribute + "^=\"" + Value + "\"]";
                case AttributeSelectorType.EndWith:
                    return "[" + Attribute + "$=\"" + Value + "\"]";
                case AttributeSelectorType.Contain:
                    return "[" + Attribute + "*=\"" + Value + "\"]";
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <value>The attribute value.</value>
        public string Value { get; }

        /// <summary>
        /// Gets the type of the attribute selector.
        /// </summary>
        /// <value>The type of the attribute selector.</value>
        public AttributeSelectorType Type { get; }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            switch (Type)
            {
                case AttributeSelectorType.Presence:
                    return nodes.Where(node => node.Attribute(Attribute) != null);
                case AttributeSelectorType.Equal:
                    return nodes.Where(node => node.Attribute(Attribute)?.EqualsValue(Value) ?? false);
                case AttributeSelectorType.ContainWord:
                    return nodes.Where(node => node.Attribute(Attribute)?.ContainsWord(Value) ?? false);
                case AttributeSelectorType.StartWithWord:
                    return nodes.Where(node => node.Attribute(Attribute)?.StartsWithWord(Value) ?? false);
                case AttributeSelectorType.StartWith:
                    return nodes.Where(node => node.Attribute(Attribute)?.StartsWith(Value) ?? false);
                case AttributeSelectorType.EndWith:
                    return nodes.Where(node => node.Attribute(Attribute)?.EndsWith(Value) ?? false);
                case AttributeSelectorType.Contain:
                    return nodes.Where(node => node.Attribute(Attribute)?.Contains(Value) ?? false);
                default:
                    throw new Exception();
            }
        }
    }

    /// <summary>
    /// Represents the type of the attribute selector.
    /// </summary>
    public enum AttributeSelectorType
    {
        /// <summary>
        ///   The value indicates that the selector selects the attributes with the 
        ///   specified name.
        /// </summary>
        Presence,
        /// <summary>
        ///   The value indicates that the selector selects the attributes with the 
        ///   specified name and value.
        /// </summary>
        Equal,
        /// <summary>
        ///   The value indicates that the selector selects the attributes that is
        ///   of the specified name and contains the specified word.
        /// </summary>
        ContainWord,
        /// <summary>
        ///   The value indicates that the selector selects the attributes with is 
        ///   of the specified name and starts with the specified word.
        /// </summary>
        StartWithWord,
        /// <summary>
        ///   The value indicates that the selector selects the attributes with is 
        ///   of the specified name and starts with the specified value.
        /// </summary>
        StartWith,
        /// <summary>
        ///   The value indicates that the selector selects the attributes with is 
        ///   of the specified name and ends with the specified value.
        /// </summary>
        EndWith,
        /// <summary>
        ///   The value indicates that the selector selects the attributes with is 
        ///   of the specified name and contains the specified value.
        /// </summary>
        Contain
    }

    /// <summary>
    /// Represents a <c>class</c> selector. 
    /// </summary>
    public class ClassSelector : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="ClassSelector"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="class">The name of the class.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public ClassSelector(string @class)
        {
            if (string.IsNullOrWhiteSpace(@class))
                throw new ArgumentException(nameof(@class));
            Class = @class;
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string Class { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "." + Class;
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            return nodes.Where(node => node.ClassList()
            .Any(c => c.Equals(Class, StringComparison.CurrentCultureIgnoreCase)));
        }
    }

    /// <summary>
    /// Represents a <c>id</c> selector. 
    /// </summary>
    /// <remarks>The <c>id</c> is case sensetive.</remarks>
    public class IdSelector : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="IdSelector"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="id">The value of the <c>id</c> attribute.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public IdSelector(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException(nameof(id));
            Id = id;
        }

        /// <summary>
        /// Gets the value of the <c>id</c> attribute.
        /// </summary>
        /// <value>The value of the <c>id</c> attribute.</value>
        public string Id { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "#" + Id;
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            return nodes.Where(node => Id.Equals(node.Id()));
        }
    }

    /// <summary>
    /// Represents a selector group. 
    /// </summary>
    public class SelectorGroup : CssSelector, IEnumerable<CssSelector>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="SelectorGroup"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="selectors">A collection of css selectors.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or empty.
        /// </exception>
        public SelectorGroup(IEnumerable<CssSelector> selectors)
        {
            if (selectors == null)
                throw new ArgumentNullException(nameof(selectors));
            else if (selectors.Count() <= 1 || selectors.Contains(null))
                throw new ArgumentException(nameof(selectors));
            this._selectors = selectors.ToList();
        }

        /// <inheritdoc cref="SelectorGroup.SelectorGroup(IEnumerable{CssSelector})"/>
        public SelectorGroup(params CssSelector[] selectors)
            : this(selectors.AsEnumerable())
        { }

        internal SelectorGroup(List<CssSelector> selectors)
        {
            this._selectors = selectors;
        }

        private List<CssSelector> _selectors { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var sb = new StringBuilder();
            bool isFirst = true;
            foreach (var selector in _selectors)
            {
                if (isFirst)
                {
                    sb.Append(",");
                    isFirst = false;
                }
                sb.Append(selector.ToString());

            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the selector ar the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the selector to get.</param>
        /// <returns>The selector ar the index <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is less than 0, or greater than or equal to 
        ///   <see cref="Count"/>.
        /// </exception>
        public CssSelector this[int index]
        {
            get { return _selectors[index]; }
        }

        /// <summary>
        /// Gets the number of selectors.
        /// </summary>
        /// <value>The number of selectors.</value>
        public int Count
        {
            get { return _selectors.Count(); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the selectors.
        /// </summary>
        /// <returns>An enumerator for the selectors.</returns>
        public IEnumerator<CssSelector> GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }

        /// <inheritdoc cref="SelectorGroup.GetEnumerator"/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            IEnumerable<HtmlNode> result = Enumerable.Empty<HtmlNode>();
            foreach (var selector in _selectors)
            {
                result = result.Union(selector.Select(nodes));
            }
            return result;
        }
    }

    /// <summary>
    /// Represents a selector sequence.
    /// </summary>
    public class SelectorSequence : CssSelector, IEnumerable<CssSelector>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="SelectorSequence"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="selectors">A sequence of selectors.</param>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or empty.
        /// </exception>
        public SelectorSequence(IEnumerable<CssSelector> selectors)
        {
            if (selectors == null)
                throw new ArgumentNullException(nameof(selectors));
            else if (selectors.Count() <= 1 || selectors.Contains(null))
                throw new ArgumentException(nameof(selectors));
            this._selectors = selectors.ToList();
        }

        /// <inheritdoc cref="SelectorSequence.SelectorSequence(IEnumerable{CssSelector})"/>
        public SelectorSequence(params CssSelector[] selectors)
            : this(selectors.AsEnumerable())
        { }

        internal SelectorSequence(List<CssSelector> selectors)
        {
            this._selectors = selectors;
        }

        private List<CssSelector> _selectors { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var selector in _selectors)
            {
                sb.Append(selector.ToString());

            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the selector ar the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the selector to get.</param>
        /// <returns>The selector ar the index <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is less than 0, or greater than or equal to 
        ///   <see cref="Count"/>.
        /// </exception>
        public CssSelector this[int index]
        {
            get { return _selectors[index]; }
        }

        /// <summary>
        /// Gets the number of selectors.
        /// </summary>
        /// <value>The number of selectors.</value>
        public int Count
        {
            get { return _selectors.Count(); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the selectors.
        /// </summary>
        /// <returns>An enumerator for the selectors.</returns>
        public IEnumerator<CssSelector> GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }

        /// <inheritdoc cref="SelectorSequence.GetEnumerator"/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            IEnumerable<HtmlNode> result = nodes;
            foreach (var selector in _selectors)
            {
                result = selector.Select(result);
            }
            return result;
        }
    }

    /// <summary>
    /// Represents the combination of two selectors. 
    /// </summary>
    public class SelectorCombinator : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="SelectorCombinator"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="left">The first selector.</param>
        /// <param name="right">The second selector.</param>
        /// <param name="type">The type of a selector combinator.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public SelectorCombinator(CssSelector left, CssSelector right, SelectorCombinatorType type)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            Left = left;
            Right = right;
            Type = type;
        }

        /// <summary>
        /// Gets the first selector.
        /// </summary>
        /// <value>The first selector.</value>
        public CssSelector Left { get; }

        /// <summary>
        /// Gets the second selector.
        /// </summary>
        /// <value>The second selector.</value>
        public CssSelector Right { get; }

        /// <summary>
        /// Gets the type of a selector combinator.
        /// </summary>
        /// <value>The type of a selector combinator.</value>
        public SelectorCombinatorType Type { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Type == SelectorCombinatorType.Descendant)
                return Left.ToString() + " " + Right.ToString();
            else if (Type == SelectorCombinatorType.Child)
                return Left.ToString() + ">" + Right.ToString();
            else if (Type == SelectorCombinatorType.AdjacentSibling)
                return Left.ToString() + "+" + Right.ToString();
            else if (Type == SelectorCombinatorType.GeneralSibling)
                return Left.ToString() + "~" + Right.ToString();
            else
                throw new Exception();
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            if (Type == SelectorCombinatorType.Descendant)
            {
                var leftResult = Left.Select(nodes);
                IEnumerable<HtmlNode> result = Enumerable.Empty<HtmlNode>();
                foreach (var node in leftResult)
                {
                    result = result.Union(Right.Select(node.Descendants()));
                }
                return result;
            }
            else if (Type == SelectorCombinatorType.Child)
            {
                var leftResult = Left.Select(nodes);
                IEnumerable<HtmlNode> result = Enumerable.Empty<HtmlNode>();
                foreach (var node in leftResult)
                {
                    result = result.Union(Right.Select(node.Children()));
                }
                return result;
            }
            else if (Type == SelectorCombinatorType.AdjacentSibling)
            {
                var leftResult = Left.Select(nodes);
                IEnumerable<HtmlNode> result = Enumerable.Empty<HtmlNode>();
                foreach (var node in leftResult)
                {
                    var nextSibling = node.NextSibling();
                    if (nextSibling != null)
                        result = result.Union(Right.Select(new HtmlNode[] { nextSibling }));
                }
                return result;
            }
            else if (Type == SelectorCombinatorType.GeneralSibling)
            {
                var leftResult = Left.Select(nodes);
                IEnumerable<HtmlNode> result = Enumerable.Empty<HtmlNode>();
                foreach (var node in leftResult)
                {
                    result = result.Union(Right.Select(node.Siblings()
                        .SkipWhile(n => n != node.NextSibling())));
                }
                return result;
            }
            else
                throw new Exception();
        }
    }

    /// <summary>
    /// Represents the type of a selector combinator.
    /// </summary>
    public enum SelectorCombinatorType
    {
        /// <summary>
        ///   The value indicates that the combined selector selects the 
        ///   descendant nodes that match the second selector of the parent
        ///   nodes that match the first selector.
        /// </summary>
        Descendant,
        /// <summary>
        ///   The value indicates that the combined selector selects the 
        ///   child nodes that match the second selector of the parent
        ///   nodes that match the first selector.
        /// </summary>
        Child,
        /// <summary>
        ///   The value indicates that the combined selector selects the 
        ///   adjacent sibling nodes that match the second selector of the 
        ///   previous nodes that match the first selector.
        /// </summary>
        AdjacentSibling,
        /// <summary>
        ///   The value indicates that the combined selector selects the 
        ///   sibling nodes that match the second selector of the previous
        ///   nodes that match the first selector.
        /// </summary>
        GeneralSibling
    }

    /// <summary>
    /// Represents a pseudo class selector. 
    /// </summary>
    public class PseudoClassSelector : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="PseudoClassSelector"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="type">The type of the pseudo class selector.</param>
        public PseudoClassSelector(PseudoClassSelectorType type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type of the pseudo class selector.
        /// </summary>
        /// <value>The type of the pseudo class selector.</value>
        public PseudoClassSelectorType Type { get; }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            if (Type == PseudoClassSelectorType.FirstChild)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>().FirstOrDefault() == n;
                });
            else if (Type == PseudoClassSelectorType.LastChild)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>().LastOrDefault() == n;
                });
            else if (Type == PseudoClassSelectorType.OnlyChild)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>().Count() == 1;
                });
            else if (Type == PseudoClassSelectorType.FirstOfType)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>().FirstOrDefault(e => e.Name == n.Name) == n;
                });
            else if (Type == PseudoClassSelectorType.LastOfType)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>().LastOrDefault(e => e.Name == n.Name) == n;
                });
            else if (Type == PseudoClassSelectorType.OnlyOfType)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>().Count(e => e.Name == n.Name) == 1;
                });
            else if (Type == PseudoClassSelectorType.Root)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    return n.Parent is HtmlDocument;
                });
            else if (Type == PseudoClassSelectorType.Empty)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    return n.Children.Count == 0;
                });
            else
                throw new Exception();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (Type)
            {
                case PseudoClassSelectorType.FirstChild:
                    return ":first-child";
                case PseudoClassSelectorType.LastChild:
                    return ":last-child";
                case PseudoClassSelectorType.OnlyChild:
                    return ":only-child";
                case PseudoClassSelectorType.FirstOfType:
                    return ":first-of-type";
                case PseudoClassSelectorType.LastOfType:
                    return ":last-of-type";
                case PseudoClassSelectorType.OnlyOfType:
                    return ":only-of-type";
                case PseudoClassSelectorType.Root:
                    return ":root";
                case PseudoClassSelectorType.Empty:
                    return ":empty";
                default:
                    throw new Exception();
            }
        }
    }

    /// <summary>
    /// Represents the type of a pseudo class selector.
    /// </summary>
    public enum PseudoClassSelectorType
    {
        /// <summary>
        ///   The value indicates that the selector selects the nodes that are the first
        ///   child node of the parent node.
        /// </summary>
        FirstChild,
        /// <summary>
        ///   The value indicates that the selector selects the nodes that are the last
        ///   child node of the parent node.
        /// </summary>
        LastChild,
        /// <summary>
        ///   The value indicates that the selector selects the nodes that are the only
        ///   child node of the parent node.
        /// </summary>
        OnlyChild,
        /// <summary>
        ///   The value indicates that the selector selects the nodes that are the first
        ///   child node of the specified type of the parent node.
        /// </summary>
        FirstOfType,
        /// <summary>
        ///   The value indicates that the selector selects the nodes that are the last
        ///   child node of the specified type of the parent node.
        /// </summary>
        LastOfType,
        /// <summary>
        ///   The value indicates that the selector selects the nodes that are the only
        ///   child node of the specified type of the parent node.
        /// </summary>
        OnlyOfType,
        /// <summary>
        /// The value indicates that the selector selects the root node.
        /// </summary>
        Root,
        /// <summary>
        ///   The value indicates that the selector selects the nodes that have no
        ///   child nodes.
        /// </summary>
        Empty
    }

    /// <summary>
    /// Represents a nth pseudo class selector. 
    /// </summary>
    public class PseudoClassNthSelector : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="PseudoClassNthSelector"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="type">The type of a nth pseudo class selector.</param>
        /// <param name="period">The period (a of an + b).</param>
        /// <param name="offset">The offset (b of an + b).</param>
        public PseudoClassNthSelector(PseudoClassNthSelectorType type, int period, int offset)
        {
            Type = type;
            Period = period;
            Offset = offset;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PseudoClassNthSelector"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="type">The type of a nth pseudo class selector.</param>
        /// <param name="offset">The offset (b of an + b).</param>
        public PseudoClassNthSelector(PseudoClassNthSelectorType type, int offset)
            : this(type, 0, offset)
        {
        }

        /// <summary>
        /// Gets the period (a of an + b).
        /// </summary>
        /// <value>The period.</value>
        public int Period { get; }

        /// <summary>
        /// Gets the offset (b of an + b).
        /// </summary>
        /// <value>The offset.</value>
        public int Offset { get; }

        /// <summary>
        /// Gets the type of a nth pseudo class selector.
        /// </summary>
        /// <value>The type of a nth pseudo class selector.</value>
        public PseudoClassNthSelectorType Type { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            string parameter;
            if (Period == 0)
                parameter = Offset.ToString();
            else if (Period == 2 && Offset == 0)
                parameter = "even";
            else if (Period == 2 && Offset == 1)
                parameter = "odd";
            else if (Period == 1)
            {
                if (Offset == 0)
                    parameter = "n";
                else if (Offset < 0)
                    parameter = "n" + Offset.ToString();
                else
                    parameter = "n+" + Offset.ToString();
            }
            else if (Period == -1)
            {
                if (Offset == 0)
                    parameter = "-n";
                else if (Offset < 0)
                    parameter = "-n" + Offset.ToString();
                else
                    parameter = "-n+" + Offset.ToString();
            }
            else
            {
                if (Offset == 0)
                    parameter = Period.ToString() + "n";
                else if (Offset < 0)
                    parameter = Period.ToString() + "n" + Offset.ToString();
                else
                    parameter = Period.ToString() + "n+" + Offset.ToString();
            }
            switch (Type)
            {
                case PseudoClassNthSelectorType.NthChild:
                    return $":nth-child({parameter})";
                case PseudoClassNthSelectorType.NthLastChild:
                    return $":nth-last-child({parameter})";
                case PseudoClassNthSelectorType.NthOfType:
                    return $":nth-of-type({parameter})";
                case PseudoClassNthSelectorType.NthLastOfType:
                    return $":nth-last-of-type({parameter})";
                default:
                    throw new Exception();
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            if (Type == PseudoClassNthSelectorType.NthChild)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>()
                    .Where((e, i) =>
                    {
                        i += 1;
                        if (Period == 0)
                            return i == Offset;
                        else
                            return (i - Offset) % Period == 0 &&
                            (i - Offset) / Period >= 0;
                    }).Contains(n);
                });
            else if (Type == PseudoClassNthSelectorType.NthLastChild)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>().Reverse()
                    .Where((e, i) =>
                    {
                        i += 1;
                        if (Period == 0)
                            return i == Offset;
                        else
                            return (i - Offset) % Period == 0 &&
                            (i - Offset) / Period >= 0;
                    }).Contains(n);
                });
            else if (Type == PseudoClassNthSelectorType.NthOfType)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>()
                    .Where(e => e.Name == n.Name)
                    .Where((e, i) =>
                    {
                        i += 1;
                        if (Period == 0)
                            return i == Offset;
                        else
                            return (i - Offset) % Period == 0 &&
                            (i - Offset) / Period >= 0;
                    }).Contains(n);
                });
            else if (Type == PseudoClassNthSelectorType.NthLastOfType)
                return nodes.OfType<HtmlElement>().Where(n =>
                {
                    var parent = n.Parent;
                    if (parent == null)
                        return false;
                    return parent.Children.OfType<HtmlElement>()
                    .Where(e => e.Name == n.Name)
                    .Reverse()
                    .Where((e, i) =>
                    {
                        i += 1;
                        if (Period == 0)
                            return i == Offset;
                        else
                            return (i - Offset) % Period == 0 &&
                            (i - Offset) / Period >= 0;
                    }).Contains(n);
                });
            else
                throw new Exception();
        }
    }

    /// <summary>
    /// Represents the type of a nth pseudo class selector.
    /// </summary>
    public enum PseudoClassNthSelectorType
    {
        /// <summary>
        ///   The value indicates that the selector selects the nth child nodes 
        ///   of their parent nodes.
        /// </summary>
        NthChild,
        /// <summary>
        ///   The value indicates that the selector selects the last nth child nodes 
        ///   of their parent nodes.
        /// </summary>
        NthLastChild,
        /// <summary>
        ///   The value indicates that the selector selects the nth child nodes of 
        ///   the type of the node.
        /// </summary>
        NthOfType,
        /// <summary>
        ///   The value indicates that the selector selects the last nth child nodes of 
        ///   the type of the node.
        /// </summary>
        NthLastOfType
    }

    /// <summary>
    /// Represents a <c>not</c> selector. 
    /// </summary>
    public class PseudoClassNegationSelector : CssSelector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="PseudoClassNegationSelector"/> class with 
        ///   the specified parameter.
        /// </summary>
        /// <param name="argument">The inner selector.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public PseudoClassNegationSelector(CssSelector argument)
        {
            if (argument == null)
                throw new ArgumentNullException(nameof(argument));
            Argument = argument;
        }

        /// <summary>
        /// Gets the inner selector.
        /// </summary>
        /// <value>The inner selector.</value>
        public CssSelector Argument { get; }

        /// <inheritdoc/>
        public override IEnumerable<HtmlNode> Select(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null || nodes.Contains(null))
                throw new ArgumentNullException(nameof(nodes));
            return nodes.Except(Argument.Select(nodes));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"not({Argument})";
        }
    }
}
