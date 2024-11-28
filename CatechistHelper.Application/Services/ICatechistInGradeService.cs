using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CatechistInGrade;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistInGradeService
    {
        Task<Result<bool>> AddCatechistToGrade(CreateCatechistInGradeRequest request);
        Task<Result<bool>> ArrangeCatechistGrade(ArrageCatechistGradeRequest request);
    }
}
