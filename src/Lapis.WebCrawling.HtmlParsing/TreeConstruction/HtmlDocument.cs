/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlDocument
 * Description : Represents a HTML document.
 * Created     : 2015/8/25
 * Note        : 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.TreeConstruction
{
    /// <summary>
    /// Represents a HTML document.
    /// </summary>
    public class HtmlDocument : HtmlContainer
    {
        /// <summary>
        /// Gets or sets the DOCTYPE.
        /// </summary>
        /// <value>The DOCTYPE.</value>
        public HtmlDocumentType DocumentType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlDocument"/> class.
        /// </summary>
        public HtmlDocument() : base() { }

        /// <inheritdoc/>
        public override string ToString(HtmlFormat format)
        {
            if (format == HtmlFormat.Compressed)
            {
                if (DocumentType != null)
                    return $"{DocumentType}{Children.ToString(HtmlFormat.Compressed)}";
                else
                    return $"{Children.ToString(HtmlFormat.Compressed)}";
            }
            else
                return ToString(0);
        }

        /// <inheritdoc/>
        public override string ToString(int indentation)
        {
            var ind = new string(' ', indentation);
            if (DocumentType != null)
                return $"{ind}{DocumentType}\n{Children.ToString(indentation)}";
            else
                return $"{Children.ToString(indentation)}";
        }
    }
}
