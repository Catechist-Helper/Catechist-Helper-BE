using CatechistHelper.Domain.Common;

namespace CatechistHelper.API.Configurations
{
    public static class ConfigureAppSettings
    {
        public static void SettingsBinding(this IConfiguration configuration)
        {
            AppConfig.ConnectionString = new ConnectionString();
            AppConfig.JwtSetting = new JwtSetting();
            AppConfig.MailSetting = new MailSetting();

            configuration.Bind("ConnectionStrings", AppConfig.ConnectionString);
            configuration.Bind("JwtSettings", AppConfig.JwtSetting);
            configuration.Bind("MailSettings", AppConfig.MailSetting);
        }
    }
}
