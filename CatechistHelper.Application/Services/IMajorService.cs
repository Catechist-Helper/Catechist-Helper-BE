using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Major;
using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IMajorService
    {
        Task<PagingResult<GetMajorResponse>> GetPagination(int page, int size);
        Task<Result<GetMajorResponse>> Get(Guid id);
        Task<PagingResult<GetLevelResponse>> GetLevelOfMajor(Guid id, int page, int size);
        Task<Result<GetMajorResponse>> Create(CreateMajorRequest request);
        Task<Result<bool>> CreateLevelOfMajor(Guid MajorId, Guid LevelId);
        Task<Result<bool>> Update(Guid id, UpdateMajorRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
