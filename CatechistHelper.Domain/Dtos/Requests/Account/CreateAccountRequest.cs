using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Account
{
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = MessageConstant.Account.Require.EmailRequired)]
        [EmailAddress(ErrorMessage = MessageConstant.Common.InvalidEmail)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Account.Require.PasswordRequired)]
        [StringLength(50, ErrorMessage = "Tối thiểu {2} đến {1} kí tự!", MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Account.Require.FullNameRequired)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string FullName { get; set; } = null!;

        [MaxLength(10, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.Require.PhoneRequired)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = MessageConstant.Common.InvalidPhoneNumber)]
        public string Phone { get; set; } = null!;

        public IFormFile? Avatar { get; set; }

        public Guid RoleId { get; set; }
    }
}
