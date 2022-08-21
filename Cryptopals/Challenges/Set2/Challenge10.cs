using System.Text;
using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge10 : BaseChallenge
    {
        private const string FileName = "10.txt";
        private const string Key = "YELLOW SUBMARINE";

        public Challenge10(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var fileUtils = new ImportFileUtilities(FileName);

            var bytes = Convert.FromBase64String(fileUtils.ReadFileAsString());
            var keyBytes = StringUtilities.ConvertPlaintextToBytes(Key);
            var iv = Enumerable.Repeat((byte)0, Key.Length).ToArray();

            var aes = new AesDataContext(bytes, keyBytes);
            var cbcDecrypted = aes.DecryptCBC_Manual(iv);

            var result = Encoding.ASCII.GetString(cbcDecrypted);

            OutputResult(Answers.CHALLENGE_10, result);
        }
    }
}
