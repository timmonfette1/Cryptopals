using Cryptopals.DataContexts;
using Cryptopals.Enums;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge11 : BaseChallenge
    {
        private const string Input = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private readonly IDictionary<EncryptionMethod, Func<byte[], byte[], byte[]>> EncryptionMapper;

        private EncryptionMethod Answer;

        public Challenge11(int index) : base(index)
        {
            EncryptionMapper = new Dictionary<EncryptionMethod, Func<byte[], byte[], byte[]>>()
            {
                { EncryptionMethod.ECB, EncryptWithECB },
                { EncryptionMethod.CBC, EncryptWithCBC }
            };
        }

        public override void Execute()
        {
            var encrypted = RandomEncryption();
            var result = EncryptionOracle(encrypted);

            OutputResult(Answer.ToString(), result.ToString());
        }

        #region Private Methods

        private byte[] RandomEncryption()
        {
            var frontPaddingSize = RandomUtilities.GetRandomNumber(5, 11);
            var frontPaddingBytes = RandomUtilities.GetRandomBytes(frontPaddingSize);

            var rearPaddingSize = RandomUtilities.GetRandomNumber(5, 11);
            var rearPaddingBytes = RandomUtilities.GetRandomBytes(rearPaddingSize);

            var bytes = StringUtilities.ConvertPlaintextToBytes(Input);
            bytes = frontPaddingBytes.Concat(bytes).Concat(rearPaddingBytes).ToArray().PKCS7Padding(16);

            var key = RandomUtilities.GetRandomBytes(16);

            var encryptionMethod = (EncryptionMethod)RandomUtilities.GetRandomNumber(1, 3);
            Answer = encryptionMethod;

            var encryptionFunction = EncryptionMapper[encryptionMethod];
            var result = encryptionFunction(bytes, key);

            return result;
        }

        private static EncryptionMethod EncryptionOracle(byte[] bytes)
        {
            var aes = new AesDataContext(bytes);
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
            var iv = RandomUtilities.GetRandomBytes(key.Length);
            var aes = new AesDataContext(bytes, key);
            return aes.EncryptCBC(iv);
        }

        #endregion Private Methods
    }
}
