using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Responses.AbsenceRequest;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class AbsenceRequestService: BaseService<AbsenceRequestService>, IAbsenceRequestService
    {
        private readonly ISystemConfigurationService _systemConfigurationService;

        public AbsenceRequestService(IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<AbsenceRequestService> logger, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ISystemConfigurationService systemConfigurationService) : 
            base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _systemConfigurationService = systemConfigurationService;
        }

        public async Task<Result<bool>> SubmitAbsentRequest(AbsenceRequestDto requestDto)
        {
            try
            {
                var absenceDateConfig = await GetAbsenceDateConfig();

                var slot = await GetSlotByIdAsync(requestDto.SlotId);
                ValidateSlotAvailability(slot, absenceDateConfig);

                var absenceRequest = requestDto.Adapt<AbsenceRequest>();

                return await SaveAbsenceRequestAsync(absenceRequest)
                    ? Success(true)
                    : throw new Exception(MessageConstant.AbsentRequest.Fail.Create);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        private async Task<Slot> GetSlotByIdAsync(Guid slotId)
        {
            return await _unitOfWork.GetRepository<Slot>()
                .SingleOrDefaultAsync(predicate: s => s.Id == slotId) ??
                throw new Exception(MessageConstant.AbsentRequest.Fail.NotFound);
        }

        public async Task<int> GetAbsenceDateConfig()
        {
            var absenceDateConfig = await _systemConfigurationService.GetConfigByKey("RestricedtAbsenceDate");
            if (absenceDateConfig != null)
            {
                return Int32.Parse(absenceDateConfig.Value);
            }
            return 7;
        }

        private static void ValidateSlotAvailability(Slot slot, int absenceDateConfig)
        {
            var allowedDate = slot.Date.AddDays(absenceDateConfig);
            if (DateTime.Now < allowedDate)
            {
                throw new Exception(string.Format(MessageConstant.AbsentRequest.Fail.NotValid, absenceDateConfig));
            }
        }

        private async Task<bool> SaveAbsenceRequestAsync(AbsenceRequest absenceRequest)
        {
            await _unitOfWork.GetRepository<AbsenceRequest>().InsertAsync(absenceRequest);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<Result<bool>> ProcessAbsentRequest(AbsenceApproveRequest absenceApprove)
        {
            try
            {
                var absenceRequest = await _unitOfWork.GetRepository<AbsenceRequest>().SingleOrDefaultAsync(
                    predicate: ar => ar.Id == absenceApprove.RequestId
                );

                absenceRequest.Status = absenceApprove.Status;
                absenceRequest.ApproverId = absenceApprove.ApproverId;
                absenceRequest.Comment = absenceApprove.Comment;
                absenceRequest.ApprovalDate = DateTime.Now;

                _unitOfWork.GetRepository<AbsenceRequest>().UpdateAsync(absenceRequest);

                var isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.AbsentRequest.Fail.Update);
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<bool>> AssignNewCatechist(AssignCatechistRequest assignCatechistRequest)
        {
            try
            {
                var absenceRequest = await _unitOfWork.GetRepository<AbsenceRequest>().SingleOrDefaultAsync(
                    predicate: ar => ar.Id == assignCatechistRequest.RequestId
                );

                if (absenceRequest.Status == RequestStatus.Rejected)
                {
                    throw new Exception(MessageConstant.AbsentRequest.Fail.NotApproved);
                }

                absenceRequest.ReplacementCatechistId = assignCatechistRequest.ReplacementCatechistId;
                absenceRequest.UpdateAt = DateTime.Now;

                _unitOfWork.GetRepository<AbsenceRequest>().UpdateAsync(absenceRequest);

                var isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.AbsentRequest.Fail.Update);
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<List<GetAbsentRequest>>> GetAll(RequestStatus? requestStatus = null)
        {
            try
            {
                var absenceRequests = await _unitOfWork.GetRepository<AbsenceRequest>()
                    .GetListAsync(include: ar => ar.Include(ar => ar.Catechist)
                                                   .Include(ar => ar.ReplacementCatechist)
                                                   .Include(ar => ar.Slot));

                if (requestStatus.HasValue)
                {
                    absenceRequests = absenceRequests.Where(ar => ar.Status == requestStatus.Value).ToList();
                }

                var result = absenceRequests.Adapt<List<GetAbsentRequest>>();

                return Success(result);
            }
            catch (Exception ex)
            {
                return Fail<List<GetAbsentRequest>>(ex.Message);
            }
        }
    }
}
