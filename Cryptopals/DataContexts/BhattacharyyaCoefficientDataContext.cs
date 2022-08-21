using System.Text;
using Cryptopals.Utilities;

namespace Cryptopals.DataContexts
{
    public class BhattacharyyaCoefficientDataContext
    {
        private readonly string _hex;

        // http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html (including space character)
        private readonly IDictionary<char, double> _letterScore = new Dictionary<char, double>
        {
            {'A', 8.12}, {'B', 1.49}, {'C', 2.71}, {'D', 4.32}, {'E', 12.02}, {'F', 2.30},
            {'G', 2.03}, {'H', 5.92}, {'I', 7.31}, {'J', 0.10}, {'K', 0.69}, {'L', 3.98},
            {'M', 2.61}, {'N', 6.95}, {'O', 7.68}, {'P', 1.82}, {'Q', 0.11}, {'R', 6.02},
            {'S', 6.28}, {'T', 9.10}, {'U', 2.88}, {'V', 1.11}, {'W', 2.09}, {'X', 0.17},
            {'Y', 2.11}, {'Z', 0.07}, {' ', 0.19}
        };

        public BhattacharyyaCoefficientDataContext(string hex)
        {
            _hex = hex;
        }

        public (double, string) GetEnglishRating()
        {
            var hexBytes = StringUtilities.ConvertHexToBytes(_hex);
            var ascii = Encoding.ASCII.GetString(hexBytes);
            var chars = ascii.ToUpper().GroupBy(c => c).Select(g => new { g.Key, Count = g.Count() });

            double coefficient = 0;

            foreach (var c in chars)
            {
                if (_letterScore.TryGetValue(c.Key, out var freq))
                {
                    coefficient += Math.Sqrt(freq * c.Count / ascii.Length);
                }
            }

            return (coefficient, ascii);
        }
    }
}
