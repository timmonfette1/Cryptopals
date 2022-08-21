using Cryptopals.DataContexts;
using Cryptopals.Extensions;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge2 : BaseChallenge
    {
        public const string Hex1 = "1c0111001f010100061a024b53535009181c";
        public const string Hex2 = "686974207468652062756c6c277320657965";

        public Challenge2(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var crypto1 = new CryptographyDataContext(Hex1);
            var crypto2 = new CryptographyDataContext(Hex2);

            var result = crypto1.Xor(crypto2);
            OutputResult(Answers.CHALLENGE_2, result.ToHexString());
        }
    }
}
