using System.Text;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge9 : BaseChallenge
    {
        private const string KEY = "YELLOW SUBMARINE";

        public Challenge9(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var bytes = StringUtilities.ConvertPlaintextToBytes(KEY);
            bytes = bytes.PKCS7Padding(20);

            var result = Encoding.ASCII.GetString(bytes);
            return OutputResult(Answers.CHALLENGE_9, result);
        }
    }
}
