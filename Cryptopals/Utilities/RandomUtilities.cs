namespace Cryptopals.Utilities
{
    public static class RandomUtilities
    {
        private static readonly Random rng = new();

        public static int GetRandomNumber(int minimum, int maximumNonInclusive) => rng.Next(minimum, maximumNonInclusive);

        public static byte[] GetRandomBytes(int length)
        {
            var result = new byte[length];
            rng.NextBytes(result);
            return result;
        }
    }
}
