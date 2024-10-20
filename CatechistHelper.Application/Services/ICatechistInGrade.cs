using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CatechistInGrade;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistInGrade
    {
        Task<Result<bool>> AddCatechistToGrade(CreateCatechistInGradeRequest request);
    }
}
