using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge16 : BaseChallenge
    {
        private const string Prepend = "comment1=cooking%20MCs;userdata=";
        private const string Append = ";comment2=%20like%20a%20pound%20of%20bacon";
        private const int BlockLength = 16;

        private const string Input = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        private readonly byte[] _key;

        public Challenge16(int index) : base(index)
        {
            _key = RandomUtilities.GetRandomBytes(BlockLength);
        }

        public override void Execute()
        {
            var (encrypted, iv) = Encrypt(Input);

            var firstBlock = StringUtilities.ConvertPlaintextToBytes(Prepend[..BlockLength]);
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

            OutputResult(Answers.CHALLENGE_16, result);
        }

        #region Private Methods

        private (byte[], byte[]) Encrypt(string input)
        {
            var sanitized = input.Replace(";", "_").Replace("=", "_");
            var bytes = StringUtilities.ConvertPlaintextToBytes(Prepend + sanitized + Append).PKCS7Padding(BlockLength);
            var iv = RandomUtilities.GetRandomBytes(BlockLength);
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
