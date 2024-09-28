using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Account
{
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = MessageConstant.Account.Require.PasswordRequired)]
        public string Password { get; set; } = string.Empty;
    }
}
