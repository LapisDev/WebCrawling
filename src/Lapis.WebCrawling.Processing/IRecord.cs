/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : IRecord
 * Description : Represents a record processed.
 * Created     : 2016/1/26
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Processing
{
    /// <summary>
    /// Represents a record processed.
    /// </summary>
    public interface IRecord : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <value>The value associated with the specified key.</value>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        ///   <paramref name="key"/> does not exist in the <see cref="IRecord"/>.
        /// </exception>
        string this[string key] { get; }

        /// <summary>
        /// Gets the number of fields contained in the <see cref="IRecord"/>.
        /// </summary>
        /// <value>The number of fields contained in the <see cref="IRecord"/>.</value>
        int Count { get; }

        /// <summary>
        /// Gets a collection containing the keys in the <see cref="IRecord"/>.
        /// </summary>
        /// <value>A collection containing the keys in the <see cref="IRecord"/>.</value>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Gets a collection containing the values in the <see cref="IRecord"/>.
        /// </summary>
        /// <value>A collection containing the values in the <see cref="IRecord"/>.</value>
        IEnumerable<string> Values { get; }

        /// <summary>
        /// Determines whether the <see cref="IRecord"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IRecord"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="IRecord"/> contains <paramref name="key"/>;
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        bool ContainsKey(string key);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        ///   When this method returns, contains the value associated with the specified key, if the 
        ///   key is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized. 
        /// </param>
        /// <returns>
        ///   <see langword="true"/> if the <see cref="IRecord"/> contains an element with the specified 
        ///   key; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        bool TryGetValue(string key, out string value);
    }
}
