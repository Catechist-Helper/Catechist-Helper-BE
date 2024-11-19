using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.TrainingList
{
    public class CreateTrainingListRequest
    {
        [Required(ErrorMessage = MessageConstant.TrainingList.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự!")]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PreviousLevelId { get; set; }
        public Guid NextLevelId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
