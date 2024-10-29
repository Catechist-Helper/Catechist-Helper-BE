using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.ParticipantInEvent
{
    public class CreateParticipantInEventRequest
    {
        [Required(ErrorMessage = MessageConstant.ParticipantInEvent.Require.FullNameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.ParticipantInEvent.Require.EmailRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.ParticipantInEvent.Require.PhoneRequired)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = MessageConstant.Common.InvalidPhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.ParticipantInEvent.Require.GenderRequired)]
        [StringLength(10)]
        public string Gender { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = MessageConstant.ParticipantInEvent.Require.AddressRequired)]
        [MaxLength(100, ErrorMessage = "Vượt quá {1} kí tự")]
        public string Address { get; set; } = null!;

        public Guid EventId { get; set; }
    }
}
