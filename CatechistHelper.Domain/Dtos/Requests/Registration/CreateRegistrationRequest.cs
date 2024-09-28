using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Registration
{
    public class CreateRegistrationRequest
    {
        [Required(ErrorMessage = MessageConstant.Registration.Require.FullNameRequired)]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.GenderRequired)]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.DateOfBirthRequired)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = MessageConstant.Registration.Require.AddresseRequired)]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.EmailRequired)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.PhoneRequired)]
        [Phone]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Registration.Require.IsTeachingBeforeeRequired)]
        public bool IsTeachingBefore { get; set; }

        [Required(ErrorMessage = MessageConstant.Registration.Require.YearOfTeachingRequired)]
        public int YearOfTeaching { get; set; }

        public string? Note { get; set; }
        public List<string>? Images { get; set; }
    }
}
