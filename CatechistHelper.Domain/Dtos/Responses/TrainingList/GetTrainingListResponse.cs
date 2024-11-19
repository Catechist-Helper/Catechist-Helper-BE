using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.TrainingList
{
    public class GetTrainingListResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public required GetLevelResponse PreviousLevel { get; set; }
        public required GetLevelResponse NextLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TrainingListStatus TrainingListStatus { get; set; }
        public bool IsDeleted { get; set; }

    }
}
