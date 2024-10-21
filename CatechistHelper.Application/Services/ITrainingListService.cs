using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.TrainingList;
using CatechistHelper.Domain.Dtos.Responses.TrainingList;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface ITrainingListService
    {
        Task<PagingResult<GetTrainingListResponse>> GetPagination(Expression<Func<TrainingList, bool>>? predicate, int page, int size);
        Task<Result<GetTrainingListResponse>> Get(Guid id);
        Task<Result<GetTrainingListResponse>> Create(CreateTrainingListRequest request);
        Task<Result<bool>> Update(Guid id, UpdateTrainingListRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
