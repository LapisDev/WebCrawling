/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlElement
 * Description : Represents a HTML element.
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
    /// Represents a HTML element.
    /// </summary>
    public class HtmlElement : HtmlContainer
    {
        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>The name of the element.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the attributes of the element.
        /// </summary>
        /// <value>The attributes of the element.</value>
        public HtmlAttributeCollection Attributes { get; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HtmlElement"/> with 
        ///   the specified name.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public HtmlElement(string name)
        {
            Name = name;
            Attributes = new HtmlAttributeCollection(this);
        }

        /// <inheritdoc/>
        public override string ToString(int indentation)
        {
            var ind = new string(' ', indentation);
            if (Children.Count == 1 && Children[0] is HtmlText)
            {
                if (Attributes.Count > 0)
                    return $"{ind}<{Name} {Attributes}>{Children.ToString(HtmlFormat.Compressed)}</{Name}>";
                else
                    return $"{ind}<{Name}>{Children.ToString(HtmlFormat.Compressed)}</{Name}>";
            }
            else if (Children.Count > 0)
            {
                if (Attributes.Count > 0)
                    return $"{ind}<{Name} {Attributes}>\n{Children.ToString(indentation + 4)}\n{ind}</{Name}>";
                else
                    return $"{ind}<{Name}>\n{Children.ToString(indentation + 4)}\n{ind}</{Name}>";
            }
            else
            {
                if (Attributes.Count > 0)
                    return $"{ind}<{Name} {Attributes}/>";
                else
                    return $"{ind}<{Name}/>";
            }
        }

        /// <inheritdoc/>
        public override string ToString(HtmlFormat format)
        {
            if (format == HtmlFormat.Compressed)
                if (Children.Count > 0)
                {
                    if (Attributes.Count > 0)
                        return $"<{Name} {Attributes}>{Children.ToString(HtmlFormat.Compressed)}</{Name}>";
                    else
                        return $"<{Name}>{Children.ToString(HtmlFormat.Compressed)}</{Name}>";
                }
                else
                {
                    if (Attributes.Count > 0)
                        return $"<{Name} {Attributes}/>";
                    else
                        return $"<{Name}/>";
                }
            else
                return ToString(0);
        }
    }
}
