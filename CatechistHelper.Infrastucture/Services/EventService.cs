using CatechistHelper.Application.Extensions;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Event;
using CatechistHelper.Domain.Dtos.Responses.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Responses.Event;
using CatechistHelper.Domain.Dtos.Responses.Member;
using CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent;
using CatechistHelper.Domain.Dtos.Responses.Process;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Domain.Models;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class EventService : BaseService<EventService>, IEventService
    {
        public EventService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<EventService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Event> GetById(Guid id)
        {
            Event eventEntity = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                predicate: e => e.Id == id);
            Validator.EnsureNonNull(eventEntity);
            return eventEntity;
        }

        public async Task<Result<GetEventResponse>> Create(CreateEventRequest request)
        {
            try
            {
                Event newEvent = request.Adapt<Event>();
                Event result = await _unitOfWork.GetRepository<Event>().InsertAsync(newEvent);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    return Fail<GetEventResponse>(MessageConstant.Event.Fail.Create);
                }
                return Success(result.Adapt<GetEventResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null!;
            }


        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Event eventFromDb = await GetById(id);
                eventFromDb.IsDeleted = true;
                _unitOfWork.GetRepository<Event>().UpdateAsync(eventFromDb);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    return Fail<bool>(MessageConstant.Event.Fail.Delete);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetEventResponse>> Get(Guid id)
        {
            Event eventFromDb = await GetById(id);
            return Success(eventFromDb.Adapt<GetEventResponse>());
        }

        public async Task<PagingResult<GetEventResponse>> GetPagination(EventFilter? filter, int page, int size)
        {
            var events = await _unitOfWork.GetRepository<Event>().GetPagingListAsync(
                            predicate: BuildGetPaginationQuery(filter),
                            orderBy: e => e.OrderBy(e => e.EventStatus)
                                           .ThenByDescending(e => e.CreatedAt),
                            include: e => e.Include(x => x.EventCategory)
                                           .Include(x => x.Processes),
                            page: page,
                            size: size);

            // Transform the events into GetEventResponse while calculating fees
            var transformedResult = events.Items.Select(evt =>
            {
                var totalFee = evt.Processes.Sum(process => process.Fee);
                var totalActualFee = evt.Processes.Sum(process => process.ActualFee);

                var response = evt.Adapt<GetEventResponse>();
                response.TotalCost = totalFee;
                response.TotalActualCost = totalActualFee;
                response.SurplusCost = totalFee - totalActualFee;

                return response;
            }).ToList();

            // Convert the transformed result to IPaginate
            var paginatedResult = new Paginate<GetEventResponse>
            {
                Items = transformedResult,
                Page = page,
                Size= size,
                Total = events.Total
            };

            return SuccessWithPaging(
                    paginatedResult,
                    page,
                    size,
                    events.Total);
        }



        private Expression<Func<Event, bool>> BuildGetPaginationQuery(EventFilter? filter)
        {
            Expression<Func<Event, bool>> filterQuery = x => x.IsDeleted == false;
            if (filter != null && filter.EventCategoryId != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.EventCategoryId == filter.EventCategoryId);
            }
            return filterQuery;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateEventRequest request)
        {
            try
            {
                Event eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                    predicate: m => m.Id.Equals(id),
                    include: m => m.Include(i => i.Processes));

                if (eventFromDb.EventStatus == EventStatus.Completed)
                {
                    return Fail<bool>("Sự kiện đã hoàn thành. Không thể chỉnh sửa");
                }

                if (request.EventStatus == EventStatus.Completed)
                {
                    foreach (var process in eventFromDb.Processes)
                    {
                        if (process.Status != ProcessStatus.Completed)
                        {
                            process.Status = ProcessStatus.Completed;
                        }
                    }
                }

                request.Adapt(eventFromDb);
                _unitOfWork.GetRepository<Event>().UpdateAsync(eventFromDb);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                if (!isSuccessful)
                {
                    return Fail<bool>(MessageConstant.Event.Fail.Update);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<PagingResult<GetMemberResponse>> GetMembersByEventId(Guid id, int page, int size)
        {
            IPaginate<Member> accounts =
                   await _unitOfWork.GetRepository<Member>().GetPagingListAsync(
                            predicate: m => m.EventId == id,
                            include: m => m.Include(m => m.Account).Include(m => m.RoleEvent),
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    accounts.Adapt<IPaginate<GetMemberResponse>>(),
                    page,
                    size,
                    accounts.Total);
        }

        public async Task<PagingResult<GetBudgetTransactionResponse>> GetBudgetTransactionByEventId(Guid id, int page, int size)
        {
            IPaginate<BudgetTransaction> budgetTransaction =
                   await _unitOfWork.GetRepository<BudgetTransaction>().GetPagingListAsync(
                            predicate: bt => bt.EventId == id,
                            include: bt => bt.Include(bt => bt.TransactionImages),
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    budgetTransaction.Adapt<IPaginate<GetBudgetTransactionResponse>>(),
                    page,
                    size,
                    budgetTransaction.Total);
        }

        public async Task<PagingResult<GetProcessResponse>> GetProcessByEventId(Guid id, int page, int size)
        {
            IPaginate<Process> processes =
                   await _unitOfWork.GetRepository<Process>().GetPagingListAsync(
                            predicate: bt => bt.EventId == id,
                            include: process => process.Include(a => a.ReceiptImages),
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    processes.Adapt<IPaginate<GetProcessResponse>>(),
                    page,
                    size,
                    processes.Total);
        }

        public async Task<PagingResult<GetParicipantInEventResponse>> GetParticipantByEventId(Guid id, int page, int size)
        {
            IPaginate<ParticipantInEvent> processes =
                   await _unitOfWork.GetRepository<ParticipantInEvent>().GetPagingListAsync(
                            predicate: bt => bt.EventId == id,
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    processes.Adapt<IPaginate<GetParicipantInEventResponse>>(),
                    page,
                    size,
                    processes.Total);
        }

        public async Task<Result<bool>> AddParticipant(Guid id, IFormFile file)
        {
            if (file == null)
                return Fail<bool>("No file provided");

            try
            {
                var eventFromDb = await _unitOfWork.GetRepository<Event>()
                    .SingleOrDefaultAsync(
                        predicate: m => m.Id == id,
                        include: e => e.Include(x => x.ParticipantInEvents)
                    );

                return eventFromDb.EventStatus switch
                {
                    //EventStatus.Cancelled => Fail<bool>("Sự kiện đã hủy bỏ, không thể thêm người"),
                    EventStatus.Completed => Fail<bool>("Sự kiện đã kết thúc, không thể thêm người"),
                    _ => await ProcessParticipants(eventFromDb, file)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding participants to event {EventId}", id);
                return Fail<bool>(ex.Message);
            }
        }

        private async Task<Result<bool>> ProcessParticipants(Event eventFromDb, IFormFile file)
        {
            var participants = FileHelper.ReadFileReturnParticipants(file, eventFromDb.Id);

            var existingParticipants = new HashSet<(string Email, string FullName)>(
                eventFromDb.ParticipantInEvents
                    .Select(p => (p.Email.ToLower(), p.FullName.ToLower()))
            );

            var newParticipants = participants
                .Where(p => !existingParticipants.Contains((p.Email.ToLower(), p.FullName.ToLower())))
                .ToList();

            if (newParticipants.Count == 0)
                return Success(true);

            await _unitOfWork.GetRepository<ParticipantInEvent>().InsertRangeAsync(newParticipants);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return Success(isSuccessful);
        }

        public async Task<byte[]> ExportParticipants(Guid id)
        {
            var participants = await _unitOfWork.GetRepository<ParticipantInEvent>()
                .GetListAsync(predicate: p => p.EventId == id);
            return FileHelper.ExportParticipantsToExcel(participants);
        }
    }
}
