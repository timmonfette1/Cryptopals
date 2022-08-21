using System.Text;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge9 : BaseChallenge
    {
        private const string Key = "YELLOW SUBMARINE";

        public Challenge9(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var bytes = StringUtilities.ConvertPlaintextToBytes(Key);
            bytes = bytes.PKCS7Padding(4, 20);

            var result = Encoding.ASCII.GetString(bytes);
            OutputResult(Answers.CHALLENGE_9, result);
        }
    }
}
