using Cryptopals.DataContexts;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge3 : BaseChallenge
    {
        private const string Hex = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";

        public Challenge3(int index) : base(index)
        {
        }

        public override void Execute()
        {
            var crypto = new CryptoDataContext(Hex);
            crypto.GenerateBruteForceXor();

            var bestRating = 0.0;
            var result = string.Empty;

            foreach (var kvp in crypto.BruteForcedXor)
            {
                var bcoef = new BhattacharyyaCoefficientDataContext(kvp.Value);
                var (rating, ascii) = bcoef.GetEnglishRating();

                if (rating > bestRating)
                {
                    bestRating = rating;
                    result = ascii;
                }
            }

            OutputResult(Answers.CHALLENGE_3, result);
        }
    }
}
