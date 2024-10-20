using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Models;

namespace CatechistHelper.Application.Services
{
    public interface IClassService
    {
        Task<PagingResult<GetClassResponse>> GetPagination(ClassFilter? filter, int page, int size);
    }
}
