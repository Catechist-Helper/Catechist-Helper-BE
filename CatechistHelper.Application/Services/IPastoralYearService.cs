using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos;
using CatechistHelper.Domain.Dtos.Requests.PastoralYear;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IPastoralYearService
    {
        Task<PagingResult<GetPastoralYearResponse>> GetPagination(Expression<Func<PastoralYear, bool>>? predicate, int page, int size);

        Task<Result<GetPastoralYearResponse>> Get(Guid id);
        Task<Result<GetPastoralYearResponse>> Create(CreatePastoralYearRequest request);
        Task<Result<bool>> Update(Guid id, UpdatePastoralYearRequest request);
        Task<Result<bool>> Delete(Guid id);

        Task<PastoralYear> Create(PastoralYearDto dto);
    }
}
