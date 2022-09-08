using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge8 : BaseChallenge
    {
        private const string FILE_NAME = "8.txt";

        public Challenge8(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var fileUtils = new ImportFileUtilities(FILE_NAME);
            var data = fileUtils.ReadFile();

            var result = string.Empty;
            foreach (var line in data)
            {
                var aes = new AesDataContext(line);
                if (aes.IsUsingECB())
                {
                    result = line;
                    break;
                }
            }

            return OutputResult(Answers.CHALLENGE_8, result);
        }
    }
}
