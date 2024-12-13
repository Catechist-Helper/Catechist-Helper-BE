using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;
using CatechistHelper.Domain.Dtos.Requests.Account;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.Authentication;
using CatechistHelper.Domain.Dtos.Requests.Authentication;
using CatechistHelper.Domain.Dtos.Responses.Timetable;

namespace CatechistHelper.Application.Services
{
    public interface IAccountService
    {
        Task<PagingResult<GetAccountResponse>> GetPagination(Expression<Func<Account, bool>>? predicate, int page, int size);
        Task<PagingResult<GetRecruiterResponse>> GetFreeRecruiter(DateTime meetingTime, int page, int size);
        Task<Result<GetAccountResponse>> Get(Guid id);
        Task<Result<GetAccountResponse>> Create(CreateAccountRequest request);
        Task<Result<bool>> Update(Guid id, UpdateAccountRequest request);
        Task<Result<bool>> Delete(Guid id);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
        Task<Result<IEnumerable<CalendarResponse>>> GetCalendar(Guid id);
        Task<Result<bool>> ChangePassword(ChangePasswordRequest request);
    }
}
