using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Grade;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Grade;

namespace CatechistHelper.Application.Services
{
    public interface IGradeService
    {
        Task<PagingResult<GetGradeResponse>> GetPagination(int page, int size);
        Task<PagingResult<GetCatechistResponse>> GetCatechistsByGradeId(Guid id, int page, int size);
        Task<PagingResult<GetClassResponse>> GetClassesByGradeId(Guid id, int page, int size);
        Task<Result<GetGradeResponse>> Get(Guid id);
        Task<Result<GetGradeResponse>> Create(CreateGradeRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
