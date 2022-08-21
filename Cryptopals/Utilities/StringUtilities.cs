using System.Text;
using Cryptopals.Extensions;

namespace Cryptopals.Utilities
{
    public static class StringUtilities
    {
        public static byte[] ConvertPlaintextToBytes(string value) => ConvertHexToBytes(ConvertToHex(value));

        public static string ConvertToHex(string value) => Encoding.Default.GetBytes(value).ToHexString();

        public static byte[] ConvertHexToBytes(string value)
        {
            var result = new byte[value.Length / 2];
            for (var i = 0; i < value.Length; i += 2)
            {
                result[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
            }

            return result;
        }
    }
}
