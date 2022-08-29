using System.Text;
using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge12 : BaseChallenge
    {
        private const string Unknown = "Um9sbGluJyBpbiBteSA1LjAKV2l0aCBteSByYWctdG9wIGRvd24gc28gbXkgaGFpciBjYW4gYmxvdwpUaGUgZ2lybGllcyBvbiBzdGFuZGJ5IHdhdmluZyBqdXN0IHRvIHNheSBoaQpEaWQgeW91IHN0b3A/IE5vLCBJIGp1c3QgZHJvdmUgYnkK";

        private readonly byte[] key;

        public Challenge12(int index) : base(index)
        {
            key = RandomUtilities.GetRandomBytes(16);
        }

        public override void Execute()
        {
            var blockSize = CalculateBlockSize();
            TestForECB(blockSize);

            var decrypted = new List<byte>();
            for (var i = 0; i < Convert.FromBase64String(Unknown).Length; i++)
            {
                var decryptedByte = DecryptByte(i, blockSize, decrypted);
                decrypted.Add(decryptedByte);
            }

            var result = Encoding.ASCII.GetString(decrypted.ToArray());
            OutputResult(Answers.CHALLENGE_12, result);
        }

        #region Private Methods

        private byte[] EncryptData(string input) => EncryptData(StringUtilities.ConvertPlaintextToBytes(input));
        private byte[] EncryptData(byte[] inputBytes)
        {
            var unknownBytes = Convert.FromBase64String(Unknown);
            var bytes = inputBytes.Concat(unknownBytes).ToArray().PKCS7Padding(16);

            var aes = new AesDataContext(bytes, key);
            return aes.EncryptECB();
        }

        private int CalculateBlockSize()
        {
            var initialEncrypted = EncryptData("X");
            var initialLength = initialEncrypted.Length;

            var blockLength = 0;

            for (var i = 2; i < 129 && blockLength == 0; i++)
            {
                var input = new string('A', i);
                var encrypted = EncryptData(input);

                if (encrypted.Length > initialLength)
                {
                    blockLength = encrypted.Length - initialLength;
                }
            }

            return blockLength;
        }

        private void TestForECB(int blockSize)
        {
            var input = new string('X', blockSize);
            var encrypted = EncryptData(input + input);

            var aes = new AesDataContext(encrypted);

            if (!aes.IsUsingECB())
            {
                throw new Exception("The data wasn't encrypted using ECB mode");
            }
        }

        private byte DecryptByte(int index, int blockSize, List<byte> decrypted)
        {
            var targetSize = index / blockSize;
            var inputSize = blockSize - (index + 1 - (blockSize * targetSize));
            var input = new string('X', inputSize);
            var encrypted = EncryptData(input);

            var lowerBound = targetSize * blockSize;
            var upperBound = (targetSize + 1) * blockSize;
            var targetBlock = encrypted[lowerBound..upperBound];

            var bruteForce = new byte[blockSize];

            var decryptedCount = decrypted.Count;
            var startingIndex = decryptedCount - blockSize < 0 ? 0 : decryptedCount - blockSize + 1;
            var count = blockSize > decryptedCount ? decryptedCount : blockSize - 1;
            var decryptedSlice = decrypted.GetRange(startingIndex, count);

            var remainder = blockSize - decryptedCount > 0 ? blockSize - decryptedCount - 1 : 0;
            var initialBytes = Enumerable.Repeat(Convert.ToByte('X'), remainder);

            initialBytes.Concat(decryptedSlice).ToArray().CopyTo(bruteForce, 0);

            for (var i = 0; i < 255; i++)
            {
                var guessByte = Convert.ToByte(i);

                bruteForce[blockSize - 1] = guessByte;

                var guess = EncryptData(bruteForce);
                var guessBlock = guess.Take(blockSize).ToArray();

                if (guessBlock.SequenceEqual(targetBlock))
                {
                    return guessByte;
                }
            }

            throw new Exception("Unable to decrypt the byte.");
        }

        #endregion Private Methods
    }
}
