/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : HtmlCleaner
 * Description : Provides methods for clean bad-formed HTML.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;

namespace Lapis.WebCrawling.Parsing
{
    /// <summary>
    /// Provides methods for clean bad-formed HTML.
    /// </summary>
    [Obsolete]
    public partial class HtmlCleaner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlCleaner"/> class.
        /// </summary>
        public HtmlCleaner() { }

        /// <summary>
        /// Converts the HTML to well-formed HTML.
        /// </summary>
        /// <param name="html">The HTML to clean.</param>
        /// <returns>The well-formed HTML converted from <paramref name="html"/>.</returns>
        public string Clean(string html)            
        {
            return Clean(html, HtmlCleanerOptions.None);
        }

        /// <summary>
        /// Converts the HTML to well-formed HTML with the specified options.
        /// </summary>
        /// <param name="html">The HTML to clean.</param>
        /// <param name="options">The options.</param>
        /// <returns>The well-formed HTML converted from <paramref name="html"/>.</returns>
        public string Clean(string html, HtmlCleanerOptions options)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            string strXml = string.Empty;

            #region  Comments, scripts and styles

            string currentHtml = string.Empty;

            // 0. Base64 encodes CDATA
            html = _regexCDATA.Replace(html, new MatchEvaluator(CdataEncodeBase64));

            // 1. Removes <!DOCTYPE
            html = _regexDOCTYPE.Replace(html, "");

            currentHtml = html;
            // Removes comments
            html = _regexComment.Replace(html, "");

            // Removes xmlns attributes
            html = _regexXmlns.Replace(html, "<html>");

            // 2. Wraps the script in <![CDATA[　]]>
            if ((options & HtmlCleanerOptions.RemoveScripts) == HtmlCleanerOptions.RemoveScripts)
            {
                html = _regexScript.Replace(html, "");
            }
            else
            {
                html = _regexScript.Replace(html, new MatchEvaluator(ScriptToCdata));
            }

            // Wraps the styles in <![CDATA[　]]>
            if ((options & HtmlCleanerOptions.RemoveStyles) == HtmlCleanerOptions.RemoveStyles)
            {
                html = _regexStyle.Replace(html, "");
            }
            else
            {
                html = _regexStyle.Replace(html, new MatchEvaluator(StyleToCdata));
            }

            #endregion

            #region Whitespaces, <, > and &

            // 3. Escapes < and > to &lt; and &gt;
            html = _regexScriptLtGt.Replace(html, new MatchEvaluator(ConvertTagEdgeInScript));

            // 4. Converts &nbsp; to whitespace
            html = _regexNbsp.Replace(html, " ");

            // 5. Escapes & to &amp;
            html = _regexAmp.Replace(html, "&amp;");

            #endregion

            #region Attributes

            // 6. Double quotes attribute values with ""
            html = _regexAttributeValue.Replace(html, new MatchEvaluator(QuoteAttributeValue));

            // Converts dumb attributes (attributes with no value)
            html = _regexDumbAttributeValue.Replace(html, new MatchEvaluator(SetDumbAttributeValue));

            // Splits attributes with a space
            html = _regexAttributeSpace.Replace(html, new MatchEvaluator(SplitAttributeWithSpace));

            // Converts tag name to lower case
            html = _regexUppercase.Replace(html, new MatchEvaluator(TagNameToLower));

            currentHtml = html;
            // Removes invalid attributes
            html = _regexInvalidAttribute.Replace(html, new MatchEvaluator(match => DeleteInvalidAttribute(match, currentHtml)));

            // Removes duplicated attributes
            html = _regexDuplicatedAttribute.Replace(html, new MatchEvaluator(RemoveDuplicatedAttribute));
         
            #endregion

            // Removes duplicated white spaces
            html = _regexSpace.Replace(html, " ");

            // Completes end tags
            html = _regexUnclosedTag.Replace(html, new MatchEvaluator(CompleteEndTag));

            // Removes illegal characters
            html = _regexIllegalChar.Replace(html, "");

            // Checks tags closed
            html = CheckTagValid(html);

            // 8. Base64 encodes CDATA
            html = _regexCDATA.Replace(html, new MatchEvaluator(CdataDecodeBase64));

            // 9. Adds XML header
            strXml = XML + html;
            return strXml;
        }

        private string CdataEncodeBase64(Match mat)
        {
            string str = string.Empty;
            if (mat.Groups[2].Value.Length > 0)
            {
                var base64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(mat.Groups[2].Value));
                str = mat.Groups[1].Value + base64 + mat.Groups[3].Value;
            }
            else
            {                
                str = mat.Value;
            }
            return str;
        }

        private string CdataDecodeBase64(Match mat)
        {
            string str = string.Empty;
            if (mat.Groups[2].Value.Length > 0)
            {
                var decode = Convert.FromBase64String(mat.Groups[2].Value);
                var cdata = Encoding.Unicode.GetString(decode, 0, decode.Length);
                str = mat.Groups[1].Value + cdata + mat.Groups[3].Value;
            }
            else
            {
                str = mat.Value;
            }
            return str;
        }


        // Wraps script in CDATA
        private string ScriptToCdata(Match mat)
        {
            string str = string.Empty;
            if (mat.Groups[2].Value.Length > 0)
            {
                str = mat.Value;
                if (str.ToUpper().IndexOf("<![CDATA[") < 0)
                {
                    var base64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(mat.Groups[2].Value));
                    str = mat.Groups[1].Value + "\n<![CDATA[" + base64 + "]]>\n" + mat.Groups[3].Value;
                }
            }
            else
            {
                str = mat.Value;
            }
            return str;
        }

        // Wraps style in CDATA
        private string StyleToCdata(Match mat)
        {
            string str = string.Empty;
            if (mat.Groups[2].Value.Length > 0)
            {
                var base64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(mat.Groups[2].Value));
                str = mat.Groups[1].Value + "\n<![CDATA[" + base64 + "]]>\n" + mat.Groups[3].Value;
            }
            else
            {
                str = mat.Value;
            }
            return str;
        }

    
        // Double quotes XML attributes
        private string QuoteAttributeValue(Match mat)
        {
            string str = mat.Value.ToString();
            int nCodeSPos = 0;
            int nCodeEPos = 0;
            int nScriptSPos = 0;
            int nScriptEPos = 0;

            // Escapes " to &quot;  e.g. doClick("paperbutton",event)"
            Regex rgx;
            rgx = new Regex("\\(\\s*\"", RegexOptions.IgnoreCase);
            str = rgx.Replace(str, "(&quot;");
            rgx = new Regex("\"\\s*,", RegexOptions.IgnoreCase);
            str = rgx.Replace(str, "&quot;,");
            rgx = new Regex("\"\\s*\\)", RegexOptions.IgnoreCase);
            str = rgx.Replace(str, "&quot;)");

            // Doesn't escape those in script
            nCodeSPos = str.IndexOf("<![CDATA[");
            if (nCodeSPos > 0)
            {
                nCodeEPos = str.IndexOf("]]>");
                if (nCodeSPos < mat.Groups[1].Index && mat.Groups[1].Index < nCodeEPos)
                {
                    return str;
                }
            }
            nScriptSPos = str.IndexOf("\"javascript");
            if (nScriptSPos > 0)
            {
                nScriptEPos = str.IndexOf("\"", nScriptSPos + 1);

            }

            int nStart = 0;
            while (true)
            {
                int nPos = 0;
                nPos = str.IndexOf('=', nStart);
                if (nPos < 0)
                {
                    break;
                }
                string strRemain = str.Substring(nPos + 1);
                // strRemain = strRemain.TrimStart(' ');
                if (strRemain.TrimStart(' ').StartsWith("\""))
                {
                    // quoted
                    nStart = str.IndexOf("\"", nPos + 2) + 1;
                    continue;
                }
                if ((nPos > nScriptSPos && nPos < nScriptEPos)
                    || (nPos > nCodeSPos && nPos < nCodeEPos))
                {
                    // in script
                    nStart = nPos + 1;
                    continue;
                }

                // needs quotes
                string strL = str.Substring(0, nPos + 1);
                string strR = string.Empty;
                string strM = string.Empty;
                int nE = 0;
                if (strRemain.StartsWith("'"))
                {
                    // single quoted
                    nE = str.IndexOf('\'', nPos + 2);
                    strM = str.Substring(nPos + 2, nE - nPos - 2);
                    strR = str.Substring(nE + 1);
                }
                else
                {
                    // not quoted
                    nE = str.IndexOf(' ', nPos + 2);
                    if (nE < 0)
                    {
                        // end of tag
                        nE = str.Length - 1;
                        if (str.EndsWith("/>"))
                        {
                            // end with />
                            nE = nE - 1;
                        }
                        strR = str.Substring(nE);
                    }
                    else
                    {
                        strR = str.Substring(nE + 1);
                    }
                    strM = str.Substring(nPos + 1, nE - nPos - 1);
                }
                // splits with space
                str = strL + "\"" + strM + "\" " + strR.TrimStart(' ');
                nStart = nE + 1;
            }
            return str;
        }

        // A attributes must have a value 
        // Invalid: <script attr="dd" attrInvalid attr2="value">  <tag attr="value" defer>
        private string SetDumbAttributeValue(Match mat)
        {
            string str = mat.Value.ToString();
            Regex rgx = new Regex("=\\s+\"", RegexOptions.IgnoreCase);
            str = rgx.Replace(str, "=\"");
            rgx = new Regex(@"\s{2,}", RegexOptions.IgnoreCase);
            str = rgx.Replace(str, " ");
            string[] att = str.Split(' ');
            str = att[0];
            // The first is tag name
            for (int i = 1; i < att.Length; i++)
            {
                string strAtt = att[i];
                if (strAtt.IndexOf("=\"") < 0)
                {
                    if (i != att.Length - 1)
                    {
                        strAtt = strAtt + "=\"NoValue\"";
                    }
                    else
                    {
                        // the last attribute
                        strAtt = strAtt.TrimEnd('>');
                        if (strAtt.Length > 1)
                        {
                            strAtt = strAtt + "=\"NoValue\">";
                        }
                        else
                        {
                            strAtt = strAtt + ">";
                        }
                    }
                }
                str = str + " " + strAtt;
            }
            return str;
        }

        // Removes attributes with no value     
        private string DeleteInvalidAttribute(Match mat, string currentHtml)
        {
            int nCDataS = currentHtml.LastIndexOf("<![CDATA[", mat.Index);
            int nCDataE = 0;
            if (nCDataS > 0)
            {
                nCDataE = currentHtml.IndexOf("]]>", nCDataS);
            }
            if (nCDataS < mat.Index && mat.Index < nCDataS)
            {
                // in script
                return mat.Value;
            }

            string remain = mat.Value;
            string str = "";
            Regex rgx1 = new Regex(@"<\w+\s+(\w+\s*=\s*""[^""]*""\s*)*", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);

            Match mat1 = rgx1.Match(remain);
            str = mat1.Value;
            int nStart = mat1.Value.Length + 1;
            Regex rgx2 = new Regex(@"(\w+\s*=\s*""[^""]*""\s*)+", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            while (true)
            {
                remain = remain.Substring(nStart);
                Match mat2 = rgx2.Match(remain);
                if (mat2.Success)
                {
                    str += " " + mat2.Value.TrimStart(' ');
                    nStart = mat2.Index + mat2.Value.Length;
                }
                else
                {
                    // end of tag
                    if (remain.EndsWith("/>"))
                    {
                        str += "/>";
                    }
                    else if (remain.EndsWith(">"))
                    {
                        str += ">";
                    }
                    break;
                }

            }
            return str;
        }
             
        // Escapes < and > to &lt; and &gt; in script
        private string ConvertTagEdgeInScript(Match mat)
        {
            string str = mat.Value;
            string strRegex = "<";
            Regex rgx = new Regex(strRegex, RegexOptions.IgnoreCase);
            str = rgx.Replace(str, "&lt;");
            strRegex = ">";
            rgx = new Regex(strRegex, RegexOptions.IgnoreCase);
            str = rgx.Replace(str, "&gt;");
            return str;
        }

        // Completes end tags
        private string CompleteEndTag(Match mat)
        {
            return mat.Value + ">";
        }

        // Removes duplicated attributes
        // e.g. <a class="videoNew" href="" target="_blank" class="video">
        private string RemoveDuplicatedAttribute(Match mat)
        {
            string str = "";
            Dictionary<string,int> attr = new Dictionary<string, int>();
            Regex rgx = new Regex(@"<\w+\s+(\w+)\s*=\s*""[^""]*""", RegexOptions.IgnoreCase);
            Match mat2 = rgx.Match(mat.Value);
            str = mat2.Value;
            attr.Add(mat2.Groups[1].Value, 1);
            int nPos = mat2.Length + 1;
            rgx = new Regex(@"(\w+)(\s*=\s*""[^""]*"")", RegexOptions.IgnoreCase);
            while (true)
            {
                string strRemains = mat.Value.Substring(nPos);
                Match matN = rgx.Match(strRemains);
                if (matN.Success)
                {
                    if (attr.ContainsKey(matN.Groups[1].Value))
                    {
                        // attribute existed
                        attr[matN.Groups[1].Value] = Convert.ToInt16(attr[matN.Groups[1].Value]) + 1;
                        str += " " + matN.Groups[1].Value + attr[matN.Groups[1].Value].ToString() + matN.Groups[2].Value;
                    }
                    else
                    {
                        str += " " + matN.Value;
                        attr.Add(matN.Groups[1].Value, 1);
                    }
                    nPos += matN.Value.Length;
                }
                else
                {
                    if (strRemains.EndsWith("/>"))
                    {
                        str += "/>";
                    }
                    else
                    {
                        str += ">";
                    }
                    break;
                }
            }
            return str;
        }
        
        // Converts tag names to lower case
        private string TagNameToLower(Match mat)
        {
            return mat.Groups[1].Value + mat.Groups[2].Value.ToLower() + mat.Groups[3].Value;
        }

        // Splits attributes with a space
        private string SplitAttributeWithSpace(Match mat)
        {
            string str = mat.Value;
            int nPos = 0;
            while (nPos < str.Length)
            {
                int nS = str.IndexOf("\"", nPos);
                if (nS < 0)
                {
                    break;
                }
                int nE = str.IndexOf("\"", nS + 1);
                if (str[nE + 1] != ' ')
                {
                    str = str.Substring(0, nE + 1) + ' ' + str.Substring(nE + 1);
                }
                nPos = nE + 1;
            }
            return str;
        }

        // Check whether a tag is closed
        private string CheckTagValid(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }

            html = CheckTagIntegral(html);

            Regex rgx = new Regex(@"<(?=[^>]*<)|<(?![/a-z\^!]+)");
            html = rgx.Replace(html, "&lt;");

            rgx = new Regex(@"(?<![\a-z\s""/])>");
            html = rgx.Replace(html, "&gt;");

            // Removes the flag ^
            rgx = new Regex(@"(?<=<)\^(?=/?(D-)?[a-z]+\d?)");
            html = rgx.Replace(html, "");

            rgx = new Regex(@"(?<=</[a-z]+\d?)>\s*|(?<=<[a-z]+\d?\s+[^>]*/)>\s*");
            html = rgx.Replace(html, ">\r\n");

            return html;
        }

        private string CheckTagIntegral(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            html = html.Trim();
            string MatchedFlag = "^";

            string SearchTag = "";
            // Searches the end tag
            SearchTag = @"</(?<Tag>\w+\d?)\s*>";
            Regex rgxEnd = new Regex(SearchTag, RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            int nMatchTime = 0;
            while (true)
            {
                nMatchTime++;
                Match matEnd = rgxEnd.Match(html);
                if (!matEnd.Success)
                {
                    break;
                }

                // Finds the last start tag before the end tag
                Regex rgxStart = new Regex(@"<(?<Tag>[a-z]+\d?)\s*>|<(?<Tag>[a-z]+\d?)\s+[^>]*[^/]>", RegexOptions.RightToLeft);
                Match matPreS = rgxStart.Match(html, matEnd.Index);

                if (matPreS.Success)
                {
                    // The start tag found
                    if (matEnd.Groups["Tag"].Value == matPreS.Groups["Tag"].Value)
                    {
                        // The start tag and end tag matches
                        html = html.Insert(matPreS.Index + 1, MatchedFlag).Insert(matEnd.Index + 2, MatchedFlag);
                    }
                    else
                    {
                        // Finds the unmatched tag
                        string[] arUnMatchTag = new string[2];
                        int[] arUnMatchTagPos = new int[2];
                        string RearPart = html.Substring(matEnd.Index + matEnd.Value.Length);
                        string NeedTag = matPreS.Groups["Tag"].Value;
                        RearPart = GetUnmatchedEndTag(RearPart, NeedTag, 2, ref arUnMatchTag, ref arUnMatchTagPos);
                        if (arUnMatchTag[0] == NeedTag)
                        {
                            // matEnd is unmatched
                            html = html.Substring(0, matEnd.Index)
                                + RepairBadTag(matEnd.Value, matEnd.Groups["Tag"].Value)
                                + RearPart;
                        }
                        else
                        {
                            Match matPreS2 = matPreS.NextMatch();
                            if (matEnd.Groups["Tag"].Value == matPreS2.Groups["Tag"].Value)
                            {
                                // matPreS is unmatched
                                string DelTag = matPreS.Value;
                                // DelTag = DelTag.Insert(1, "^D-");
                                // DelTag = DelTag.Insert(DelTag.Length - 1, "/");
                                DelTag = RepairBadTag(matPreS.Value, matPreS.Groups[1].Value);
                                html = html.Substring(0, matPreS2.Index)
                                           + matPreS2.Value.Insert(1, MatchedFlag)
                                           + html.Substring(matPreS2.Index + matPreS2.Value.Length, matPreS.Index - matPreS2.Index - matPreS2.Value.Length)
                                           + DelTag
                                           + html.Substring(matPreS.Index + matPreS.Value.Length, matEnd.Index - matPreS.Index - matPreS.Value.Length)
                                           + matEnd.Value.Insert(1, MatchedFlag)
                                           + RearPart;
                            }
                            else if (arUnMatchTag[0] == matPreS2.Groups["Tag"].Value)
                            {
                                // both are unmatched
                                string Mid = html.Substring(matPreS.Index + matPreS.Value.Length, matEnd.Index - matPreS.Index - matPreS.Value.Length);
                                string DelPreS = RepairBadTag(matPreS.Value, matPreS.Groups[1].Value);
                                string DelEnd = RepairBadTag(matEnd.Value, matEnd.Groups[1].Value);
                                html = html.Substring(0, matPreS.Index)
                                    + DelPreS
                                    + Mid
                                    + DelEnd
                                    + RearPart;
                            }
                            else if (arUnMatchTag[1] == NeedTag)
                            {
                                // matEnd and the first end tag with no start tag after matEnd are unmatched
                                html = html.Substring(0, matEnd.Index)
                                           //+ "<^D-" + matEnd.Groups["Tag"].Value + "/>"
                                           + RepairBadTag(matEnd.Value, matEnd.Groups[1].Value)
                                           + RearPart.Insert(arUnMatchTagPos[1] + 1, "^D-");

                            }
                            else if ("body" == NeedTag || "html" == NeedTag || "form" == NeedTag)
                            {
                                // matEnd is unmatched
                                html = html.Substring(0, matEnd.Index)
                                    + RepairBadTag(matEnd.Value, matEnd.Groups["Tag"].Value)
                                    + RearPart;
                            }
                            else
                            {
                                string DelTag = RepairBadTag(matPreS.Value, matPreS.Groups[1].Value);
                                html = html.Substring(0, matPreS.Index)
                                    + DelTag
                                    + html.Substring(matPreS.Index + matPreS.Value.Length);
                            }
                        }
                    }
                }
                else
                {
                    // no start tag. matEnd is unmatched. Marks prefix "D-" for removing
                    html = html.Substring(0, matEnd.Index) + "<^D-" + matEnd.Groups["Tag"].Value + "/>" + html.Substring(matEnd.Index + matEnd.Value.Length);
                }
            }
            return html;
        }

        private string GetUnmatchedEndTag(string html, string firstNeededTag, int nGetNum, ref string[] arUnmatchTag, ref int[] UnmatchTagPos)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            int nFound = 0;
            Regex rgxEnd = new Regex(@"</(?<Tag>\w+\d?)\s*>", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            int maxTest = 100;
            while (nFound < nGetNum && maxTest > 0)
            {
                Match matEnd = rgxEnd.Match(html);
                if (!matEnd.Success)
                {
                    break;
                }
                if (html.Substring(0, matEnd.Index).Trim().Length < 1)
                {
                    // The first is an end tag
                    arUnmatchTag[nFound] = matEnd.Groups["Tag"].Value;
                    UnmatchTagPos[nFound] = matEnd.Index;
                    return html;
                }
                // Finds the last start tag before the end tag
                Regex rgxStart = new Regex(@"<(?<Tag>[a-z]+\d?)\s*>|<(?<Tag>[a-z]+\d?)\s+[^>]*[^/]>", RegexOptions.RightToLeft);
                Match matPreS = rgxStart.Match(html, matEnd.Index);
                if (!matPreS.Success)
                {
                    html = html.Insert(matEnd.Index + 1, "#");
                    arUnmatchTag[nFound] = matEnd.Groups["Tag"].Value;
                    UnmatchTagPos[nFound] = matEnd.Index;
                    nFound++;

                }
                else if (matPreS.Groups["Tag"].Value == matEnd.Groups["Tag"].Value)
                {
                    html = html.Insert(matPreS.Index + 1, "^").Insert(matEnd.Index + 1 + 1, "^");
                }
                else if (matEnd.Groups["Tag"].Value == firstNeededTag)
                {
                    string DelTag = matPreS.Value.Insert(1, "^D-");
                    DelTag = DelTag.Insert(DelTag.Length - 1, "/");
                    html = html.Substring(0, matPreS.Index)
                              + DelTag
                              + html.Substring(matPreS.Index + matPreS.Value.Length);
                }
                maxTest--;
            }
            Regex rgx = new Regex("(?<=<)#");
            html = rgx.Replace(html, "");
            return html;
        }

        private string RepairBadTag(string TagData, string TagName)
        {
            if (TagData.StartsWith("</"))
            {
                // The end tag, temporarily marked unclosed
                TagData = "<" + TagData.Substring(2);
            }
            if ("input" == TagName.ToLower()
                || "img" == TagName.ToLower()
                || "br" == TagName.ToLower()
                || "link" == TagName.ToLower()
                || "meta" == TagName.ToLower())
            {
                TagData = TagData.Insert(1, "^");
            }
            else
            {
                TagData = TagData.Insert(1, "^D-");
            }
            TagData = TagData.Insert(TagData.Length - 1, "/");
            return TagData;
        }
    }   
}
