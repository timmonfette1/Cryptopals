using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge7 : BaseChallenge
    {
        private const string key = "YELLOW SUBMARINE";

        public Challenge7(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var fileUtils = new ImportFileUtilities("7.txt");
            var data = fileUtils.ReadFileAsString();

            var aes = new AesDataContext(Convert.FromBase64String(data), StringUtilities.ConvertPlaintextToBytes(key));
            var result = aes.Decrypt();

            OutputResult(Answers.CHALLENGE_7, result);
        }
    }
}
