/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : HtmlCleaner
 * Description : Provides methods for clean bad-formed HTML.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lapis.WebCrawling.Parsing
{
    public partial class HtmlCleaner
    {
        // CDATA
        const string CDATA = @"(<!\[CDATA\[)([\s\S]*?)(\]\]>)";
        Regex _regexCDATA= new Regex(CDATA, RegexOptions.IgnoreCase);

        // <!DOCTYPE
        const string DOCTYPE = "<!DOCTYPE[^>]*>";
        Regex _regexDOCTYPE = new Regex(DOCTYPE, RegexOptions.IgnoreCase);

        // Comment
        const string COMMENT = "<!--(?!-->).*?-->";
        Regex _regexComment = new Regex(COMMENT, RegexOptions.IgnoreCase);

        // xmlns attribute
        const string XMLNS = "<html[^>]*>";
        Regex _regexXmlns = new Regex(XMLNS, RegexOptions.IgnoreCase);

        // Script
        const string SCRIPT = @"(<script[^>]*>)([\s\S]*?)(</script>)";
        Regex _regexScript = new Regex(SCRIPT, RegexOptions.IgnoreCase);

        // Style
        const string STYLE = @"(<style[^>]*>)([\s\S]*?)(</style>)";
        Regex _regexStyle = new Regex(STYLE, RegexOptions.IgnoreCase);

        // < and > in script
        const string SCRIPT_LTGT = @"""javascript[^""]*[><]+[^""]*""";
        Regex _regexScriptLtGt = new Regex(SCRIPT_LTGT, RegexOptions.IgnoreCase);

        // space
        const string NBSP = "&nbsp;";
        Regex _regexNbsp = new Regex(NBSP, RegexOptions.IgnoreCase);

        // &
        const string AMP = "&(?!(gt;|lt;quot;|amp;))";
        Regex _regexAmp = new Regex(AMP, RegexOptions.IgnoreCase);

        // Attribute
        const string ATTRIBUTE_VALUE = @"<\w[^><]*[\s'""]\w+\s*=\s*[^""][^>]*>|<\w[^><]*[\s'""]\w+\s*=\s*[^""][^>]*/>";
        Regex _regexAttributeValue = new Regex(ATTRIBUTE_VALUE, RegexOptions.IgnoreCase);

        // Attribute with no value
        const string DUMB_ATTRBUTE_Value = @"<\w+(?:\s+\w+=""[^""]*"")*\s+\w+(?:\s*>|\s+[^>]*?>|\s+(?!/>)*?/>)";
        Regex _regexDumbAttributeValue = new Regex(DUMB_ATTRBUTE_Value, RegexOptions.IgnoreCase);

        // Spaces between attribute
        const string ATTRIBUTE_SPACE = "<\\w+[^>]+\\w+=\"[^\"]*\"\\w+=[^>]*>";
        Regex _regexAttributeSpace = new Regex(ATTRIBUTE_SPACE, RegexOptions.IgnoreCase);

        // Upper case tag name
        const string UPPERCASE = @"(</?)(\w*[A-Z]+\w*)(\s+|>)";
        Regex _regexUppercase = new Regex(UPPERCASE);

        // Invalid attribute
        const string INVALID_ATTRIBUTE = @"<\w+\s+(?:\w+\s*=\s*""[^""]*""\s*)*\s*([^\s=>""]+\s+[^>]*>|[^\s=>""]+\s*/>|[^\s=>/""]+\s*>)";
        Regex _regexInvalidAttribute = new Regex(INVALID_ATTRIBUTE, RegexOptions.IgnoreCase);

        // Duplicated attributes
        const string DUPLICATED_ATTRIBUTE = @"<\w+\s+(?:\w+\s*=\s*""[^""]*""\s*)*(\w+=)""[^""]*""\s+(?:\w+\s*=\s*""[^""]*""\s*)*\s*\1[^>]*/?>";
        Regex _regexDuplicatedAttribute = new Regex(DUPLICATED_ATTRIBUTE, RegexOptions.IgnoreCase);

        // Duplicated spaces
        const string SPACE = @"\s{3,}";
        Regex _regexSpace = new Regex(SPACE);

        // Unclosed tag
        const string UNCLOSED_TAG = @"</\w+(?=<|\s)";
        Regex _regexUnclosedTag = new Regex(UNCLOSED_TAG, RegexOptions.IgnoreCase);

        // Invalid character
        const string ILLEGAL_CHAR = @"(?<=<)[^a-z](?=[a-z]+\s+)";
        Regex _regexIllegalChar = new Regex(ILLEGAL_CHAR, RegexOptions.IgnoreCase);

        // XML header
        const string XML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";
    }
}
