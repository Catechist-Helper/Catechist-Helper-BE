using CatechistHelper.Domain.Enums;
namespace CatechistHelper.Domain.Dtos.Requests.CatechistInTraining
{
    public class CreateCatechistInTrainingRequest
    {
        public Guid Id { get; set; }
        public CatechistInTrainingStatus Status { get; set; }
    }
}
