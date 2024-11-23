using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Registration
{
    public class CreateRegistrationRequest
    {
        [Required(ErrorMessage = MessageConstant.Registration.Require.FullNameRequired)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.GenderRequired)]
        [MaxLength(10, ErrorMessage = "Tối đa {1} kí tự")]
        public string Gender { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = MessageConstant.Registration.Require.AddresseRequired)]
        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.EmailRequired)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự")]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.PhoneRequired)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "không đúng định dạng số điện thoại")]
        public string Phone { get; set; } = null!;

        public bool IsTeachingBefore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public int YearOfTeaching { get; set; } = 0;

        [MaxLength(1000, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Note { get; set; }
        public List<IFormFile>? CertificateOfCandidates { get; set; }
    }
}
