/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlNodeCollection
 * Description : Represents a collection of HTML nodes.
 * Created     : 2015/8/25
 * Note        : 
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.TreeConstruction
{
    /// <summary>
    /// Represents a collection of HTML nodes.
    /// </summary>
    public class HtmlNodeCollection : IEnumerable<HtmlNode>
    {
        /// <summary>
        /// Gets the parent <see cref="HtmlContainer"/> of the nodes.
        /// </summary>
        /// <value>The parent element.</value>
        public HtmlContainer Parent { get; }

        /// <summary>
        /// Gets or sets the node at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the node to get or set.</param>
        /// <value>The node to get or set.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is negative, or greater than or equal to <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when setting the value to <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="value"/> is already a child node of another element.
        /// </exception>
        public HtmlNode this[int index]
        {
            get
            {
                return _innerList[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Parent != null)
                    throw new ArgumentException(ExceptionResource.NodeHasParent);
                _innerList[index].Parent = null;
                value.Parent = Parent;
                _innerList[index] = value;
            }
        }

        /// <summary>
        /// Gets the number of the nodes contained in the collection.
        /// </summary>
        /// <value>The number of the nodes.</value>
        public int Count
        {
            get { return _innerList.Count; }
        }

        /// <summary>
        /// Adds a node to the end of the collection.
        /// </summary>
        /// <param name="node">The node to add.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="node"/> is already a child node of another element.
        /// </exception>
        public void Add(HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException();
            if (node.Parent != null)
                throw new ArgumentException(ExceptionResource.NodeHasParent);
            node.Parent = Parent;
            _innerList.Add(node);
        }

        /// <summary>
        /// Removes all nodes from the collection.
        /// </summary>
        public void Clear()
        {
            foreach (var node in _innerList)
                node.Parent = null;
            _innerList.Clear();
        }

        /// <summary>
        /// Determines whether a node is in the collection.
        /// </summary>
        /// <param name="node">The node to locate in the collection.</param>
        /// <returns>
        ///   <see langword="true"/> is <paramref name="node"/> is found in the collection;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(HtmlNode node)
        {
            return _innerList.Contains(node);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<HtmlNode> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        /// <summary>
        ///   Searches for the specified node and returns the zero-based index of the 
        ///   first occurrence within the collection.
        /// </summary>
        /// <param name="node">The node to locate in the collection.</param>
        /// <returns>
        ///   The zero-based index of the first occurrence of <paramref name="node"/>
        ///   within the collection; otherwise, -1.
        /// </returns>
        public int IndexOf(HtmlNode node)
        {
            return _innerList.IndexOf(node);
        }

        /// <summary>
        /// Inserts a node into the collection at the specified index.
        /// </summary>
        /// <param name="index">
        ///   The zero-based index at which <paramref name="node"/> should be inserted.
        /// </param>
        /// <param name="node">The node to insert.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="node"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is negative, or greater than or equal to <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="node"/> is already a child node of another element.
        /// </exception>
        public void Insert(int index, HtmlNode node)
        {
            if (node.Parent != null)
                throw new ArgumentException(ExceptionResource.NodeHasParent);
            node.Parent = Parent;
            _innerList.Insert(index, node);
        }

        /// <summary>
        /// Removes the first occurrence of a specific node from the collection.
        /// </summary>
        /// <param name="node">The node to remove from the collection.</param>
        /// <returns>
        ///   <see langword="true"/> if <paramref name="node"/> is successfully removed; 
        ///   otherwise, <see langword="false"/>. This method also returns <see langword="false"/>
        ///   if <paramref name="node"/> was not found in the collection.
        /// </returns>
        public bool Remove(HtmlNode node)
        {
            node.Parent = null;
            return _innerList.Remove(node);
        }

        /// <summary>
        /// Removes the node at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the node to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is negative, or greater than or equal to <see cref="Count"/>.
        /// </exception>
        public void RemoveAt(int index)
        {
            _innerList[index].Parent = null;
            _innerList.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of the <see cref="HtmlNodeCollection"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="HtmlNodeCollection"/> object.
        /// </returns>
        public override string ToString()
        {
            return ToString(0);
        }

        /// <summary>
        ///   Returns the string representation of the <see cref="HtmlNodeCollection"/> object with
        ///   the specified indent number of characters.
        /// </summary>
        /// <param name="indentation">The indent number of characters.</param>
        /// <returns>
        ///   The string representation of the <see cref="HtmlNodeCollection"/> object.
        /// </returns>
        public string ToString(int indentation)
        {
            var sb = new StringBuilder();
            bool isFirst = true;
            foreach (var node in _innerList)
            {   if (isFirst)
                    isFirst = false;
                else
                    sb.AppendLine();
                sb.Append(node.ToString(indentation));
            }
            return sb.ToString();
        }

        /// <summary>
        ///   Returns the string representation of the <see cref="HtmlNodeCollection"/> object with
        ///   the specified <see cref="HtmlFormat"/>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///   The string representation of the <see cref="HtmlNodeCollection"/> object.
        /// </returns>
        public string ToString(HtmlFormat format)
        {
            if (format == HtmlFormat.Compressed)
            {
                var sb = new StringBuilder();
                foreach (var node in _innerList)
                    sb.Append(node.ToString(HtmlFormat.Compressed));
                return sb.ToString();
            }
            else
                return ToString(0);
        }

        internal HtmlNodeCollection(HtmlContainer parent)
        {
            Parent = parent;
        }

        private IList<HtmlNode> _innerList = new List<HtmlNode>();
    }
}
