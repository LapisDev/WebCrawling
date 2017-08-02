/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : DOCTYPEToken
 * Description : Represents a DOCTYPE token.
 * Created     : 2015/8/24
 * Note        : 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.Tokenization
{
    /// <summary>
    /// Represents a DOCTYPE token.
    /// </summary>
    public class DOCTYPEToken : Token
    {
        /// <summary>
        /// Gets or sets the name of the DOCTYPE.
        /// </summary>
        /// <value>The name of the DOCTYPE.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the public identifier.
        /// </summary>
        /// <value>The public identifier.</value>
        public string PublicIdentifier { get; set; } = null;

        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <value>The system identifierE.</value>
        public string SystemIdentifier { get; set; } = null;

        /// <summary>
        /// Gets or sets the force-quirks flag.
        /// </summary>
        /// <value>The force-quirks flag.</value>
        public bool ForceQuirksFlag { get; set; } = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="DOCTYPEToken"/> class with the specified parameters.
        /// </summary>
        /// <param name="location">The position of the token.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="location"/> is <see langword="null"/>.
        /// </exception>
        public DOCTYPEToken(Location location)
            : base(location)
        { }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (PublicIdentifier != null && SystemIdentifier != null)
                return $"{Location} : <!DOCTYPE {Name} PUBLIC \"{PublicIdentifier}\" \"{SystemIdentifier}\">";
            else if (PublicIdentifier != null)
                return $"{Location} : <!DOCTYPE {Name} PUBLIC \"{PublicIdentifier}\">";
            else if (SystemIdentifier != null)
                return $"{Location} : <!DOCTYPE {Name} \"{SystemIdentifier}\">";
            else
                return $"{Location} : <!DOCTYPE {Name}>";
        }
    }
}
