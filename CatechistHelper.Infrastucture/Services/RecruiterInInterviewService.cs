using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.RecruiterInInterview;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class RecruiterInInterviewService : BaseService<RecruiterInInterviewService>, IRecruiterInInterviewService
    {
        public RecruiterInInterviewService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<RecruiterInInterviewService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> AddRecruiterInInterview(Guid interviewId, List<CreateRecruieterInInterviewRequest> request)
        {
            try
            {
                var interview = await _unitOfWork.GetRepository<Interview>().SingleOrDefaultAsync(
                    predicate: i => i.Id == interviewId,
                    include: i => i.Include(i => i.RecruiterInInterviews)
                ) ?? throw new Exception(MessageConstant.Interview.Fail.NotFoundInterview);
                var requestedAccounts = await _unitOfWork.GetRepository<Account>().GetListAsync(
                    predicate: a => request.Select(rii => rii.Id).Contains(a.Id));
                if (requestedAccounts.Count != request.Count)
                {
                    throw new Exception(MessageConstant.Account.Fail.NotFoundAccount);
                }
                // Remove accounts not in the request
                var accountsToRemove = interview.RecruiterInInterviews
                    .Where(rii => !request.Select(rii => rii.Id).Contains(rii.AccountId))
                    .ToList();
                if (accountsToRemove.Any())
                {
                    _unitOfWork.GetRepository<RecruiterInInterview>().DeleteRangeAsync(accountsToRemove);
                }
                // Lists for inserts and updates
                var accountsToInsert = new List<RecruiterInInterview>();
                var accountsToUpdate = new List<RecruiterInInterview>();
                foreach (var account in request)
                {
                    var accountFromDb = await _unitOfWork.GetRepository<RecruiterInInterview>().SingleOrDefaultAsync(
                        predicate: rii => rii.AccountId == account.Id);
                    if (accountFromDb != null)
                    {
                        accountsToUpdate.Add(accountFromDb);
                    }
                    else
                    {
                        accountsToInsert.Add(new RecruiterInInterview
                        {
                            InterviewId = interviewId,
                            AccountId = account.Id,
                        });
                    }
                }
                // Batch inserts and updates
                if (accountsToInsert.Any())
                {
                    await _unitOfWork.GetRepository<RecruiterInInterview>().InsertRangeAsync(accountsToInsert);
                }
                if (accountsToUpdate.Any())
                {
                    _unitOfWork.GetRepository<RecruiterInInterview>().UpdateRange(accountsToUpdate);
                }
                await _unitOfWork.CommitAsync();
                return Success(true);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }
    }
}