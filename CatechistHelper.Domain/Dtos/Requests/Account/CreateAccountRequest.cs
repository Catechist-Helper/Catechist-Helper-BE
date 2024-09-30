using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Account
{
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = MessageConstant.Account.Require.EmailRequired)]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email!")]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Account.Require.PasswordRequired)]
        [StringLength(50, ErrorMessage = "Tối thiểu {2} đến {1} kí tự", MinimumLength = 6)]
        public string Password { get; set; } = null!;

        public Guid RoleId { get; set; }
    }
}
