namespace m_verify_BE.Configuration
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = "m-verify";
        public string Audience { get; set; } = "m-verify";
        public int ExpiryHours { get; set; } = 8;
    }

    public class AdminSeedOptions
    {
        public const string SectionName = "AdminSeed";
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
