namespace CatechistHelper.Domain.Dtos.Requests.TrainingList
{
    public class CreateTrainingListRequest
    {
        public Guid PreviousLevelId { get; set; }
        public Guid NextLevelId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
