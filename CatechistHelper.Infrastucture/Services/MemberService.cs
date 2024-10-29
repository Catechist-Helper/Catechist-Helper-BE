using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Member;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    internal class MemberService : BaseService<MemberService>, IMemberService
    {
        public MemberService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<MemberService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> AddMemberToEvent(Guid eventId, List<CreateMemberRequest> request)
        {
            try
            {
                var eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                    predicate: e => e.Id == eventId,
                    include: e => e.Include(e => e.Members)
                ) ?? throw new Exception(MessageConstant.Event.Fail.NotFound);
                var requestedMembers = await _unitOfWork.GetRepository<Account>().GetListAsync(
                    predicate: c => request.Select(m => m.Id).Contains(c.Id));
                if (requestedMembers.Count != request.Count)
                {
                    throw new Exception(MessageConstant.Account.Fail.NotFoundAccount);
                }
                // Remove accounts not in the request
                var accountsToRemove = eventFromDb.Members
                    .Where(m => !request.Select(m => m.Id).Contains(m.AccountId))
                    .ToList();
                if (accountsToRemove.Any())
                {
                    _unitOfWork.GetRepository<Member>().DeleteRangeAsync(accountsToRemove);
                }
                // Lists for inserts and updates
                var accountsToInsert = new List<Member>();
                var accountsToUpdate = new List<Member>();
                foreach (var account in request)
                {
                    var accountFromDb = await _unitOfWork.GetRepository<Member>().SingleOrDefaultAsync(
                        predicate: m => m.AccountId == account.Id);
                    if (accountFromDb != null)
                    {
                        accountFromDb.RoleEventId = account.RoleEventId;
                        accountsToUpdate.Add(accountFromDb);
                    }
                    else
                    {
                        accountsToInsert.Add(new Member
                        {
                            EventId = eventId,
                            AccountId = account.Id,
                            RoleEventId = account.RoleEventId,
                        });
                    }
                }
                // Batch inserts and updates
                if (accountsToInsert.Any())
                {
                    await _unitOfWork.GetRepository<Member>().InsertRangeAsync(accountsToInsert);
                }
                if (accountsToUpdate.Any())
                {
                    _unitOfWork.GetRepository<Member>().UpdateRange(accountsToUpdate);
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
