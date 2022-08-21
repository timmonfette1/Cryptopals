﻿using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge4 : BaseChallenge
    {
        public Challenge4(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var fileUtils = new ImportFileUtilities("4.txt");
            var hexLines = fileUtils.ReadFile();

            var result = string.Empty;
            var bestRating = 0.0;

            foreach (var hexLine in hexLines)
            {
                var crypto = new CryptoDataContext(hexLine);
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
