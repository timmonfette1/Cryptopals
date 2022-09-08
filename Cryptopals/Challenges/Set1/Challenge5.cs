using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge5 : BaseChallenge
    {
        private const string INPUT = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
        private const string KEY = "ICE";

        public Challenge5(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var inputBytes = StringUtilities.ConvertPlaintextToBytes(INPUT);
            var keyBytes = StringUtilities.ConvertPlaintextToBytes(KEY);

            var crypto = new CryptographyDataContext(inputBytes, keyBytes);
            crypto.ExpandKey();
            var result = crypto.Xor();

            return OutputResult(Answers.CHALLENGE_5, result.ToHexString());
        }
    }
}
