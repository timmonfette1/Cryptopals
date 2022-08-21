using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.DataContexts
{
    public class CryptographyDataContext
    {
        private readonly byte[] _bytes;
        private readonly string _bytesHex;

        private byte[] _key;
        private string _keyHex;

        private readonly IDictionary<string, string> _bruteForcedXor;

        private CryptographyDataContext()
        {
            _bruteForcedXor = new Dictionary<string, string>();
        }

        public CryptographyDataContext(string hex) : this()
        {
            _bytes = StringUtilities.ConvertHexToBytes(hex);
            _bytesHex = hex;
        }

        public CryptographyDataContext(string hex, string key) : this()
        {
            _bytes = StringUtilities.ConvertHexToBytes(hex);
            _bytesHex = hex;
            _key = StringUtilities.ConvertHexToBytes(key);
            _keyHex = key;
        }

        public CryptographyDataContext(byte[] bytes) : this()
        {
            _bytes = bytes;
            _bytesHex = _bytes.ToHexString();
        }

        public CryptographyDataContext(byte[] bytes, byte[] key) : this()
        {
            _bytes = bytes;
            _bytesHex = _bytes.ToHexString();
            _key = key;
            _keyHex = _key.ToHexString();
        }

        public byte[] Bytes => _bytes;
        public string Hex => _bytesHex;
        public byte[] Key => _key;
        public string KeyHex => _keyHex;
        public IDictionary<string, string> BruteForcedXor => _bruteForcedXor;

        public void ExpandKey() => ExpandKey(_keyHex);
        private void ExpandKey(string key)
        {
            var expandedKey = string.Empty;
            while (expandedKey.Length < _bytesHex.Length)
            {
                expandedKey += key;
            }

            _key = StringUtilities.ConvertHexToBytes(expandedKey);
            _keyHex = expandedKey;
        }

        public byte[] Xor() => Xor(_key);
        public byte[] Xor(CryptographyDataContext value) => Xor(value.Bytes);
        public byte[] Xor(byte[] value)
        {
            var result = new byte[_bytes.Length];
            for (int i = 0; i < _bytes.Length; i++)
            {
                result[i] = (byte)(_bytes[i] ^ value[i]);
            }

            return result;
        }

        public void GenerateBruteForceXor()
        {
            if (_bruteForcedXor.Count != 0)
            {
                throw new Exception($"{nameof(GenerateBruteForceXor)} has already been execute. Create a new {nameof(CryptographyDataContext)} to execute a different or new Brute Forced Xor.");
            }

            foreach (var key in Enumerable.Range(0, 127))
            {
                var keyAsHex = Convert.ToString(key, 16);
                ExpandKey(keyAsHex);

                var bytes = Xor();
                var hex = bytes.ToHexString();
                _bruteForcedXor.Add(keyAsHex, hex);
            }
        }

        public int HammingDistance(int keySize, int index)
        {
            var chunk1 = _bytes.Skip(keySize * (index - 1)).Take(keySize).ToArray();
            var chunk2 = _bytes.Skip(keySize * index).Take(keySize).ToArray();

            var distance = 0;

            for (int i = 0; i < chunk1.Length; i++)
            {
                var xor = chunk1[i] ^ chunk2[i];
                while (xor > 0)
                {
                    if ((xor & 1) == 1)
                    {
                        distance++;
                    }

                    xor >>= 1;
                }
            }

            return distance;
        }
    }
}
