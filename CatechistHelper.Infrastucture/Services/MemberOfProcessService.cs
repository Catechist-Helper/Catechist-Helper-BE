using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.MemberOfProcess;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class MemberOfProcessService : BaseService<MemberOfProcessService>, IMemberOfProcessService
    {
        public MemberOfProcessService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<MemberOfProcessService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> AddMemberToProcess(Guid processId, List<CreateMemberOfProcessRequest> request)
        {
            try
            {
                var process = await _unitOfWork.GetRepository<Process>().SingleOrDefaultAsync(
                    predicate: p => p.Id.Equals(processId),
                    include: p => p.Include(p => p.MemberOfProcesses)
                ) ?? throw new Exception(MessageConstant.Process.Fail.NotFound);
                var requestedAccounts = await _unitOfWork.GetRepository<Account>().GetListAsync(
                    predicate: a => request.Select(mop => mop.Id).Contains(a.Id));
                if (requestedAccounts.Count != request.Count)
                {
                    throw new Exception(MessageConstant.Account.Fail.NotFoundAccount);
                }
                // Remove catechists not in the request
                var accountsToRemove = process.MemberOfProcesses
                    .Where(mop => !request.Select(mop => mop.Id).Contains(mop.AccountId))
                    .ToList();
                if (accountsToRemove.Any())
                {
                    _unitOfWork.GetRepository<MemberOfProcess>().DeleteRangeAsync(accountsToRemove);
                }
                // Lists for inserts and updates
                var accountsToInsert = new List<MemberOfProcess>();
                var accountsToUpdate = new List<MemberOfProcess>();
                foreach (var account in request)
                {
                    var accountFromDb = await _unitOfWork.GetRepository<MemberOfProcess>().SingleOrDefaultAsync(
                        predicate: mop => mop.ProcessId == mop.ProcessId && mop.AccountId == account.Id);
                    if (accountFromDb != null)
                    {
                        accountFromDb.IsMain = account.IsMain;
                        accountsToUpdate.Add(accountFromDb);
                    }
                    else
                    {
                        accountsToInsert.Add(new MemberOfProcess
                        {
                            ProcessId = processId,
                            AccountId = account.Id,
                            IsMain = account.IsMain,
                        });
                    }
                }
                // Batch inserts and updates
                if (accountsToInsert.Any())
                {
                    await _unitOfWork.GetRepository<MemberOfProcess>().InsertRangeAsync(accountsToInsert);
                }
                if (accountsToUpdate.Any())
                {
                    _unitOfWork.GetRepository<MemberOfProcess>().UpdateRange(accountsToUpdate);
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
