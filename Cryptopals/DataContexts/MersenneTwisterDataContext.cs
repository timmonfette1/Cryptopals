namespace Cryptopals.DataContexts
{
    public class MersenneTwisterDataContext
    {
        #region MT Constants

        private const int MT_N = 624;
        private const int MT_M = 397;

        private const uint MT_A = 0x9908B0DF;
        private const uint MT_F = 1812433253;

        private const int MT_U = 11;

        private const int MT_S = 7;
        private const uint MT_B = 0x9D2C5680;

        private const int MT_T = 15;
        private const uint MT_C = 0xEFC60000;

        private const int MT_L = 18;

        private const uint MT_LOWER_MASK = 0x7FFFFFFF;
        private const uint MT_UPPER_MASK = 0x80000000;

        #endregion MT Constants

        #region Public Static Getters

        public static int N => MT_N;
        public static int U => MT_U;
        public static int S => MT_S;
        public static int T => MT_T;
        public static int L => MT_L;

        #endregion Public Static Getters

        private readonly uint _seed;
        private readonly uint[] _state;

        private int _index;

        public uint[] State => _state;

        public MersenneTwisterDataContext(uint seed)
        {
            _seed = seed;
            _state = new uint[N];
            _index = 0;

            Initialize();
        }

        public uint GetRandomValue()
        {
            if (_index >= N)
            {
                Twist();
            }

            var x = _state[_index++];
            x ^= x >> MT_U;
            x ^= (x << MT_S) & MT_B;
            x ^= (x << MT_T) & MT_C;
            x ^= x >> MT_L;

            return x;
        }

        #region Private Methods

        private void Initialize()
        {
            _state[_index] = _seed;

            for (int i = 1; i < N; i++)
            {
                _state[i] = (uint)((MT_F * (_state[i - 1] ^ (_state[i - 1] >> 30))) + i);
            }

            Twist();
        }

        private void Twist()
        {
            var loopIndex = 0;
            for (int i = 0; i < (N - MT_M); i++)
            {
                var firstHalfBits = (_state[i] & MT_UPPER_MASK) | (_state[i + 1] & MT_LOWER_MASK);
                _state[i] = _state[i + MT_M] ^ (firstHalfBits >> 1) ^ ((firstHalfBits & 1) * MT_A);
                loopIndex = i;
            }

            loopIndex++;

            for (var i = loopIndex; i < N - 1; i++)
            {
                var secondHalfBits = (_state[i] & MT_UPPER_MASK) | (_state[i + 1] & MT_LOWER_MASK);
                _state[i] = _state[i - (N - MT_M)] ^ (secondHalfBits >> 1) ^ ((secondHalfBits & 1) * MT_A);
                loopIndex = i;
            }

            loopIndex++;

            var bits = (_state[loopIndex] & MT_UPPER_MASK) | (_state[0] & MT_LOWER_MASK);
            _state[loopIndex] = _state[MT_M - 1] ^ (bits >> 1) ^ ((bits & 1) * MT_A);

            _index = 0;
        }

        #endregion Private Methods
    }
}
