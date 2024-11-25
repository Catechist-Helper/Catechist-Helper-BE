using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.TrainingList
{
    public class CreateTrainingListRequest : IValidatableObject
    {
        [Required(ErrorMessage = MessageConstant.TrainingList.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.TrainingList.Require.DescriptionRequired)]
        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Description { get; set; } = null!;

        public Guid CertificateId { get; set; }
        public Guid PreviousLevelId { get; set; }
        public Guid NextLevelId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

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
