using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Level;
using CatechistHelper.Domain.Dtos.Responses.Level;

namespace CatechistHelper.Application.Services
{
    public interface ILevelService
    {
        Task<Result<GetLevelResponse>> Get(Guid id);
        Task<Result<GetLevelResponse>> Create(CreateLevelRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
