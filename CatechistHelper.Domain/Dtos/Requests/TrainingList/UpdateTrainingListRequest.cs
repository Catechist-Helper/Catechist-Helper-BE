using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.TrainingList
{
    public class UpdateTrainingListRequest : CreateTrainingListRequest
    {
        public bool IsDeleted { get; set; }
        public TrainingListStatus TrainingListStatus { get; set; }
    }
}
