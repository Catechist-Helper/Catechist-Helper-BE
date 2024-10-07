namespace CatechistHelper.Domain.Common
{
    public class AppConfig
    {
        public static ConnectionString ConnectionString { get; set; } = null!;
        public static JwtSetting JwtSetting { get; set; } = null!;
        public static MailSetting MailSetting { get; set; } = null!;
        public static GoogleImage GoogleImage { get; set; } = null!;
    }
    public class ConnectionString
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }
    public class JwtSetting
    {
        public string SecretKey { get; set; } = "Secret Key";
        public bool ValidateIssuerSigningKey { get; set; }
        public string? IssuerSigningKey { get; set; }
        public bool ValidateIssuer { get; set; } = true;
        public string? ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; } = true;
        public string? ValidAudience { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; } = true;
    }
    public class MailSetting
    {
        public string HostEmail { get; set; }
        public int PortEmail { get; set; }
        public string EmailSender { get; set; }
        public string PasswordSender { get; set; }
    }
    public class GoogleImage
    {
        public string Type { get; set; }
        public string ProjectId { get; set; }
        public string PrivateKeyId { get; set; }
        public string PrivateKey { get; set; }
        public string ClientEmail { get; set; }
        public string ClientId { get; set; }
    }
}
