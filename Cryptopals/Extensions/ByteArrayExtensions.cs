namespace Cryptopals.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] value) => BitConverter.ToString(value).Replace("-", "");
    }
}
