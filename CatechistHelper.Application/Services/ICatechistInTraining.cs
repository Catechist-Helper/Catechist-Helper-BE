using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CatechistInTraining;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistInTraining
    {
        Task<Result<bool>> AddCatechistToTrainingList(Guid trainingListId, List<CreateCatechistInTrainingRequest> request);
    }
}
