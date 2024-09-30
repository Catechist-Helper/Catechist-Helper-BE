using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Account
{
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = MessageConstant.Account.Require.PasswordRequired)]
        [StringLength(50, ErrorMessage = "Tối thiểu {2} đến {1} kí tự", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
