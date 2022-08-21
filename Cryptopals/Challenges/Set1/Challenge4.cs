using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge4 : BaseChallenge
    {
        private const string FileName = "4.txt";

        public Challenge4(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var fileUtils = new ImportFileUtilities(FileName);
            var hexLines = fileUtils.ReadFile();

            var result = string.Empty;
            var bestRating = 0.0;

            foreach (var hexLine in hexLines)
            {
                var crypto = new CryptographyDataContext(hexLine);
                crypto.GenerateBruteForceXor();

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
            }

            OutputResult(Answers.CHALLENGE_4, result);
        }
    }
}
