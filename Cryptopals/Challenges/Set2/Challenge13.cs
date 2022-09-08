using Cryptopals.DataContexts;
using Cryptopals.Extensions;
using Cryptopals.Utilities;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge13 : BaseChallenge
    {
        private const long DEFAULT_UID = 10;
        private const string DEFAULT_ROLE = "user";
        private const string ADMIN_ROLE = "admin";

        private const string EMAIL = "foo@email.com";

        public Challenge13(int index) : base(index)
        {

        }

        public override bool Execute()
        {
            var cookie = ProfileFor(EMAIL);
            var (initialCookie, key) = EncryptCookie(cookie);
            var compromisedCookie = CompromiseCookie(initialCookie, key);

            var result = CookieUtilities.Encode(compromisedCookie);

            return OutputResult(Answers.CHALLENGE_13, result);
        }

        #region Private Methods

        private static CookieDataContext CompromiseCookie(byte[] initialCookie, byte[] key)
        {
            var compromisedPayload = EMAIL.Replace("@email.com", ADMIN_ROLE) + "\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b\x0b";
            var adminCookie = ProfileFor(compromisedPayload);
            var (adminEncrypted, adminKey) = EncryptCookie(adminCookie, key);

            var compromisedByes = new byte[initialCookie.Length - key.Length];

            for (int i = 0; i < adminKey.Length * 2; i++)
            {
                compromisedByes[i] = initialCookie[i];
            }

            for (int i = adminKey.Length; i < adminKey.Length * 2; i++)
            {
                compromisedByes[i + adminKey.Length] = adminEncrypted[i];
            }

            return DecryptCookie(compromisedByes, adminKey);
        }

        private static CookieDataContext ProfileFor(string email)
        {
            return new CookieDataContext()
            {
                Uid = DEFAULT_UID,
                Email = email,
                Role = DEFAULT_ROLE
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
