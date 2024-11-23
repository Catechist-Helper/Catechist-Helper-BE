using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using CatechistHelper.Domain.Constants;

namespace CatechistHelper.Domain.Dtos.Requests.Process
{
    public class CreateProcessRequest : IValidatableObject
    {
        [Required(ErrorMessage = MessageConstant.Process.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Process.Require.DescriptionRequired)]
        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Description { get; set; } = null!;

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public double Fee { get; set; }

        public ProcessStatus Status { get; set; }

        public Guid EventId { get; set; }

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
