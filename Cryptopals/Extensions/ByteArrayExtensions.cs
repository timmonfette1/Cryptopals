namespace Cryptopals.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] value) => BitConverter.ToString(value).Replace("-", "");

        public static byte[][] SplitIntoBlocks(this byte[] source, int blockSize)
        {
            var chunked = 0;
            var result = new List<byte[]>();
            while (chunked < source.Length)
            {
                var chunk = source.Skip(chunked).Take(blockSize).ToArray();
                result.Add(chunk);
                chunked += blockSize;
            }

            return result.ToArray();
        }

        public static byte[] PKCS7Padding(this byte[] source, int length)
        {
            var mod = length - source.Length % length;
            var padding = Enumerable.Repeat((byte)mod, mod).ToArray();
            var result = source.Concat(padding).ToArray();
            return result;
        }

        public static byte[] PKCS7Strip(this byte[] source)
        {
            if (source.Length == 0)
            {
                return source;
            }

            byte last = source.Last();
            if (last >= 1 && last <= 16 && source.Skip(source.Length - last).All(x => x == last))
            {
                return source.Take(source.Length - last).ToArray();
            }
            else
            {
                throw new ArgumentException("The provided Byte Array does not use PKCS7 Padding.", nameof(source));
            }
        }
    }
}
