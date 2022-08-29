using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge13 : BaseChallenge
    {
        private const long DefaultUid = 10;
        private const string DefaultRole = "user";
        private const string AdminRole = "admin";

        private const string Email = "foo@email.com";

        public Challenge13(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var cookie = ProfileFor(Email);
            var (initialCookie, key) = EncryptCookie(cookie);
            var compromisedCookie = CompromiseCookie(initialCookie, key);

            var result = CookieUtilities.Encode(compromisedCookie);

            OutputResult(Answers.CHALLENGE_13, result);
        }

        #region Private Methods

        private static CookieDataContext CompromiseCookie(byte[] initialCookie, byte[] key)
        {
            var compromisedPayload = Email.Replace("@email.com", AdminRole) + "\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b";
            var adminCookie = ProfileFor(compromisedPayload);
            var (adminEncrypted, adminKey) = EncryptCookie(adminCookie, key);

            var compromisedByes = new byte[initialCookie.Length - 16];

            for (int i = 0; i < 32; i++)
            {
                compromisedByes[i] = initialCookie[i];
            }

            for (int i = 16; i < 32; i++)
            {
                compromisedByes[i + 16] = adminEncrypted[i];
            }

            return DecryptCookie(compromisedByes, adminKey);
        }

        private static CookieDataContext ProfileFor(string email)
        {
            return new CookieDataContext()
            {
                Uid = DefaultUid,
                Email = email,
                Role = DefaultRole
            };
        }

        private static (byte[], byte[]) EncryptCookie(CookieDataContext cookie, byte[] key = null)
        {
            key ??= RandomUtilities.GetRandomBytes(16);

            var cookieString = CookieUtilities.Encode(cookie);
            var cookieBytes = StringUtilities.ConvertPlaintextToBytes(cookieString);
            cookieBytes = cookieBytes.PKCS7Padding(key.Length);

            var aes = new AesDataContext(cookieBytes, key);
            var result = aes.EncryptECB();

            return (result, key);
        }

        private static CookieDataContext DecryptCookie(byte[] encryptedCookie, byte[] key)
        {
            var aes = new AesDataContext(encryptedCookie, key);
            var result = aes.DecryptECBManual();
            return CookieUtilities.Parse<CookieDataContext>(result);
        }

        #endregion Private Methods
    }
}
