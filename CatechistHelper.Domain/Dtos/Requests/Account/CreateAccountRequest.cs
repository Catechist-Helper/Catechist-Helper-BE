using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Account
{
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = MessageConstant.Account.Require.EmailRequired)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = MessageConstant.Account.Require.PasswordRequired)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = MessageConstant.Account.Require.RoleRequired)]
        public Guid RoleId { get; set; }
    }
}
