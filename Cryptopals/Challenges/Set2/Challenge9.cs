using System.Text;
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
            var bytes = new List<byte>(StringUtilities.ConvertPlaintextToBytes(Key));
            while (bytes.Count < 20)
            {
                bytes.Add(4);
            }

            var result = Encoding.ASCII.GetString(bytes.ToArray());
            OutputResult(Answers.CHALLENGE_9, result);
        }
    }
}
