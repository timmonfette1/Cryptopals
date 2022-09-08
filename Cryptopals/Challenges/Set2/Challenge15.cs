using System.Text;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge15 : BaseChallenge
    {
        private const string INPUT = "ICE ICE BABY\x04\x04\x04\x04";

        // These are invalid PCKS7, comment them in for negative testing.
        //private const string INPUT = "ICE ICE BABY\x05\x05\x05\x05";
        //private const string INPUT = "ICE ICE BABY\x01\x02\x03\x04";

        public Challenge15(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var bytes = StringUtilities.ConvertPlaintextToBytes(INPUT);
            var result = Encoding.ASCII.GetString(bytes.PKCS7Strip());

            return OutputResult(Answers.CHALLENGE_15, result);
        }
    }
}
