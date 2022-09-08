using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge10 : BaseChallenge
    {
        private const string FILE_NAME = "10.txt";
        private const string KEY = "YELLOW SUBMARINE";

        public Challenge10(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var fileUtils = new ImportFileUtilities(FILE_NAME);

            var bytes = Convert.FromBase64String(fileUtils.ReadFileAsString());
            var keyBytes = StringUtilities.ConvertPlaintextToBytes(KEY);
            var iv = Enumerable.Repeat((byte)0, KEY.Length).ToArray();

            var aes = new AesDataContext(bytes, keyBytes);
            var result = aes.DecryptCBCManual(iv);

            return OutputResult(Answers.CHALLENGE_10, result);
        }
    }
}
