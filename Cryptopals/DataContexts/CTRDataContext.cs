namespace Cryptopals.DataContexts
{
    public class CTRDataContext
    {
        private readonly byte[] _bytes;
        private readonly byte[] _key;
        private readonly ulong _nonce;

        public CTRDataContext(byte[] bytes, byte[] key, ulong nonce)
        {
            _bytes = bytes;
            _key = key;
            _nonce = nonce;
        }

        public byte[] CryptBlocks()
        {
            var length = _bytes.Length;
            var result = new byte[length];

            for (ulong i = 0; (int)i < length; i += 16)
            {
                var blockLength = Math.Min(length - (int)i, 16);

                var toEncrypt = BitConverter.GetBytes(_nonce).Concat(BitConverter.GetBytes(i >> 4)).ToArray();

                var aes = new AesDataContext(toEncrypt, _key);
                var encrypted = aes.EncryptECB();

                var block = _bytes.Skip((int)i).Take(blockLength).ToArray();
                var crypto = new CryptographyDataContext(block);

                var xor = crypto.Xor(encrypted.Take(blockLength).ToArray());
                xor.CopyTo(result, (int)i);
            }

            return result;
        }
    }
}
