using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
//
namespace ERP_Core.Common
{
    public static class StringHelpers
    {
   
        public static String ToTitleCase(String input)
        {
            String returnValue = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
            return returnValue;
        }

        public static String UpperFirstChar(String str)
        {
            StringBuilder builder = new StringBuilder();

            //bo di ky tu dau tien
            String partOfStr = str.Substring(1, str.Length - 1);
            //lay ky tu dau tien va chuyen thanh ky tu hoa
            String upperFirstChar = str.Substring(0, 1).ToUpper();

            return upperFirstChar + partOfStr;
        }

        public static String Replace(String s, Char[] replaceChars, Char replaceBy)
        {
            for (int i = 0; i < replaceChars.Length; i++)
            {
                s = s.Replace(replaceChars[i], replaceBy);
            }
            return s;
        }
        public static string ReplaceVietnameseChar(String s)
        {

            string[] vietnameseSigns = new string[]{
                "_aAeEoOuUiIdDyY",//replace char
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"};
            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                s = Replace(s, vietnameseSigns[i].ToCharArray(), vietnameseSigns[0][i]);
            }
            return s;
        }
    }
}
