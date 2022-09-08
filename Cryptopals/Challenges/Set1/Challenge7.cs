using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge7 : BaseChallenge
    {
        private const string FILE_NAME = "7.txt";
        private const string KEY = "YELLOW SUBMARINE";

        public Challenge7(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var fileUtils = new ImportFileUtilities(FILE_NAME);
            var data = fileUtils.ReadFileAsString();

            var aes = new AesDataContext(Convert.FromBase64String(data), StringUtilities.ConvertPlaintextToBytes(KEY));
            var result = aes.DecryptECB();

            return OutputResult(Answers.CHALLENGE_7, result);
        }
    }
}
