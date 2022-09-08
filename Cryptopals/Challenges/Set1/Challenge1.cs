using Cryptopals.DataContexts;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge1 : BaseChallenge
    {
        private const string HEX = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";

        public Challenge1(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var crypto = new CryptographyDataContext(HEX);
            var result = Convert.ToBase64String(crypto.Bytes);

            return OutputResult(Answers.CHALLENGE_1, result);
        }
    }
}
