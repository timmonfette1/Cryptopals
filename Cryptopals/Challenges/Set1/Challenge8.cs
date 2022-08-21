using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set1
{
    public class Challenge8 : BaseChallenge
    {
        public Challenge8(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var fileUtils = new ImportFileUtilities("8.txt");
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

            OutputResult(Answers.CHALLENGE_8, result);
        }
    }
}
