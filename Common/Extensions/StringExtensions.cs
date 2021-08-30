using System.Text.RegularExpressions;
using Common.Exceptions;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string str)
        {
            if(str.IsNullOrEmpty())
            {
                throw new InnerException("str should not be empty", "10002");
            }
            Regex r = new(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            return r.Match(str).Success;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return (str.IsNull() || str == "");
        }
    }
}
