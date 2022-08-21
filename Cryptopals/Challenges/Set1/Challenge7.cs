using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge7 : BaseChallenge
    {
        private const string FileName = "7.txt";
        private const string Key = "YELLOW SUBMARINE";

        public Challenge7(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var fileUtils = new ImportFileUtilities(FileName);
            var data = fileUtils.ReadFileAsString();

            var aes = new AesDataContext(Convert.FromBase64String(data), StringUtilities.ConvertPlaintextToBytes(Key));
            var result = aes.Decrypt();

            OutputResult(Answers.CHALLENGE_7, result);
        }
    }
}
