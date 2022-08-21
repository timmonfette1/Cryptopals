namespace Cryptopals.Comparers
{
    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        private const int p = 16777619;

        public bool Equals(byte[] x, byte[] y)
        {
            if (x == null || y == null || x.Length != y.Length)
            {
                return false;
            }
            else if (ReferenceEquals(x, y))
            {
                return true;
            }
            else
            {
                return x.SequenceEqual(y);
            }
        }

        public int GetHashCode(byte[] obj)
        {
            int hash = unchecked((int)2166136261);

            foreach (byte b in obj)
            {
                hash = (hash ^ b) * p;
            }

            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;

            return hash;
        }
    }
}
