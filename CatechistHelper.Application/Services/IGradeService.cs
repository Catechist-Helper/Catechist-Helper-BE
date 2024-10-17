using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Grade;
using CatechistHelper.Domain.Dtos.Responses.Grade;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IGradeService
    {
        Task<PagingResult<GetGradeResponse>> GetPagination(int page, int size);
        Task<Result<GetGradeResponse>> Get(Guid id);
        Task<Result<GetGradeResponse>> Create(CreateGradeRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
