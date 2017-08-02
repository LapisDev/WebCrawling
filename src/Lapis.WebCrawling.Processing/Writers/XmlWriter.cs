/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : XmlWriter
 * Description : Implements an IRecordWriter that writes IRecord to XML.
 * Created     : 2016/1/26
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lapis.WebCrawling.Processing;
using System.IO;

namespace Lapis.WebCrawling.Writers
{
    /// <summary>
    ///   Implements an <see cref="IRecordWriter"/> that writes <see cref="IRecord"/> to 
    ///   XML.
    /// </summary>
    public sealed class XmlWriter : IRecordWriter, IDisposable
    {
        /// <summary>
        ///   Initalize a new instance of the <see cref="XmlWriter"/> class using a 
        ///   <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="TextWriter"/> used by the <see cref="XmlWriter"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public XmlWriter(TextWriter writer)
            : this(writer, XmlOptions.Indent | XmlOptions.WriteDocument)
        { }

        /// <summary>
        ///   Initalize a new instance of the <see cref="XmlWriter"/> class using a 
        ///   <see cref="TextWriter"/> and specified <see cref="XmlOptions"/>.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="TextWriter"/> used by the <see cref="XmlWriter"/>.
        /// </param>
        /// <param name="options">The <see cref="XmlOptions"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public XmlWriter(TextWriter writer, XmlOptions options)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            Options = options;
            _writer = System.Xml.XmlWriter.Create(writer,
                new System.Xml.XmlWriterSettings()
                {
                    ConformanceLevel = System.Xml.ConformanceLevel.Auto,
                    Indent = (Options & XmlOptions.Indent) != 0
                });
        }

        /// <summary>
        /// Gets the options used by the <see cref="XmlWriter"/>.
        /// </summary>
        /// <value>The options used by the <see cref="XmlWriter"/>.</value>
        public XmlOptions Options { get; }

        /// <inheritdoc cref="IRecordWriter.Write(IEnumerable{IRecord})"/>
        public void Write(IEnumerable<IRecord> records)
        {
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
            if (!_hasWrittenStartDocument)
            {
                WriteStartDocument();                
            }
            _writer.WriteStartElement("Record");
            foreach (var pair in record)
            {
                _writer.WriteStartElement(pair.Key);
                _writer.WriteString(pair.Value);
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
        }

        /// <inheritdoc cref="IRecordWriter.Close"/>
        public void Close()
        {
            Dispose();
        }


        private void WriteStartDocument()
        {
            if ((Options & XmlOptions.WriteDocument) != 0)
            {
                _writer.WriteStartDocument();
                _writer.WriteStartElement("Records");
            }
            _hasWrittenStartDocument = true;
        }
        private void WriteEndDocument()
        {
            if ((Options & XmlOptions.WriteDocument) != 0 &&
                _hasWrittenStartDocument)
            {
                _writer.WriteEndDocument();
            }
        }

        private System.Xml.XmlWriter _writer;

        private bool _hasWrittenStartDocument = false;

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
                WriteEndDocument();
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
        /// Releases all resources used by the <see cref="XmlWriter"/> object. 
        /// </summary>
        /// <remarks>
        ///   Call <see cref="Dispose()"/> when you are finished using the <see cref="XmlWriter"/>. 
        ///   The <see cref="Dispose()"/> method leaves the <see cref="XmlWriter"/> in an unusable state.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    /// <summary>
    /// Represents the options that a <see cref="XmlWriter"/> uses.
    /// </summary>
    [Flags]
    public enum XmlOptions
    {
        /// <summary>
        /// The default option.
        /// </summary>
        None,
        /// <summary>
        /// The value indicates that the XML is indented.
        /// </summary>
        Indent,
        /// <summary>
        /// The value indicates that the records are wriiten as a XML document with XML header.
        /// </summary>
        WriteDocument
    }
}
