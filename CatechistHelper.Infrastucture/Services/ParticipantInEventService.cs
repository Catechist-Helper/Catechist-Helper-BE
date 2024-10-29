using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.ParticipantInEvent;
using CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class ParticipantInEventService : BaseService<ParticipantInEventService>, IParticipantInEventService
    {
        public ParticipantInEventService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<ParticipantInEventService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetParicipantInEventResponse>> Create(CreateParticipantInEventRequest request)
        {
            Event eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                    predicate: e => e.Id.Equals(request.EventId)) ?? throw new Exception(MessageConstant.Event.Fail.NotFound);
            ParticipantInEvent participantInEvent = request.Adapt<ParticipantInEvent>();
            ParticipantInEvent result = await _unitOfWork.GetRepository<ParticipantInEvent>().InsertAsync(participantInEvent);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.ParticipantInEvent.Fail.Create);
            }
            return Success(result.Adapt<GetParicipantInEventResponse>());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                ParticipantInEvent participantInEvent = await _unitOfWork.GetRepository<ParticipantInEvent>().SingleOrDefaultAsync(
                    predicate: pie => pie.Id.Equals(id)) ?? throw new Exception(MessageConstant.ParticipantInEvent.Fail.NotFound);
                _unitOfWork.GetRepository<ParticipantInEvent>().DeleteAsync(participantInEvent);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.ParticipantInEvent.Fail.Delete);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetParicipantInEventResponse>> Get(Guid id)
        {
            ParticipantInEvent participantInEvent = await _unitOfWork.GetRepository<ParticipantInEvent>().SingleOrDefaultAsync(
                predicate: pie => pie.Id == id);
            return Success(participantInEvent.Adapt<GetParicipantInEventResponse>());
        }

        public async Task<PagingResult<GetParicipantInEventResponse>> GetPagination(int page, int size)
        {
            IPaginate<ParticipantInEvent> participantInEvents =
                   await _unitOfWork.GetRepository<ParticipantInEvent>().GetPagingListAsync(
                            page: page,
            size: size);
            return SuccessWithPaging(
                    participantInEvents.Adapt<IPaginate<GetParicipantInEventResponse>>(),
                    page,
                    size,
                    participantInEvents.Total);
        }

        public async Task<Result<bool>> Update(Guid id, UpdateParicipantInEventRequest request)
        {
            try
            {
                ParticipantInEvent participantInEvent = await _unitOfWork.GetRepository<ParticipantInEvent>().SingleOrDefaultAsync(
                    predicate: pie => pie.Id.Equals(id)) ?? throw new Exception(MessageConstant.ParticipantInEvent.Fail.NotFound);
                request.Adapt(participantInEvent);
                _unitOfWork.GetRepository<ParticipantInEvent>().UpdateAsync(participantInEvent);
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
    }
}
