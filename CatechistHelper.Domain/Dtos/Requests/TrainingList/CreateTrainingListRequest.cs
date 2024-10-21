using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.TrainingList
{
    public class CreateTrainingListRequest
    {
        public string PreviousLevel { get; set; }
        public string NextLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TrainingListStatus TrainingListStatus { get; set; }
    }
}
