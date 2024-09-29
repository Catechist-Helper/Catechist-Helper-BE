using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace CatechistHelper.Infrastructure.Utils
{
    public static class JwtUtil
    {

        public static string GenerateJwtToken(Account account)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secrectKey =
                new(Encoding.UTF8.GetBytes(AppConfig.JwtSetting.SecretKey));
            var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
            List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, account.Email),
            new Claim(ClaimTypes.Role, account.Role.RoleName),
        ];
            var expires = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken(AppConfig.JwtSetting.IssuerSigningKey, null, claims, notBefore: DateTime.Now, expires, credentials);
            return jwtHandler.WriteToken(token);
        }


    }
}