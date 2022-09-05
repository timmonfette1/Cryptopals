using Cryptopals.DataContexts;

namespace Cryptopals.Challenges.Set3
{
    public class Challenge23 : BaseChallenge
    {
        public Challenge23(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var mt = new MersenneTwisterDataContext(0);
            var originalState = mt.State;

            var clonedState = new uint[MersenneTwisterDataContext.N];
            for (int i = 0; i < MersenneTwisterDataContext.N; i++)
            {
                clonedState[i] = Untemper(mt.GetRandomValue());
            }

            OutputResult(originalState, clonedState);
        }

        protected override void OutputResult(object answer, object result, bool skipAnswerPrint = true)
        {
            var original = (uint[])answer;
            var cloned = (uint[])result;

            if (!skipAnswerPrint)
            {
                Console.WriteLine($"Answer: {original}");
                Console.WriteLine($"Result: {cloned}");
            }

            Console.WriteLine($"===== Challenge {_index} =====");
            Console.WriteLine($"Challenge Passed: {original.SequenceEqual(cloned)}");
        }

        #region Private Methods

        private static uint Untemper(uint original)
        {
            original ^= original >> MersenneTwisterDataContext.L;
            original ^= (original & 0x1DF8Cu) << MersenneTwisterDataContext.T;

            var inverse = original;
            inverse = ((inverse & 0x0000002D) << MersenneTwisterDataContext.S) ^ original;
            inverse = ((inverse & 0x000018AD) << MersenneTwisterDataContext.S) ^ original;
            inverse = ((inverse & 0x001A58AD) << MersenneTwisterDataContext.S) ^ original;
            original = ((inverse & 0x013A58AD) << MersenneTwisterDataContext.S) ^ original;

            var high = original & 0xFFE00000;
            var middle = original & 0x001FFC00;
            var low = original & 0x000003ff;

            return high |
                ((high >> MersenneTwisterDataContext.U) ^ middle) |
                ((((high >> MersenneTwisterDataContext.U) ^ middle) >> MersenneTwisterDataContext.U) ^ low);
        }

        #endregion Private Methods
    }
}
