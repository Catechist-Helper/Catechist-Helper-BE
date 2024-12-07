using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Event
{
    public class CreateEventRequest : IValidatableObject
    {
        [Required(ErrorMessage = MessageConstant.Event.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Event.Require.DescriptionRequired)]
        [MaxLength(1000, ErrorMessage = "Tối đa {1} kí tự.")]
        public string Description { get; set; } = null!;

        public bool IsPeriodic { get; set; }

        public bool IsCheckedIn { get; set; }

        [MaxLength(200, ErrorMessage = "Tối đa {1} kí tự.")]
        public string? Address { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public double current_budget { get; set; }

        public EventStatus EventStatus { get; set; }

        public Guid EventCategoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (StartTime > EndTime)
            {
                results.Add(new ValidationResult(MessageConstant.Common.InvalidStartEndTimeError, [nameof(StartTime), nameof(EndTime)]));
            }

            return results;
        }
    }
}
