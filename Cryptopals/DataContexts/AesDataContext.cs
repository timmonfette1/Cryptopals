using System.Security.Cryptography;
using System.Text;
using Cryptopals.Comparers;
using Cryptopals.Extensions;
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

        public byte[] EncryptECB()
        {
            var plainText = Encoding.ASCII.GetString(_bytes);

            using var alg = Aes.Create();
            alg.Mode = CipherMode.ECB;
            alg.Key = _key;

            using var encryptor = alg.CreateEncryptor(alg.Key, null);
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using var streamWriter = new StreamWriter(cryptoStream);
                streamWriter.Write(plainText);
            }

            return memoryStream.ToArray();
        }

        public string DecryptECB()
        {
            using var alg = Aes.Create();
            alg.Mode = CipherMode.ECB;
            alg.Key = _key;

            using var decryptor = alg.CreateDecryptor(alg.Key, null);
            using var memoryStream = new MemoryStream(_bytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public byte[] EncryptCBC(byte[] iv)
        {
            var plainText = Encoding.ASCII.GetString(_bytes);

            using var alg = Aes.Create();
            alg.Mode = CipherMode.CBC;
            alg.Key = _key;
            alg.IV = iv;

            using var encryptor = alg.CreateEncryptor(alg.Key, alg.IV);
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using var streamWriter = new StreamWriter(cryptoStream);
                streamWriter.Write(plainText);
            }

            return memoryStream.ToArray();
        }

        public string DecryptCBC(byte[] iv)
        {
            using var alg = Aes.Create();
            alg.Mode = CipherMode.CBC;
            alg.Key = _key;
            alg.IV = iv;

            using var decryptor = alg.CreateDecryptor(alg.Key, alg.IV);
            using var memoryStream = new MemoryStream(_bytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public string DecryptCBC_Manual(byte[] iv)
        {
            var result = new byte[_bytes.Length];

            using (var alg = Aes.Create())
            {
                alg.Mode = CipherMode.ECB;
                alg.Key = _key;
                alg.Padding = PaddingMode.None;

                using var dectyptor = alg.CreateDecryptor(alg.Key, null);

                var index = 0;

                while (index < _bytes.Length)
                {
                    index += dectyptor.TransformBlock(_bytes, index, alg.BlockSize / 8, result, index);

                    var chunk1 = result.Skip(index - alg.BlockSize / 8).Take(alg.BlockSize / 8).ToArray();
                    var chunk2 = index != alg.BlockSize / 8 ?
                        _bytes.Skip(index - alg.BlockSize / 8 * 2).Take(alg.BlockSize / 8).ToArray() :
                        iv;

                    var crypto = new CryptographyDataContext(chunk1);
                    var xor = crypto.Xor(chunk2);

                    xor.CopyTo(result, index - alg.BlockSize / 8);
                }
            }

            return Encoding.ASCII.GetString(result.PKCS7Strip());
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
