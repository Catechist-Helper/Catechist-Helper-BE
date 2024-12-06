using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistInClassService
    {
        Task<Result<bool>> AddCatechistToClass(CreateCatechistInClassRequest request);
        Task<Result<bool>> ReplaceCatechistInClass(ReplaceCatechistInClassRequest request);
        Task<PagingResult<SearchCatechistResponse>> SearchAvailableCatechists(Guid id, Guid excludeId, int page, int size);
    }
}
