namespace Cryptopals.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int SequenceCount<T>(this IEnumerable<T> source, IEnumerable<T> input)
        {
            var result = 0;
            var minimum = Math.Min(source.Count(), input.Count());

            while (source.ElementAt(result).Equals(input.ElementAt(result)) && result < minimum)
            {
                result++;
            }

            return result;
        }
    }
}
