using Cryptopals.DataContexts;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge1 : BaseChallenge
    {
        private const string Hex = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";

        public Challenge1(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var crypto = new CryptoDataContext(Hex);
            var result = Convert.ToBase64String(crypto.Bytes);

            OutputResult(Answers.CHALLENGE_1, result);
        }
    }
}
