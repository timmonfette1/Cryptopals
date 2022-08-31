using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.DataContexts
{
    public class EncryptionOracleDataContext
    {
        private const char CHAR = 'X';

        private readonly byte[] _unknown;
        private readonly byte[] _key;

        private readonly byte[] _randomPrefix;
        private readonly bool _useRandomPrefix;

        public EncryptionOracleDataContext(byte[] unknown, bool useRandomPrefix = false)
        {
            _unknown = unknown;
            _key = RandomUtilities.GetRandomBytes(16);
            _useRandomPrefix = useRandomPrefix;

            if (_useRandomPrefix)
            {
                var num = RandomUtilities.GetRandomNumber(16, 33);
                _randomPrefix = RandomUtilities.GetRandomBytes(num);
            }
            else
            {
                _randomPrefix = Array.Empty<byte>();
            }
        }

        public byte[] DecryptUnknown()
        {
            if (_useRandomPrefix)
            {
                throw new Exception($"This Oracle has been configured to use a random prefix. To decrypt the unknown string, use '{nameof(DecryptUnknownAdvanced)}' instead.");
            }

            var blockSize = CalculateBlockSize();
            TestForECB(blockSize);

            var decrypted = new List<byte>();
            for (var i = 0; i < _unknown.Length; i++)
            {
                var decryptedByte = DecryptByte(i, blockSize, decrypted);
                decrypted.Add(decryptedByte);
            }

            return decrypted.ToArray();
        }

        public byte[] DecryptUnknownAdvanced()
        {
            if (!_useRandomPrefix)
            {
                throw new Exception($"This Oracle has not been configured to use a random prefix. To decrypt the unknown string, use '{nameof(DecryptUnknown)}' instead.");
            }

            var blockSize = CalculateBlockSize();
            TestForECB(blockSize);

            var prefixSize = CalculatePrefixSize(blockSize);

            var decrypted = new List<byte>();
            for (var i = 0; i < _unknown.Length; i++)
            {
                var decryptedByte = DecryptByteAdvanced(i, blockSize, prefixSize, decrypted);
                decrypted.Add(decryptedByte);
            }

            return decrypted.ToArray();
        }

        #region Private Methods

        private byte[] EncryptData(string input) => EncryptData(StringUtilities.ConvertPlaintextToBytes(input));
        private byte[] EncryptData(byte[] inputBytes)
        {
            var bytes = _randomPrefix.Concat(inputBytes).Concat(_unknown).ToArray().PKCS7Padding(_key.Length);
            var aes = new AesDataContext(bytes, _key);
            return aes.EncryptECB();
        }

        private int CalculateBlockSize()
        {
            var initialEncrypted = EncryptData("X");
            var initialLength = initialEncrypted.Length;

            var blockLength = 0;

            for (var i = 2; i <= 256 && blockLength == 0; i++)
            {
                var input = new string(CHAR, i);
                var encrypted = EncryptData(input);

                if (encrypted.Length > initialLength)
                {
                    blockLength = encrypted.Length - initialLength;
                }
            }

            return blockLength;
        }

        private int CalculatePrefixSize(int blockSize)
        {
            var commonBlocks = 0;
            var newCommonBlocks = 0;

            var index = blockSize;
            var input = new string(CHAR, index);
            var encrypted = EncryptData(input);

            byte[] newEncrypted;
            while (index > 0 && newCommonBlocks >= commonBlocks)
            {
                index--;

                input = new string(CHAR, index);
                newEncrypted = EncryptData(input);
                newCommonBlocks = encrypted.SequenceCount(newEncrypted) / blockSize;

                if (commonBlocks == 0)
                {
                    commonBlocks = newCommonBlocks;
                }

                encrypted = newEncrypted;
            }

            var offset = index + 1;

            if (newCommonBlocks == commonBlocks)
            {
                offset = index;
            }

            return blockSize * commonBlocks - offset;
        }

        private void TestForECB(int blockSize)
        {
            var input = new string(CHAR, blockSize);
            var encrypted = EncryptData(string.Concat(Enumerable.Repeat(input, 5)));

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
            var input = new string(CHAR, inputSize);
            var encrypted = EncryptData(input);

            var lowerBound = targetSize * blockSize;
            var upperBound = (targetSize + 1) * blockSize;
            var targetBlock = encrypted[lowerBound..upperBound];

            var decryptedCount = decrypted.Count;
            var startingIndex = decryptedCount - blockSize < 0 ? 0 : decryptedCount - blockSize + 1;
            var count = blockSize > decryptedCount ? decryptedCount : blockSize - 1;
            var decryptedSlice = decrypted.GetRange(startingIndex, count);

            var remainder = blockSize - decryptedCount > 0 ? blockSize - decryptedCount - 1 : 0;
            var extraBytes = Enumerable.Repeat(Convert.ToByte(CHAR), remainder);

            var bruteForce = new byte[blockSize];
            extraBytes.Concat(decryptedSlice).ToArray().CopyTo(bruteForce, 0);

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

        private byte DecryptByteAdvanced(int index, int blockSize, int prefixSize, List<byte> decrypted)
        {
            var prefixInputSize = blockSize - (prefixSize % blockSize);
            var targetSize = (index + prefixSize + prefixInputSize) / blockSize;
            var blockOffsetSize = (prefixSize + prefixInputSize) / blockSize;
            var inputSize = prefixInputSize + blockSize - (index + 1 - blockSize * (targetSize - blockOffsetSize));

            var input = new string(CHAR, inputSize);
            var encrypted = EncryptData(input);

            var lowerBound = targetSize * blockSize;
            var upperBound = (targetSize + 1) * blockSize;
            var targetBlock = encrypted[lowerBound..upperBound];

            var decryptedCount = decrypted.Count;
            var startingIndex = decryptedCount - blockSize < 0 ? 0 : decryptedCount - blockSize + 1;
            var count = blockSize > decryptedCount ? decryptedCount : blockSize - 1;
            var decryptedSlice = decrypted.GetRange(startingIndex, count);

            var remainder = blockSize - decryptedCount > 0 ? blockSize - decryptedCount - 1 : 0;
            var extraBytes = Enumerable.Repeat(Convert.ToByte(CHAR), remainder);

            var prefixBytes = Enumerable.Repeat(Convert.ToByte(CHAR), prefixInputSize);

            var bruteForce = new byte[blockSize + prefixInputSize];
            prefixBytes.Concat(extraBytes).Concat(decryptedSlice).ToArray().CopyTo(bruteForce, 0);

            var blockStart = prefixSize + prefixInputSize;
            var blockEnd = blockStart + blockSize;
            var lastIndex = bruteForce.Length - 1;
            for (var i = 0; i < 255; i++)
            {
                var guessByte = Convert.ToByte(i);

                bruteForce[lastIndex] = guessByte;

                var guess = EncryptData(bruteForce);
                var guessBlock = guess[blockStart..blockEnd];

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
