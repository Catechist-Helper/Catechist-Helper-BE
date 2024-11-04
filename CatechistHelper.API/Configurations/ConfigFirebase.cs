using System.Text.Json;
using CatechistHelper.Domain.Common;

namespace CatechistHelper.API.Configurations;

public static class ConfigFirebase
{
    public static void ConfigureFirebase()
    {
        var googleJson = JsonSerializer.Serialize(AppConfig.GoogleImage, new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(), 
            WriteIndented = true 
        });

        var temp = Path.GetTempFileName();
        File.WriteAllText(temp, googleJson);
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", temp);
    }

    private class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return string.Concat(
                name.Select((x, i) => i > 0 && char.IsUpper(x) 
                    ? "_" + x.ToString().ToLower() 
                    : x.ToString().ToLower()));
        }
    }

}