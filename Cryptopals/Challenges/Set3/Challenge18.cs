using System.Text;
using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set3
{
    public class Challenge18 : BaseChallenge
    {
        private const string INPUT = "L77na/nrFsKvynd6HzOoG7GHTLXsTVu9qvY/2syLXzhPweyyMTJULu/6/kXX0KSvoOLSFQ==";
        private const string KEY = "YELLOW SUBMARINE";
        private const int NONCE = 0;

        private readonly byte[] _key;

        public Challenge18(int index) : base(index)
        {
            _key = StringUtilities.ConvertPlaintextToBytes(KEY);
        }

        public override bool Execute()
        {
            var bytes = Convert.FromBase64String(INPUT);
            var ctr = new CTRDataContext(bytes, _key, NONCE);
            var ctrResult = ctr.CryptBlocks();
            var result = Encoding.ASCII.GetString(ctrResult);

            return OutputResult(Answers.CHALLENGE_18, result);
        }
    }
}
