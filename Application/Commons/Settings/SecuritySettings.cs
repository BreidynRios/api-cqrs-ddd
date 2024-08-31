namespace Application.Commons.Settings
{
    public class SecuritySettings
    {
        public ICollection<AuthorizedApiKey> AuthorizedApiKeys { get; set; }
        public JwtConfig JwtConfig { get; set; }
    }

    public class JwtConfig
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public string EncryptionKey { get; set; }
        public int ExpiryInMinutes { get; set; }
    }

    public class AuthorizedApiKey
    {
        public string ApiKey { get; set; }
        public string Application { get; set; }
    }
}
