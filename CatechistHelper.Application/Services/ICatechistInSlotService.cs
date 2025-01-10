using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CatechistInSlot;
using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistInSlotService
    {
        Task<PagingResult<SearchCatechistResponse>> FindAvailableCatechists(Guid id, int page, int size);
        Task<Result<bool>> ReplaceCatechist(Guid id, ReplaceCatechistRequest request);
        Task<PagingResult<SearchCatechistResponse>> SearchAvailableCatechists(Guid slotId, Guid excludeId, int page, int size);
    }
}
