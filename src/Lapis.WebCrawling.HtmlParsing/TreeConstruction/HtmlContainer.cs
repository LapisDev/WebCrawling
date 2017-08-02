/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : HtmlContainer
 * Description : Represents a node that can contain other nodes.
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
    /// Represents a node that can contain other nodes.
    /// </summary>
    public abstract class HtmlContainer : HtmlNode
    {
        /// <summary>
        /// Gets the child nodes of the node.
        /// </summary>
        /// <value>The child nodes of the node.</value>
        public HtmlNodeCollection Children { get; }
   

        internal HtmlContainer()
        {
            Children = new HtmlNodeCollection(this);
        }
    }
}
