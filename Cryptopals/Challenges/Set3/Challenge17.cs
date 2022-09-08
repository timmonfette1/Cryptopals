using System.Text;
using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set3
{
    public class Challenge17 : BaseChallenge
    {
        private const string FILE_NAME = "17.txt";
        private const int BLOCK_SIZE = 16;

        private readonly byte[] _key;
        private readonly byte[] _iv;

        public Challenge17(int index) : base(index)
        {
            _key = RandomUtilities.GetRandomBytes(BLOCK_SIZE);
            _iv = RandomUtilities.GetRandomBytes(BLOCK_SIZE);
        }

        public override bool Execute()
        {
            var rand = RandomUtilities.GetRandomNumber(0, 10);
            var fileUtils = new ImportFileUtilities(FILE_NAME);
            var lines = fileUtils.ReadFile().Select(x => Encoding.ASCII.GetString(Convert.FromBase64String(x))).ToArray();
            var line = lines[rand];

            var bytes = StringUtilities.ConvertPlaintextToBytes(line);
            var encrypted = Encrypt(bytes);

            var encryptedAndIv = _iv.Concat(encrypted).ToArray();
            BruteForce(encryptedAndIv);

            var result = Encoding.ASCII.GetString(encryptedAndIv.Skip(BLOCK_SIZE).ToArray().PKCS7Strip());

            return OutputResult(line, result);
        }

        #region Private Methods

        private byte[] Encrypt(byte[] bytes)
        {
            var aes = new AesDataContext(bytes, _key);
            return aes.EncryptCBC(_iv);
        }

        private byte[] Decrypt(byte[] bytes)
        {
            var aes = new AesDataContext(bytes, _key);
            return aes.DecryptCBCAsBytes(_iv);
        }

        private bool CBCPaddingOracle(byte[] input)
        {
            var decrypted = Decrypt(input);
            return decrypted.IsPKCS7Padded();
        }

        private static void XorBitByBit(byte[] input, byte[] output, int block, int bit)
        {
            for (var i = 15; i > bit; i--)
            {
                input[(block - 1) * BLOCK_SIZE + i] ^= (byte)(output[i] ^ (BLOCK_SIZE - bit));
            }
        }

        private static void XorBit(byte[] input, int value, int block, int bit)
        {
            input[(block - 1) * BLOCK_SIZE + bit] ^= (byte)(value ^ (BLOCK_SIZE - bit));
        }

        private void BruteForce(byte[] bytes)
        {
            for (var block = bytes.Length / BLOCK_SIZE - 1; block >= 1; block--)
            {
                var blockData = new byte[BLOCK_SIZE];
                for (var bit = 15; bit >= 0; bit--)
                {
                    XorBitByBit(bytes, blockData, block, bit);

                    for (var byteValue = 255; byteValue >= 0; byteValue--)
                    {
                        XorBit(bytes, byteValue, block, bit);

                        var toOracle = bytes.Take((block + 1) * BLOCK_SIZE).ToArray();
                        if (CBCPaddingOracle(toOracle))
                        {
                            XorBit(bytes, byteValue, block, bit);
                            blockData[bit] = (byte)byteValue;
                            break;
                        }

                        XorBit(bytes, byteValue, block, bit);
                    }

                    XorBitByBit(bytes, blockData, block, bit);
                }

                Array.Copy(blockData, 0, bytes, block * BLOCK_SIZE, BLOCK_SIZE);
            }
        }

        #endregion Private Methods
    }
}
