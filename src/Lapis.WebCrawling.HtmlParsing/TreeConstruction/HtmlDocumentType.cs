/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlDocumentType
 * Description : Represents a HTML DOCTYPE definition.
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
    /// Represents a HTML DOCTYPE definition.
    /// </summary>
    public class HtmlDocumentType : HtmlObject
    {
        /// <summary>
        /// Gets or sets the name of the DOCTYPE.
        /// </summary>
        /// <value>The name of the DOCTYPE.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the public identifier.
        /// </summary>
        /// <value>The public identifier.</value>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <value>The system identifier.</value>
        public string SystemId { get; set; }

        /// <summary>
        ///   Initializes an instance of the <see cref="HtmlDocumentType"/> class with
        ///   the specified parameters.
        /// </summary>
        /// <param name="name">The name of the DOCTYPE.</param>
        /// <param name="publicId">The public identifier.</param>
        /// <param name="systemId">The system identifier.</param>
        public HtmlDocumentType(string name, string publicId, string systemId)
        {
            Name = name;
            PublicId = publicId;
            SystemId = systemId;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (PublicId != null && SystemId != null)
                return $"<!DOCTYPE {Name} PUBLIC \"{PublicId}\" \"{SystemId}\">";
            else if (PublicId != null)
                return $"<!DOCTYPE {Name} PUBLIC \"{PublicId}\">";
            else if (SystemId != null)
                return $"<!DOCTYPE {Name} \"{SystemId}\">";
            else
                return $"<!DOCTYPE {Name}>";
        }
    }
}
