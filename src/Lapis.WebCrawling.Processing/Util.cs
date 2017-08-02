/********************************************************************************
 * Module      : Lapis.WebCrawling.Processing
 * Class       : Util
 * Description : Provides commonly used methods.
 * Created     : 2016/1/27
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Processing
{
    internal static class Util
    {
        #region String

        public static bool IsValidIdentifier(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            else if (!char.IsLetter(value[0]) && value[0] != '_')
            {
                return false;
            }
            else
            {
                foreach (char c in value)
                {
                    if (!char.IsLetter(c) && !char.IsDigit(c) && c != '_' && c != '-' && c != '.')
                        return false;
                }
            }
            return true;
        }

        #endregion
    }
}
