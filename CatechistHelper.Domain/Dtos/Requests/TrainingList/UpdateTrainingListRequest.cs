using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.TrainingList
{
    public class UpdateTrainingListRequest : CreateTrainingListRequest
    {
        public TrainingListStatus TrainingListStatus { get; set; }
    }
}
