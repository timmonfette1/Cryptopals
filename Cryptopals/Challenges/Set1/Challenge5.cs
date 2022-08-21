using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge5 : BaseChallenge
    {
        private const string input = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
        private const string key = "ICE";

        public Challenge5(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var inputBytes = StringUtilities.ConvertPlaintextToBytes(input);
            var keyBytes = StringUtilities.ConvertPlaintextToBytes(key);

            var crypto = new CryptoDataContext(inputBytes, keyBytes);
            crypto.ExpandKey();
            var result = crypto.Xor();

            OutputResult(Answers.CHALLENGE_5, result.ToHexString());
        }
    }
}
