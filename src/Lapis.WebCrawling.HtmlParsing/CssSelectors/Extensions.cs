/********************************************************************************
 * Module      : Lapis.WebCrawling.HtmlParsing
 * Class       : Extensions
 * Description : Provides extension methods for accessing HTML DOM with css selectors.
 * Created     : 2016/1/8
 * Note        :
*********************************************************************************/

using Lapis.WebCrawling.HtmlParsing.TreeConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.HtmlParsing.CssSelectors
{
    /// <summary>
    /// Provides extension methods for accessing HTML DOM with css selectors.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Selects the HTML with the specified selector.
        /// </summary>
        /// <param name="document">The HTML document to search.</param>
        /// <param name="cssSelector">A string that represents the css selector.</param>
        /// <returns>
        ///   The nodes in <paramref name="document"/> that match <paramref name="cssSelector"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="CssSelectorException">
        ///   <paramref name="cssSelector"/> is invalid.
        /// </exception>
        public static IEnumerable<HtmlNode> FindAll(this HtmlDocument document, string cssSelector)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            if (cssSelector == null)
                throw new ArgumentNullException(nameof(cssSelector));
            return CssSelector.Parse(cssSelector).Select(document.AsEnumerable());
        }

        /// <summary>
        /// Selects the HTML with the specified selector and returns the first selected node.
        /// </summary>
        /// <param name="document">The HTML document to search.</param>
        /// <param name="cssSelector">A string that represents the css selector.</param>
        /// <returns>
        ///   The first node in <paramref name="document"/> that matches <paramref name="cssSelector"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="CssSelectorException">
        ///   <paramref name="cssSelector"/> is invalid.
        /// </exception>
        public static HtmlNode Find(this HtmlDocument document, string cssSelector)
        {
            return document.FindAll(cssSelector).FirstOrDefault();
        }

        /// <summary>
        /// Selects the HTML elements with the specified selector.
        /// </summary>
        /// <param name="element">The HTML element to search.</param>
        /// <param name="cssSelector">A string that represents the css selector.</param>
        /// <returns>
        ///   The nodes in <paramref name="element"/> that match <paramref name="cssSelector"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="CssSelectorException">
        ///   <paramref name="cssSelector"/> is invalid.
        /// </exception>
        public static IEnumerable<HtmlNode> FindAll(this HtmlElement element, string cssSelector)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (cssSelector == null)
                throw new ArgumentNullException(nameof(cssSelector));
            return CssSelector.Parse(cssSelector).Select(element.AsEnumerable());
        }

        /// <summary>
        /// Selects the HTML elements with the specified selector and returns the first selected node.
        /// </summary>
        /// <param name="element">The HTML element to search.</param>
        /// <param name="cssSelector">A string that represents the css selector.</param>
        /// <returns>
        ///   The first node in <paramref name="element"/> that matches <paramref name="cssSelector"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="CssSelectorException">
        ///   <paramref name="cssSelector"/> is invalid.
        /// </exception>
        public static HtmlNode Find(this HtmlElement element, string cssSelector)
        {
            return element.FindAll(cssSelector).FirstOrDefault();
        }

        /// <summary>
        /// Returns the css selector for a HTML element.
        /// </summary>
        /// <param name="element">The HTML element.</param>
        /// <returns>A string that represents the selector.</returns>
        /// <exception cref="ArgumentNullException">
        ///   The parameter is <see langword="null"/>.
        /// </exception>
        public static string GetCssSelector(this HtmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            var sb = new StringBuilder();
            HtmlNode node = element;
            bool stop = false;
            while (!stop && node != null &&
                !(node is HtmlDocument))
            {
                string str = "";
                if (node is HtmlElement)
                {
                    var id = node.Id();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        str = $"#{id}";
                        stop = true;
                        goto done;
                    }
                    var className = node.ClassName();
                    if (!string.IsNullOrWhiteSpace(className))
                    {
                        str = "";
                        foreach (var cls in node.ClassList())
                        {
                            str += $".{cls}";
                        }
                        goto done;
                    }
                    var tagName = (node as HtmlElement).Name;
                    {
                        int position;
                        if ((node as HtmlElement).IsUniqueTagName(out position))
                            str = tagName.ToLower();
                        else
                            str = $"{tagName.ToLower()}:nth-of-type({position})";
                    }
                    done:
                    if (sb.Length > 0)
                        sb.Insert(0, ">").Insert(0, str);
                    else
                        sb.Append(str);
                }
                node = node.Parent;
            }
            return sb.ToString();
        }
        

    }
}
