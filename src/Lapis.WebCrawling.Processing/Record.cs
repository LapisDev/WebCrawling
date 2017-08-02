/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : Record
 * Description : Implements a default IRecord.
 * Created     : 2016/1/26
 * Note        :
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Processing
{
    /// <summary>
    ///   Implements a default <see cref="IRecord"/>. This class is a wrapper of 
    ///   <see cref="Dictionary{String, String}"/>.
    /// </summary>
    public sealed class Record : IRecord
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="Record"/> class with an 
        ///   <see cref="IDictionary{String, String}"/>.
        /// </summary>
        /// <param name="dictionary">
        ///   An <see cref="IDictionary{String, String}"/> whose keys and values 
        ///   are copied to the new <see cref="Record"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public Record(IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            _dictionary = new Dictionary<string, string>(dictionary);
        }

        internal Record(Dictionary<string, string> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <inheritdoc cref="IRecord"/>
        public string this[string key]
        {
            get { return _dictionary[key]; }
        }

        /// <inheritdoc cref="IRecord.Count"/>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <inheritdoc cref="IRecord.Keys"/>
        public IEnumerable<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        /// <inheritdoc cref="IRecord.Values"/>
        public IEnumerable<string> Values
        {
            get { return _dictionary.Values; }
        }

        /// <inheritdoc cref="IRecord.ContainsKey"/>
        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <inheritdoc cref="Record.GetEnumerator"/>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <inheritdoc cref="IRecord.TryGetValue"/>
        public bool TryGetValue(string key, out string value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc cref="Record.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of the <see cref="Record"/> object.
        /// </summary>
        /// <returns>The string representation of the <see cref="Record"/> object.</returns>
        public override string ToString()
        {
            return string.Join(", ", _dictionary.Select(pair => $"{pair.Key} = {pair.Value}"));
        }

        /// <summary>
        ///   Defines an explicit conversion of an <see cref="Record"/> object to a
        ///   <see cref="Dictionary{String, String}"/> object.
        /// </summary>
        /// <param name="record">The <see cref="Record"/> object.</param>
        /// <returns>
        ///   A <see cref="Dictionary{String, String}"/> that contains the keys and values in 
        ///   <paramref name="record"/>.
        /// </returns>
        public static explicit operator Dictionary<string, string>(Record record)
        {
            if (record == null)
                return null;
            else
                return new Dictionary<string, string>(record._dictionary);
        }

        private Dictionary<string, string> _dictionary;
    }
}
