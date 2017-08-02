/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : TagTokenAttributeCollection
 * Description : Represents a collection of attributes.
 * Created     : 2015/8/24
 * Note        : 
*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.Tokenization
{
    /// <summary>
    /// Represents a collection of attributes.
    /// </summary>
    public class TagTokenAttributeCollection : IEnumerable<TagTokenAttribute>
    {
        /// <summary>
        /// Gets or sets the attribute at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the attribute to get or set.</param>
        /// <value>The attribute to get or set.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is negative, or greater than or equal to <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when setting the value to <see langword="null"/>.
        /// </exception>
        public TagTokenAttribute this[int index]
        {
            get { return _innerList[index]; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _innerList[index] = value;
            }
        }

        /// <summary>
        /// Gets the number of the attributes contained in the collection.
        /// </summary>
        /// <value>The number of the attributes.</value>
        public int Count
        {
            get { return _innerList.Count; }
        }

        /// <summary>
        /// Adds an attribute to the end of the collection.
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public void Add(TagTokenAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException();
            _innerList.Add(attribute);
        }

        /// <summary>
        /// Removes all attributes from the collection.
        /// </summary>
        public void Clear()
        {
            _innerList.Clear();
        }

        /// <summary>
        /// Determines whether an attribute is in the collection.
        /// </summary>
        /// <param name="attribute">The attribute to locate in the collection.</param>
        /// <returns>
        ///   <see langword="true"/> is <paramref name="attribute"/> is found in the collection;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(TagTokenAttribute attribute)
        {
            return _innerList.Contains(attribute);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<TagTokenAttribute> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        /// <summary>
        ///   Searches for the specified attribute and returns the zero-based index of the 
        ///   first occurrence within the collection.
        /// </summary>
        /// <param name="attribute">The attribute to locate in the collection.</param>
        /// <returns>
        ///   The zero-based index of the first occurrence of <paramref name="attribute"/>
        ///   within the collection; otherwise, -1.
        /// </returns>
        public int IndexOf(TagTokenAttribute attribute)
        {
            return _innerList.IndexOf(attribute);
        }

        /// <summary>
        /// Inserts an attribute into the collection at the specified index.
        /// </summary>
        /// <param name="index">
        ///   The zero-based index at which <paramref name="attribute"/> should be inserted.
        /// </param>
        /// <param name="attribute">The attribute to insert.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="attribute"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is negative, or greater than or equal to <see cref="Count"/>.
        /// </exception>
        public void Insert(int index, TagTokenAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException();
            _innerList.Insert(index, attribute);
        }

        /// <summary>
        /// Removes the first occurrence of a specific attribute from the collection.
        /// </summary>
        /// <param name="attribute">The attribute to remove from the collection.</param>
        /// <returns>
        ///   <see langword="true"/> if <paramref name="attribute"/> is successfully removed; 
        ///   otherwise, <see langword="false"/>. This method also returns <see langword="false"/>
        ///   if <paramref name="attribute"/> was not found in the collection.
        /// </returns>
        public bool Remove(TagTokenAttribute attribute)
        {
            return _innerList.Remove(attribute);
        }

        /// <summary>
        /// Removes the attribute at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the attribute to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is negative, or greater than or equal to <see cref="Count"/>.
        /// </exception>
        public void RemoveAt(int index)
        {
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
        /// Returns the string representation of the <see cref="TagTokenAttributeCollection"/> object.
        /// </summary>
        /// <returns>
        ///   The string representation of the <see cref="TagTokenAttributeCollection"/> object.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var attr in this)
            {
                sb.Append(attr).Append(' ');
            }
            if (sb.Length > 0)
                sb.Length -= 1;
            return sb.ToString();
        }


        internal TagTokenAttributeCollection() { }

        private IList<TagTokenAttribute> _innerList = new List<TagTokenAttribute>();
    }
}
