﻿using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Event;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Responses.Event;
using CatechistHelper.Domain.Dtos.Responses.Member;
using CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent;
using CatechistHelper.Domain.Dtos.Responses.Process;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Models;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
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

        public async Task<Result<GetEventResponse>> Create(CreateEventRequest request)
        {
            Event newEvent = request.Adapt<Event>();
            Event result = await _unitOfWork.GetRepository<Event>().InsertAsync(newEvent);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Event.Fail.Create);
            }
            return Success(result.Adapt<GetEventResponse>());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Event eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                    predicate: g => g.Id.Equals(id)) ?? throw new Exception(MessageConstant.Event.Fail.NotFound);
                eventFromDb.IsDeleted = true;
                _unitOfWork.GetRepository<Event>().UpdateAsync(eventFromDb);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Event.Fail.Delete);
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
            Event eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                predicate: e => e.Id == id) ?? throw new Exception(MessageConstant.Event.Fail.NotFound);
            return Success(eventFromDb.Adapt<GetEventResponse>());
        }

        public async Task<PagingResult<GetEventResponse>> GetPagination(EventFilter? filter, int page, int size)
        {
            IPaginate<Event> events =
                   await _unitOfWork.GetRepository<Event>().GetPagingListAsync(
                            predicate: BuildGetPaginationQuery(filter),
                            orderBy: e => e.OrderBy(e => e.CreatedAt),
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    events.Adapt<IPaginate<GetEventResponse>>(),
                    page,
                    size,
                    events.Total);
        }

        private Expression<Func<Event, bool>> BuildGetPaginationQuery(EventFilter? filter)
        {
            Expression<Func<Event, bool>> filterQuery = x => x.IsDeleted == false;
/*            if (filter.MajorId != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Grade.Major.Id.Equals(filter.MajorId));
            }*/
            return filterQuery;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateEventRequest request)
        {
            try
            {
                Event eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                    predicate: m => m.Id.Equals(id)) ?? throw new Exception(MessageConstant.Event.Fail.NotFound);
                request.Adapt(eventFromDb);
                _unitOfWork.GetRepository<Event>().UpdateAsync(eventFromDb);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Event.Fail.Update);
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
                            include: m => m.Include(m => m.Account),
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
    }
}