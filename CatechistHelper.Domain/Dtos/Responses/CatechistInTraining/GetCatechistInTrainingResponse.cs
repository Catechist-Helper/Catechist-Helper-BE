using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.CatechistInTraining
{
    public class GetCatechistInTrainingResponse
    {
        public required GetCatechistResponse Catechist { get; set; }
        public CatechistInTrainingStatus CatechistInTrainingStatus { get; set; }

    }
}
