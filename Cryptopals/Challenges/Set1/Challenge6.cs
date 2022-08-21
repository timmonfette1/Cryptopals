using System.Text;
using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge6 : BaseChallenge
    {
        private const string FileName = "6.txt";

        public Challenge6(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var fileUtils = new ImportFileUtilities(FileName);
            var data = fileUtils.ReadFileAsString();
            var bytes = Convert.FromBase64String(data);
            var key = CreateKey(bytes);

            var crypto = new CryptoDataContext(bytes, key);
            crypto.ExpandKey();
            var output = crypto.Xor();
            var result = Encoding.ASCII.GetString(output);

            OutputResult(Answers.CHALLENGE_6, result);
        }

        #region Private Methods

        private static byte[] CreateKey(byte[] data)
        {
            var crypto = new CryptoDataContext(data);
            var keysize = GetSmallestKeysize(crypto);
            var blocks = CreateBlocks(data, keysize);
            blocks = Transpose(blocks);

            var bruteForcedResults = blocks
                .Select(block =>
                {
                    var crypto = new CryptoDataContext(block);
                    crypto.GenerateBruteForceXor();
                    return crypto.BruteForcedXor;
                })
                .ToList();

            var key = string.Empty;
            foreach (var bfResult in bruteForcedResults)
            {
                var bestRating = 0.0;
                var bestKey = string.Empty;

                foreach (var kvp in bfResult)
                {
                    var bcoef = new BhattacharyyaCoefficientDataContext(kvp.Value);
                    var (rating, _) = bcoef.GetEnglishRating();

                    if (rating > bestRating)
                    {
                        bestRating = rating;
                        bestKey = kvp.Key;
                    }
                }

                key += bestKey;
            }

            return StringUtilities.ConvertHexToBytes(key);
        }

        private static int GetSmallestKeysize(CryptoDataContext crypto)
        {
            var smallestKeysize = 0;
            var smallestDistance = 0;
            for (int keysize = 2; keysize <= 40; keysize++)
            {
                var comparisons = 0;
                var distance = 0;
                for (int i = 1; i < crypto.Bytes.Length / keysize; i++)
                {
                    distance += crypto.HammingDistance(keysize, i);
                    comparisons++;
                }

                var normalizedDistance = distance / comparisons / keysize;

                if (smallestKeysize == 0 || normalizedDistance < smallestDistance)
                {
                    smallestKeysize = keysize;
                    smallestDistance = normalizedDistance;
                }
            }

            return smallestKeysize;
        }

        private static byte[][] CreateBlocks(byte[] data, int keysize)
        {
            var chunked = 0;
            var result = new List<byte[]>();
            while (chunked < data.Length)
            {
                var chunk = data.Skip(chunked).Take(keysize).ToArray();
                result.Add(chunk);
                chunked += keysize;
            }

            return result.ToArray();
        }

        private static byte[][] Transpose(byte[][] data)
        {
            var result = new List<List<byte>>();

            for (var i = 0; i < data.Length; i++)
            {
                foreach (var block in data)
                {
                    if (i < block.Length)
                    {
                        if (result.ElementAtOrDefault(i) == null)
                        {
                            result.Add(new List<byte>());
                        }

                        result[i].Add(block[i]);
                    }
                }
            }

            return result.Select(x => x.ToArray()).ToArray();
        }

        #endregion Private Methods
    }
}
