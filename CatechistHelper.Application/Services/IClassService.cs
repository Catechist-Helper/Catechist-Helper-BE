using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IClassService
    {
        Task<PagingResult<GetClassResponse>> GetPagination(Expression<Func<Class, bool>>? predicate, int page, int size);
    }
}
