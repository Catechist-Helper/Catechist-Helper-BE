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
using CatechistHelper.Domain.Dtos.Responses.Timetable;

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
                Validator.CheckPageInput(page, size);
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
                return NotFounds<GetAccountResponse>(ex.Message);
            }
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

            if(account == null)
            {
                throw new Exception(MessageConstant.Login.Fail.InvalidAccount);
            }

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

        public async Task<PagingResult<GetRecruiterResponse>> GetFreeRecruiter(DateTime meetingTime, int page, int size)
        {
            try
            {
                int day = meetingTime.Day;
                IPaginate<Account> recruiters =
                    await _unitOfWork.GetRepository<Account>()
                    .GetPagingListAsync(
                            predicate: a => !a.IsDeleted,
                            include: a => a.Include(a => a.Interviews.Where(i => i.MeetingTime.Day == day))
                                           .Include(a => a.Role),
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        recruiters.Adapt<IPaginate<GetRecruiterResponse>>(),
                        page,
                        size,
                        recruiters.Total);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<IEnumerable<CalendarResponse>>> GetCalendar(Guid id)
        {
            try
            {
                var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id == id,
                    include: e => e.Include(e => e.Events));

                if (account == null)
                {
                    return NotFound<IEnumerable<CalendarResponse>>("Account not found.");
                }

                var slots = await _unitOfWork.GetRepository<CatechistInSlot>()
                    .GetListAsync(predicate: s => s.Catechist.AccountId == id,
                                  include: s => s.Include(s => s.Catechist)
                                                 .Include(s => s.Slot)
                                                 .ThenInclude(s => s.Class));

                var interviews = await _unitOfWork.GetRepository<RecruiterInInterview>()
                    .GetListAsync(predicate: i => i.AccountId == id,
                                  include: i => i.Include(i => i.Interview));

                var slotsTime = slots.Select(s => new CalendarResponse
                {
                    Title = s.Slot.Class.Name,
                    Start = HolidayService.ApplyDateToTimes(s.Slot.Date, s.Slot.StartTime),
                    End = HolidayService.ApplyDateToTimes(s.Slot.Date, s.Slot.EndTime),
                });

                var interviewsTime = interviews.Select(i => new CalendarResponse
                {
                    Title = "Interview Meeting",
                    Start = HolidayService.TimeToString(i.Interview.MeetingTime),
                    Description = i.OnlineRoomUrl
                });

                var eventsTime = account.Events.Select(e => new CalendarResponse
                {
                    Title = e.Name,
                    Start = HolidayService.TimeToString(e.StartTime),
                    End = HolidayService.TimeToString(e.EndTime),
                    Description = e.Description
                });

                var holidays = await HolidayService.GetAllHolidays();

                var holidaysTime = holidays.Items.Select(e => new CalendarResponse
                {
                    Title = e.Summary,
                    Description = e.Description,
                    Start = e.Start.Date,
                    End = e.End.Date
                });

                var calendarResponses = slotsTime.Concat(interviewsTime).Concat(eventsTime).Concat(holidaysTime);

                return Success(calendarResponses);
            }
            catch (Exception ex)
            {
                return BadRequest<IEnumerable<CalendarResponse>>(ex.Message);
            }
        }

        public async Task<Result<bool>> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: a => a.Email.Equals(request.Email));
                if (account == null)
                    return Fail<bool>("Email không tồn tại");

                if (!PasswordUtil.VerifyPassword(request.OldPassword, account.HashedPassword))
                    return Fail<bool>("Password không đúng");

                account.HashedPassword = PasswordUtil.HashPassword(request.NewPassword);
                _unitOfWork.GetRepository<Account>().UpdateAsync(account);

                if (await _unitOfWork.CommitAsync() <= 0)
                    return Fail<bool>("Cập nhật password thất bại");

                return Success(true);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

    }
}
