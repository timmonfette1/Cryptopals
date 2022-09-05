using Cryptopals.DataContexts;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set3
{
    public class Challenge22 : BaseChallenge
    {
        public Challenge22(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var initialSeed = (uint)DateTimeOffset.Now.ToUnixTimeSeconds();
            var mt = new MersenneTwisterDataContext(initialSeed);
            var sleep = initialSeed + (uint)RandomUtilities.GetRandomNumber(40, 1001);
            var random = mt.GetRandomValue();

            var seed = sleep;
            while (true)
            {
                mt = new MersenneTwisterDataContext(seed);
                if (mt.GetRandomValue() == random)
                {
                    break;
                }

                seed--;
            }

            OutputResult(initialSeed, seed);
        }
    }
}
