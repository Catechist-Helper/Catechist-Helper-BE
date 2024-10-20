using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CatechistInClass;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistInClassService
    {
        Task<Result<bool>> Create(CreateCatechistInClassRequest request);
        Task<Result<bool>> Update(Guid id, UpdateCatechistInClassRequest request);

    }
}
