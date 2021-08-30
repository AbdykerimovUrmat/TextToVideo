using System.Text.Json;

namespace Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object o)
        {
            return JsonSerializer.Serialize(o);
        }

        public static bool IsNull(this object o)
        {
            return o == null;
        }

        public static bool IsNotNull(this object o)
        {
            return !IsNull(o);
        }
    }
}
