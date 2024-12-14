using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.CatechistInTraining
{
    public class GetTrainingInfomationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<TrainingOfCatechist>? TrainingInformation { get; set; }
    }

    public class TrainingOfCatechist
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PreviousLevel { get; set; } = string.Empty;
        public string NextLevel { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TrainingListStatus TrainingListStatus { get; set; }
        public CatechistInTrainingStatus CatechistInTrainingStatus { get; set; }

    }
}