using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Infrastructure.Utils;
using Microsoft.IdentityModel.Tokens;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Requests.Account;
using CatechistHelper.Domain.Dtos.Responses.Authentication;
using CatechistHelper.Domain.Dtos.Requests.Authentication;

namespace CatechistHelper.Infrastructure.Services
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        private const int bufferMinutes = 30;
        private readonly IFirebaseService _firebaseService;

        public AccountService(IFirebaseService firebaseService, IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _firebaseService = firebaseService;
        }

        public async Task<Result<GetAccountResponse>> Get(Guid id)
        {
            try
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: a => a.Include(a => a.Role));

                return Success(account.Adapt<GetAccountResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetAccountResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetAccountResponse>> GetPagination(Expression<Func<Account, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<Account> accounts =
                    await _unitOfWork.GetRepository<Account>()
                    .GetPagingListAsync(
                            predicate: a => a.IsDeleted == false,
                            include: a => a.Include(x => x.Role),
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        accounts.Adapt<IPaginate<GetAccountResponse>>(),
                        page,
                        size,
                        accounts.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<GetAccountResponse>> Create(CreateAccountRequest request)
        {
            try
            {
                if (!request.RoleName.IsNullOrEmpty())
                {
                    Role role = await _unitOfWork.GetRepository<Role>().SingleOrDefaultAsync(
                        predicate: r => r.RoleName.Equals(request.RoleName)) ?? throw new Exception(MessageConstant.Role.Fail.NotFoundRole);
                    request.RoleId = role.Id;
                } 
                else
                {
                    Role role = await _unitOfWork.GetRepository<Role>().SingleOrDefaultAsync(
                        predicate: r => r.Id.Equals(request.RoleId)) ?? throw new Exception(MessageConstant.Role.Fail.NotFoundRole);
                }

                Account accountFromDb = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Email.Equals(request.Email));

                if (accountFromDb != null)
                {
                    throw new Exception(MessageConstant.Account.Fail.EmailExisted);
                }

                Account account = request.Adapt<Account>();
                account.HashedPassword = PasswordUtil.HashPassword(request.Password);

                if (request.Avatar != null)
                {
                    string avatar = await _firebaseService.UploadImageAsync(request.Avatar, $"account/");
                    account.Avatar = avatar;
                }

                Account result = await _unitOfWork.GetRepository<Account>().InsertAsync(account);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.CreateAccount);
                }

                MailUtil.SendEmail(request.Email, ContentMailUtil.Title_AccountInformation,
                    ContentMailUtil.AnnounceAccountInformation(request.FullName, request.Email, request.Password), "");

                return Success(result.Adapt<GetAccountResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetAccountResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateAccountRequest request)
        {
            try
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                request.Adapt(account);

                if (account.Avatar != null)
                {
                    await _firebaseService.DeleteImageAsync(account.Avatar);
                    account.Avatar = null;
                }

                if (request.Avatar != null)
                {
                    string image = await _firebaseService.UploadImageAsync(request.Avatar, "account/");
                    account.Avatar = image;
                }

                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.CreateAccount);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));
                account.IsDeleted = true;
                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.DeleteAccount);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var account = await ValidateLoginRequest(request.Email, request.Password);

                if (account.IsDeleted)
                {
                    throw new Exception(MessageConstant.Login.Fail.InvalidAccount);
                }

                var response = new LoginResponse()
                {
                    Id = account.Id,
                    Email = account.Email,
                    Token = JwtUtil.GenerateJwtToken(account),
                    Role = account.Role.RoleName,
                };

                return Success(response);
            }
            catch (Exception ex)
            {
                return Fail<LoginResponse>(ex.Message);
            }
        }

        public async Task<Account> ValidateLoginRequest(string email, string password)
        {
            var account = await GetAccountByEmailAsync(email);

            ArgumentNullException.ThrowIfNull(account);

            if (!PasswordUtil.VerifyPassword(password, account.HashedPassword))
            {
                throw new Exception(MessageConstant.Login.Fail.PasswordIncorrect);
            }

            return account;
        }

        private async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: a => a.Email.Equals(email), include: query => query.Include(a => a.Role));
        }

        public async Task<PagingResult<GetAccountResponse>> GetFreeRecruiter(DateTime meetingTime, int page, int size)
        {
            try
            {
                DateTime bufferStart = meetingTime.AddMinutes(-bufferMinutes);
                DateTime bufferEnd = meetingTime.AddMinutes(bufferMinutes);

                // Get busy recruiters
                var busyRecruiters = await _unitOfWork.GetRepository<RecruiterInInterview>().GetListAsync(
                        predicate: ri => ri.Interview.MeetingTime >= bufferStart && ri.Interview.MeetingTime <= bufferEnd,
                        include: ri => ri.Include(ri => ri.Interview),
                        selector: ri => ri.Account.Id
                    );
                // Get free recruiters
                IPaginate<Account> recruiters =
                    await _unitOfWork.GetRepository<Account>()
                    .GetPagingListAsync(
                            predicate: a => !a.IsDeleted && !busyRecruiters.Contains(a.Id),
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        recruiters.Adapt<IPaginate<GetAccountResponse>>(),
                        page,
                        size,
                        recruiters.Total);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
