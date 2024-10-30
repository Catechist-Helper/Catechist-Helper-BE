using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Grade;
using CatechistHelper.Domain.Dtos.Responses.CatechistInGrade;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Grade;
using CatechistHelper.Domain.Models;

namespace CatechistHelper.Application.Services
{
    public interface IGradeService
    {
        Task<PagingResult<GetGradeResponse>> GetPagination(GradeFilter filter, int page, int size);
        Task<PagingResult<GetClassResponse>> GetClassesByGradeId(Guid id, int page, int size);
        Task<PagingResult<GetCatechistInGradeResponse>> GetCatechistsByGradeId(Guid gradeId, int page, int size, bool excludeClassAssigned = false);
        Task<Result<GetGradeResponse>> Get(Guid id);
        Task<Result<GetGradeResponse>> Create(CreateGradeRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
