using System;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string str)
        {
            if(str.IsNullOrEmpty())
            {
                throw new Exception("str should not be empty");
            }
            Regex r = new(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            return r.Match(str).Success;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return (str == null || str == "");
        }
    }
}
