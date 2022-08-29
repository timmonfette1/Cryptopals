namespace Cryptopals.DataContexts
{
    public class CookieDataContext : IEquatable<CookieDataContext>
    {
        public long Uid { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public override bool Equals(object obj) => Equals(obj as CookieDataContext);
        public bool Equals(CookieDataContext other)
        {
            return other != null &&
                Uid == other.Uid &&
                string.Equals(Email, other.Email, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Role, other.Role, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode() => HashCode.Combine(Uid, Email, Role);
    }
}
