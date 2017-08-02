/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : IRecordWriter
 * Description : Provides methods for writing IRecord.
 * Created     : 2016/1/26
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Writers
{
    /// <summary>
    /// Provides methods for writing <see cref="IRecord"/>.
    /// </summary>
    public interface IRecordWriter
    {
        /// <summary>
        /// Writes an <see cref="IRecord"/> object.
        /// </summary>
        /// <param name="record">The <see cref="IRecord"/> to write.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        void Write(IRecord record);

        /// <summary>
        /// Writes a sequence of <see cref="IRecord"/> objects.
        /// </summary>
        /// <param name="records">A sequencs of <see cref="IRecord"/> objects.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        void Write(IEnumerable<IRecord> records);

        /// <summary>
        /// Closes the writer.
        /// </summary>
        void Close();
    }
}
