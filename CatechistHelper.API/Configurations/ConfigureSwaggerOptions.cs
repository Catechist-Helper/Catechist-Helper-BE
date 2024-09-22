using Microsoft.OpenApi.Models;

namespace CatechistHelper.API.Configurations
{
    public static class ConfigureSwaggerOptions
    {
        public static IServiceCollection AddSwaggerGenOption(this IServiceCollection service)
        {
            return service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CatechistHelper API",
                    Version = "v1",
                    Description = "CatechistHelper API",
                    Contact = new OpenApiContact
                    {
                        Name = "Contact Developers",
                        //Url = new Uri("")
                    }
                });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }
    }
}
