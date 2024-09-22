using CatechistHelper.Domain.Common;

namespace CatechistHelper.API.Configurations
{
    public static class ConfigureAppSettings
    {
        public static void SettingsBinding(this IConfiguration configuration)
        {

            AppConfig.ConnectionString = new ConnectionString();
            AppConfig.JwtSetting = new JwtSetting();

            configuration.Bind("ConnectionStrings", AppConfig.ConnectionString);
            configuration.Bind("JwtSettings", AppConfig.JwtSetting);
        }
    }
}
