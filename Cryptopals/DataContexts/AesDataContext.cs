using System.Security.Cryptography;
using Cryptopals.Comparers;
using Cryptopals.Utilities;

namespace Cryptopals.DataContexts
{
    public class AesDataContext
    {
        private readonly byte[] _bytes;
        private readonly byte[] _key;

        public AesDataContext(string hex)
        {
            _bytes = StringUtilities.ConvertHexToBytes(hex);
        }

        public AesDataContext(string hex, string key)
        {
            _bytes = StringUtilities.ConvertHexToBytes(hex);
            _key = StringUtilities.ConvertHexToBytes(key);
        }

        public AesDataContext(byte[] bytes)
        {
            _bytes = bytes;
        }

        public AesDataContext(byte[] bytes, byte[] key)
        {
            _bytes = bytes;
            _key = key;
        }

        public string Decrypt()
        {
            using var alg = Aes.Create();
            alg.Mode = CipherMode.ECB;
            alg.Key = _key;

            var decryptor = alg.CreateDecryptor(alg.Key, null);

            using var memoryStream = new MemoryStream(_bytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public bool IsUsingECB()
        {
            var hashSet = new HashSet<byte[]>(new ByteArrayComparer());

            for (int i = 0; i < _bytes.Length / 16; i++)
            {
                var chunk = _bytes.Skip(i * 16).Take(16).ToArray();
                if (hashSet.Contains(chunk))
                {
                    return true;
                }
                else
                {
                    hashSet.Add(chunk);
                }
            }

            return false;
        }
    }
}
