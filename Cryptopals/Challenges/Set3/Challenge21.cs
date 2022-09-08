using Cryptopals.DataContexts;

namespace Cryptopals.Challenges.Set3
{
    public class Challenge21 : BaseChallenge
    {
        public Challenge21(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var mt = new MersenneTwisterDataContext(0);
            var result = mt.GetRandomValue();
            return OutputResult(Answers.CHALLENGE_21, result);
        }
    }
}
