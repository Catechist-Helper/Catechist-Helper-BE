using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Authentication
{
    public class LoginRequest
    {
        [EmailAddress(ErrorMessage = MessageConstant.Common.InvalidEmail)]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
