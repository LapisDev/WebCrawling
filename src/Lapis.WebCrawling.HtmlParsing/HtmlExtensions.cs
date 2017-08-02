/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlExtensions
 * Description : Provides extension methods for HTML DOM.
 * Created     : 2016/1/2
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing
{
    /// <summary>
    /// Provides extension methods for HTML DOM.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Returns the sibling nodes of this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   An <see cref="IEnumerable{HtmlNode}"/> of the sibling nodes of this node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> Siblings(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node.Parent != null)
                return node.Parent.Children.Where(child => node != child);
            else
                return Enumerable.Empty<HtmlNode>();
        }

        /// <summary>
        /// Returns the sibling nodes of this node filtered by a predicate.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   An <see cref="IEnumerable{HtmlNode}"/> of the sibling nodes that satisfy 
        ///   the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> Siblings(this HtmlNode node, Func<HtmlNode, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            return node.Siblings().Where(predicate);
        }

        /// <summary>
        /// Returns the descendant nodes of this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   An <see cref="IEnumerable{HtmlNode}"/> of the descendant nodes of this node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> Descendants(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            var element = node as HtmlContainer;
            if (element != null)
            {
                foreach (var child in element.Children)
                {
                    foreach (var t in child.AsEnumerable())
                    {
                        yield return t;
                    }
                }
            }
        }

        /// <summary>
        /// Returns this node and its descendant nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   An <see cref="IEnumerable{HtmlNode}"/> of this node and its the 
        ///   descendant nodes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> AsEnumerable(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            yield return node;
            var element = node as HtmlContainer;
            if (element != null)
            {
                foreach (var child in element.Children)
                {
                    foreach (var t in child.AsEnumerable())
                    {
                        yield return t;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the descendant nodes of this node filtered by a predicate.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   An <see cref="IEnumerable{HtmlNode}"/> of the descendant nodes that satisfy 
        ///   the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> Descendants(this HtmlNode node, Func<HtmlNode, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            return node.Descendants().Where(predicate);
        }

        /// <summary>
        /// Returns the child nodes of this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   An <see cref="IEnumerable{HtmlNode}"/> of the child nodes of this node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> Children(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            var element = node as HtmlContainer;
            if (element != null)
                return element.Children;
            else
                return Enumerable.Empty<HtmlNode>();
        }

        /// <summary>
        /// Returns the child nodes of this node filtered by a predicate.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   An <see cref="IEnumerable{HtmlNode}"/> of the child nodes that satisfy 
        ///   the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> Children(this HtmlNode node, Func<HtmlNode, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            return node.Children().Where(predicate);
        }

        /// <summary>
        /// Returns the node immediately following this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The node immediately following this node.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static HtmlNode NextSibling(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node.Parent != null)
            {
                var index = node.Parent.Children.IndexOf(node);
                if (index < node.Parent.Children.Count - 1)
                    return node.Parent.Children[index + 1];
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Returns the node immediately preceding this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The node immediately preceding this node.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static HtmlNode PreviousSibling(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node.Parent != null)
            {
                var index = node.Parent.Children.IndexOf(node);
                if (index > 0)
                    return node.Parent.Children[index - 1];
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Returns the first child of the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The first child of the node.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static HtmlNode FirstChild(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            var element = node as HtmlContainer;
            if (element != null)
                return element.Children.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Returns the last child of the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The last child of the node.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static HtmlNode LastChild(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            var element = node as HtmlContainer;
            if (element != null)
                return element.Children.LastOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Returns the parent nodes of this node (for nodes that can have parents).
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The parent nodes of <paramref name="node"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<HtmlNode> Parents(this HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            while (node.Parent != null)
            {
                yield return node.Parent;
                node = node.Parent;
            }
        }

        /// <summary>
        /// Returns the specified attribute of this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The value of <paramref name="attribute"/> of <paramref name="node"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static HtmlAttribute Attribute(this HtmlNode node, string attribute)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            var element = node as HtmlElement;
            if (element != null)
                return element.Attributes.Where(attr => attr.Name.Equals(attribute, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Returns the child nodes of this node with the specified tag name.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="tagName">The tag name.</param>
        /// <returns>The child nodes of <paramref name="node"/> with the tag name <paramref name="tagName"/>.</returns>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public static IEnumerable<HtmlNode> GetElementsByTagName(this HtmlNode node, string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                throw new ArgumentException(nameof(tagName));
            return node.Descendants().OfType<HtmlElement>().Where(e => e.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase));
        }
      
        /// <summary>
        /// Returns the child nodes of this node with the specified id attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="id">The value of the id attribute.</param>
        /// <returns>The child nodes of <paramref name="node"/> with the id <paramref name="id"/>.</returns>
        /// <exception cref="ArgumentException">
        ///   The parameter is <see langword="null"/> or white space.
        /// </exception>
        public static HtmlNode GetElementById(this HtmlNode node, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException(nameof(id));
            return node.Descendants().OfType<HtmlElement>().Where(e => e.Name.Equals(id, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        }

        /// <summary>
        /// Returns the value of the id attribute of this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The id of <paramref name="node"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static string Id(this HtmlNode node)
        {
            return node.Attribute("id")?.Value;
        }
     
        /// <summary>
        /// Returns the value of the class attribute of this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The class of <paramref name="node"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static string ClassName(this HtmlNode node)
        {
            return node.Attribute("class")?.Value;
        }

        /// <summary>
        /// Returns the values of the class attribute of this node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The class list of <paramref name="node"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<string> ClassList(this HtmlNode node)
        {
            return node.ClassName()?.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries) ??
                Enumerable.Empty<string>();
        }

        /// <summary>
        /// Determines whether the attribute value is the specified value.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        ///   <see langword="true"/> if the value of <paramref name="attribute"/> is
        ///   <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="attribute"/> is <see langword="null"/>.
        /// </exception>
        public static bool EqualsValue(this HtmlAttribute attribute, string value)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
            return attribute.Value?.Equals(value, StringComparison.CurrentCultureIgnoreCase) ?? false;
        }

        /// <summary>
        /// Determines whether the attribute value has the specified prefix.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        ///   <see langword="true"/> if the value of <paramref name="attribute"/> has
        ///   prefix <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="attribute"/> is <see langword="null"/>.
        /// </exception>
        public static bool StartsWith(this HtmlAttribute attribute, string value)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
            return attribute.Value?.StartsWith(value, StringComparison.CurrentCultureIgnoreCase) ?? false;
        }

        /// <summary>
        /// Determines whether the attribute value has the specified postfix.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        ///   <see langword="true"/> if the value of <paramref name="attribute"/> has
        ///   postfix <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="attribute"/> is <see langword="null"/>.
        /// </exception>
        public static bool EndsWith(this HtmlAttribute attribute, string value)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
            return attribute.Value?.EndsWith(value, StringComparison.CurrentCultureIgnoreCase) ?? false;
        }

        /// <summary>
        /// Determines whether the attribute value contains the specified substring.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        ///   <see langword="true"/> if the value of <paramref name="attribute"/> contains
        ///   <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="attribute"/> is <see langword="null"/>.
        /// </exception>
        public static bool Contains(this HtmlAttribute attribute, string value)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
            return attribute.Value?.ToLower()?.Contains(value.ToLower()) ?? false;
        }

        /// <summary>
        ///   Determines whether the attribute value starts with the specified prefix 
        ///   and a dash (-).
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        ///   <see langword="true"/> if the value of <paramref name="attribute"/> has
        ///   starts with <paramref name="value"/>-; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="attribute"/> is <see langword="null"/>.
        /// </exception>
        public static bool StartsWithWord(this HtmlAttribute attribute, string value)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
            if (attribute.Value?.Equals(value, StringComparison.CurrentCultureIgnoreCase) == true)
                return true;
            else
                return attribute.Value?.StartsWith(value + "-", StringComparison.CurrentCultureIgnoreCase) ?? false;
        }
    
        /// <summary>
        ///   Determines whether the attribute value has the specified word splitted by 
        ///   white spaces.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        ///   <see langword="true"/> if the value of <paramref name="attribute"/> has
        ///   word <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="attribute"/> is <see langword="null"/>.
        /// </exception>
        public static bool ContainsWord(this HtmlAttribute attribute, string value)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
            if (attribute.Value?.Equals(value, StringComparison.CurrentCultureIgnoreCase) == true)
                return true;
            else
                return attribute.Value?.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    ?.Any(val => val.Equals(value, StringComparison.CurrentCultureIgnoreCase)) ?? false;
        }

        internal static bool IsUniqueTagName(this HtmlElement element, out int position)
        {
            position = 1;
            var parent = element.Parent;
            if (parent == null)
                return true;
            else
            {
                int i = 1;
                bool hasFindPos = false;
                foreach (var e in parent.Children.OfType<HtmlElement>())
                {
                    if (e == element)
                    {
                        position = i;
                        hasFindPos = true;
                    }
                    else if (e.Name.Equals(element.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (hasFindPos)
                            return false;
                        i++;
                    }
                }
                return true;
            }
        }
    }
}
