using System.Reflection;
using System.Text;

namespace Cryptopals.Utilities
{
    public static class CookieUtilities
    {
        private static readonly BindingFlags PropertyFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public static string Encode<T>(T cookie) where T : class, new()
        {
            var result = new StringBuilder();

            var props = cookie.GetType().GetProperties(PropertyFlags);
            for (var i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var value = prop.GetValue(cookie, null).ToString();
                value = value.Replace("&", "").Replace("=", "");
                result.Append($"{prop.Name.ToLower()}={value}");

                if (i + 1 < props.Length)
                {
                    result.Append('&');
                }
            }

            return result.ToString();
        }

        public static T Parse<T>(string cookie) where T : class, new()
        {
            var cookieKvps = cookie
                .Split('&')
                .Select(x => x.Split('='))
                .Select(y => new
                {
                    Key = y[0],
                    Value = y[1]
                });

            var result = new T();
            foreach (var kvp in cookieKvps)
            {
                var prop = result.GetType().GetProperty(kvp.Key, PropertyFlags);
                if (prop != null)
                {
                    var value = Convert.ChangeType(kvp.Value, prop.PropertyType);
                    prop.SetValue(result, value, null);
                }
            }

            return result;
        }
    }
}
