using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge16 : BaseChallenge
    {
        private const string PREPEND = "comment1=cooking%20MCs;userdata=";
        private const string APPEND = ";comment2=%20like%20a%20pound%20of%20bacon";
        private const int BLOCK_LENGTH = 16;

        private const string INPUT = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private readonly byte[] _key;

        public Challenge16(int index) : base(index)
        {
            _key = RandomUtilities.GetRandomBytes(BLOCK_LENGTH);
        }

        public override bool Execute()
        {
            var (encrypted, iv) = Encrypt(INPUT);

            var firstBlock = StringUtilities.ConvertPlaintextToBytes(PREPEND[..BLOCK_LENGTH]);
            var crypto = new CryptographyDataContext(firstBlock);
            var bitFlipped = crypto.Xor(iv);

            var payload = StringUtilities.ConvertPlaintextToBytes(";admin=true;xxxx");
            var cryptoPayload = new CryptographyDataContext(payload);
            var bitFlippedPayload = cryptoPayload.Xor(bitFlipped);

            var decrypted = Decrypt(encrypted, bitFlippedPayload);
            var index = decrypted.IndexOf(Answers.CHALLENGE_16, StringComparison.OrdinalIgnoreCase);

            var result = string.Empty;
            if (index >= 0)
            {
                result = decrypted.Substring(index, Answers.CHALLENGE_16.Length);
            }

            return OutputResult(Answers.CHALLENGE_16, result);
        }

        #region Private Methods

        private (byte[], byte[]) Encrypt(string input)
        {
            var sanitized = input.Replace(";", "_").Replace("=", "_");
            var bytes = StringUtilities.ConvertPlaintextToBytes(PREPEND + sanitized + APPEND).PKCS7Padding(BLOCK_LENGTH);
            var iv = RandomUtilities.GetRandomBytes(BLOCK_LENGTH);
            var aes = new AesDataContext(bytes, _key);
            var result = aes.EncryptCBC(iv);

            return (result, iv);
        }

        private string Decrypt(byte[] bytes, byte[] iv)
        {
            var aes = new AesDataContext(bytes, _key);
            return aes.DecryptCBCManual(iv);
        }

        #endregion Private Methods
    }
}
