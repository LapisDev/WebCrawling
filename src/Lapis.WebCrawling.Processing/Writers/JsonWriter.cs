/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : JsonWriter
 * Description : Implements an IRecordWriter that writes IRecord to JSON.
 * Created     : 2016/1/27
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Lapis.WebCrawling.Writers
{
    /// <summary>
    ///   Implements an <see cref="IRecordWriter"/> that writes <see cref="IRecord"/> to 
    ///   JSON.
    /// </summary>
    public sealed class JsonWriter : IRecordWriter, IDisposable
    {        
        /// <summary>
        ///   Initalize a new instance of the <see cref="JsonWriter"/> class using a 
        ///   <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="TextWriter"/> used by the <see cref="JsonWriter"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public JsonWriter(TextWriter writer)
            : this(writer, JsonOptions.Indent | JsonOptions.WriteArray)
        { }

        /// <summary>
        ///   Initalize a new instance of the <see cref="JsonWriter"/> class using a 
        ///   <see cref="TextWriter"/> and specified <see cref="JsonOptions"/>.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="TextWriter"/> used by the <see cref="JsonWriter"/>.
        /// </param>
        /// <param name="options">The <see cref="JsonOptions"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public JsonWriter(TextWriter writer, JsonOptions options)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            Options = options;
            _writer = writer;
        }

        /// <summary>
        /// Gets the options used by the <see cref="JsonWriter"/>.
        /// </summary>
        /// <value>The options used by the <see cref="JsonWriter"/>.</value>
        public JsonOptions Options { get; }

        /// <inheritdoc cref="IRecordWriter.Write(IEnumerable{IRecord})"/>
        public void Write(IEnumerable<IRecord> records)
        {
            if (records == null)
                throw new ArgumentNullException(nameof(records));
            CheckDisposed();
            foreach (var record in records)
            {
                if (record != null)
                    Write(record);
            }
        }

        /// <inheritdoc cref="IRecordWriter.Write(IRecord)"/>
        public void Write(IRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            CheckDisposed();
            if (_isFirstRecord)
            {
                WriteStartArray();
                _isFirstRecord = false;
            }
            else
            {
                _writer.Write(",");
                if ((Options & JsonOptions.Indent) != 0)
                    _writer.WriteLine();
            }
            if ((Options & JsonOptions.Indent) != 0)
            {
                _writer.Write(new string(' ', _currentIndent));              
            }
            _writer.Write("{");
            if ((Options & JsonOptions.Indent) != 0)
            {
                _writer.WriteLine();
                _currentIndent += 4;
            }
            bool isFirst = true;
            foreach (var pair in record)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    _writer.Write(",");
                    if ((Options & JsonOptions.Indent) != 0)
                        _writer.WriteLine();
                }
                if ((Options & JsonOptions.Indent) != 0)
                {
                    _writer.Write(new string(' ', _currentIndent));
                }
                _writer.Write("\"");
                _writer.Write(Escape(pair.Key));
                _writer.Write("\":\"");
                _writer.Write(Escape(pair.Value));
                _writer.Write("\"");
            }
            if ((Options & JsonOptions.Indent) != 0)
            {
                _writer.WriteLine();
                _currentIndent -= 4;
                _writer.Write(new string(' ', _currentIndent));
            }
            _writer.Write("}");
        }

        /// <inheritdoc cref="IRecordWriter.Close"/>
        public void Close()
        {
            Dispose();
        }

        private string Escape(string s)
        {
            var result = s;
            if (result == null)
                return result;
            if (result.Contains("\n"))
            {
                result = result.Replace("\n", @"\n");
            }
            if (result.Contains("\r"))
            {
                result = result.Replace("\r", @"\r");
            }
            if (result.Contains("\""))
            {
                result = result.Replace("\"", "\\\"");
            }
            return result;
        }

        private void WriteStartArray()
        {
            if ((Options & JsonOptions.WriteArray) != 0)
            {
                _writer.Write("[");
                if ((Options & JsonOptions.Indent) != 0)
                {
                    _writer.WriteLine();
                    _currentIndent += 4;
                }
            }
        }
        private void WriteEndArray()
        {
            if ((Options & JsonOptions.WriteArray) != 0 &&
                !_isFirstRecord)
            {
                if ((Options & JsonOptions.Indent) != 0)
                {
                    _writer.WriteLine();
                    _currentIndent -= 4;
                }
                _writer.Write("]");
            }
        }

        private TextWriter _writer;

        private bool _isFirstRecord = true;

        private int _currentIndent = 0;


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
                WriteEndArray();
                if (disposing)
                {
                    if (_writer != null)
                        _writer.Dispose();
                }
                _writer = null;
                _isDisposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="JsonWriter"/> object. 
        /// </summary>
        /// <remarks>
        ///   Call <see cref="Dispose()"/> when you are finished using the <see cref="JsonWriter"/>. 
        ///   The <see cref="Dispose()"/> method leaves the <see cref="JsonWriter"/> in an unusable state.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

    }

    /// <summary>
    /// Represents the options that a <see cref="JsonWriter"/> uses.
    /// </summary>
    [Flags]
    public enum JsonOptions
    {
        /// <summary>
        /// The default option.
        /// </summary>
        None,
        /// <summary>
        /// The value indicates that the JSON is indented.
        /// </summary>
        Indent,
        /// <summary>
        /// The value indicates that the records are wriiten as a JSON array.
        /// </summary>
        WriteArray
    }
}
