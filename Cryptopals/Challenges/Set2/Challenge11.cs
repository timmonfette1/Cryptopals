using Cryptopals.DataContexts;
using Cryptopals.Enums;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge11 : BaseChallenge
    {
        private const string Input = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private readonly Random rng;
        private readonly IDictionary<EncryptionMethod, Func<byte[], byte[], byte[]>> EncryptionMapper;

        private EncryptionMethod Answer;

        public Challenge11(int index) : base(index)
        {
            rng = new Random();

            EncryptionMapper = new Dictionary<EncryptionMethod, Func<byte[], byte[], byte[]>>()
            {
                { EncryptionMethod.ECB, EncryptWithECB },
                { EncryptionMethod.CBC, EncryptWithCBC }
            };
        }

        public override void Execute()
        {
            var (encrypted, key) = RandomEncryption();
            var result = EncryptionOracle(encrypted, key);

            OutputResult(Answer.ToString(), result.ToString());
        }

        #region Private Methods

        private (byte[], byte[]) RandomEncryption()
        {
            var frontPaddingSize = rng.Next(5, 11);
            var frontPaddingBytes = new byte[frontPaddingSize];
            rng.NextBytes(frontPaddingBytes);

            var rearPaddingSize = rng.Next(5, 11);
            var rearPaddingBytes = new byte[rearPaddingSize];
            rng.NextBytes(rearPaddingBytes);

            var bytes = StringUtilities.ConvertPlaintextToBytes(Input);
            bytes = frontPaddingBytes.Concat(bytes).Concat(rearPaddingBytes).ToArray();

            var key = GetRandomBytes(16);

            var encryptionMethod = (EncryptionMethod)rng.Next(1, 3);
            Answer = encryptionMethod;

            var encryptionFunction = EncryptionMapper[encryptionMethod];

            var result = encryptionFunction(bytes, key);

            return (result, key);
        }

        private static EncryptionMethod EncryptionOracle(byte[] bytes, byte[] key)
        {
            var aes = new AesDataContext(bytes, key);
            var isECB = aes.IsUsingECB();

            return isECB ? EncryptionMethod.ECB : EncryptionMethod.CBC;
        }

        private byte[] EncryptWithECB(byte[] bytes, byte[] key)
        {
            var aes = new AesDataContext(bytes, key);
            return aes.EncryptECB();
        }

        private byte[] EncryptWithCBC(byte[] bytes, byte[] key)
        {
            var iv = GetRandomBytes(key.Length);
            var aes = new AesDataContext(bytes, key);
            return aes.EncryptCBC(iv);
        }

        private byte[] GetRandomBytes(int length)
        {
            var result = new byte[length];
            rng.NextBytes(result);
            return result;
        }

        #endregion Private Methods
    }
}
