/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : CsvWriter
 * Description : Implements an IRecordWriter that writes IRecord to Comma-Separated
                 Values (CSV).
 * Created     : 2016/1/26
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.Processing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Writers
{
    /// <summary>
    ///   Implements an <see cref="IRecordWriter"/> that writes <see cref="IRecord"/> to 
    ///   Comma-Separated Values (CSV).
    /// </summary>
    public sealed class CsvWriter : IRecordWriter, IDisposable
    {
        /// <summary>
        ///   Initalize a new instance of the <see cref="CsvWriter"/> class using a 
        ///   <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="TextWriter"/> used by the <see cref="CsvWriter"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public CsvWriter(TextWriter writer)
            : this(writer, CsvOptions.WriteHeader)
        { }

        /// <summary>
        ///   Initalize a new instance of the <see cref="CsvWriter"/> class using a 
        ///   <see cref="TextWriter"/> and specified <see cref="CsvOptions"/>.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="TextWriter"/> used by the <see cref="CsvWriter"/>.
        /// </param>
        /// <param name="options">The <see cref="CsvOptions"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public CsvWriter(TextWriter writer, CsvOptions options)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            _writer = writer;
            Options = options;
        }

        /// <summary>
        /// Gets the options used by the <see cref="CsvWriter"/>.
        /// </summary>
        /// <value>The options used by the <see cref="CsvWriter"/>.</value>
        public CsvOptions Options { get; }

        /// <inheritdoc cref="IRecordWriter.Write(IRecord)"/>
        public void Write(IRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            CheckDisposed();
            if (_headers == null)
            {
                _headers = record.Keys;
                WriteLine(_headers);
            }
            WriteLine(_headers.Select(h =>
            {
                string v;
                record.TryGetValue(h, out v);
                return v;
            }));
        }

        /// <inheritdoc cref="IRecordWriter.Write(IEnumerable{IRecord})"/>
        public void Write(IEnumerable<IRecord> records)
        {
            if (records == null)
                throw new ArgumentNullException(nameof(records));
            CheckDisposed();

            var enumerator = records.GetEnumerator();
            if (!enumerator.MoveNext())
                return;
            var record = enumerator.Current;
            if (_headers == null)
            {
                _headers = record.Keys;
                WriteLine(_headers);
            }
            while (true)
            {
                record = enumerator.Current;
                WriteLine(_headers.Select(h =>
                {
                    string v;
                    record.TryGetValue(h, out v);
                    return v;
                }));
                if (!enumerator.MoveNext())
                    break;
            }
        }

        /// <inheritdoc cref="IRecordWriter.Close"/>
        public void Close()
        {
            Dispose();
        }

        private bool NeedQuote(string value, out string result)
        {
            bool quote = false;
            result = value;
            if (result == null)
                return false;
            if (result.Contains(","))
            {
                quote = true;
            }
            if (result.Contains("\n"))
            {
                result = result.Replace("\n", @"\n");
                quote = true;
            }
            if (result.Contains("\r"))
            {
                result = result.Replace("\r", @"\r");
                quote = true;
            }
            if (result.Contains("\""))
            {
                result = result.Replace("\"", "\"\"");
                quote = true;
            }
            if (quote)
                result = $"\"{result}\"";
            return quote;
        }

        private void WriteLine(IEnumerable<string> fields)
        {
            var line = string.Join(",", fields.Select(s =>
            {
                string r;
                bool quote = NeedQuote(s, out r);
                if ((Options & CsvOptions.QuoteAll) != 0 && !quote)
                    r = $"\"{r}\"";
                return r;
            }));
            _writer.WriteLine(line);
        }

        private void WriteHeader()
        {
            if ((Options & CsvOptions.WriteHeader) != 0)
                WriteLine(_headers);
        }

        private IEnumerable<string> _headers;

        private TextWriter _writer;


        #region IDisposable Support

        private bool _isDisposed = false;

        private void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().ToString());
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_writer != null)
                        _writer.Dispose();
                }
                _writer = null;
                _headers = null;
                _isDisposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="CsvWriter"/> object. 
        /// </summary>
        /// <remarks>
        ///   Call <see cref="Dispose()"/> when you are finished using the <see cref="CsvWriter"/>. 
        ///   The <see cref="Dispose()"/> method leaves the <see cref="CsvWriter"/> in an unusable state.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion                
    }

    /// <summary>
    /// Represents the options that a <see cref="CsvWriter"/> uses.
    /// </summary>
    [Flags]
    public enum CsvOptions
    {
        /// <summary>
        /// The default option.
        /// </summary>
        None,
        /// <summary>
        /// The value indicates that all the fields are double quoted.
        /// </summary>
        QuoteAll,
        /// <summary>
        /// The value indicates that a header is written before the records.
        /// </summary>
        WriteHeader
    }
}
