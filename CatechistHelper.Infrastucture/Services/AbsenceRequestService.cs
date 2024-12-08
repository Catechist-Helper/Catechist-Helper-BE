using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Responses.AbsenceRequest;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CatechistHelper.Infrastructure.Services
{
    public class AbsenceRequestService: BaseService<AbsenceRequestService>, IAbsenceRequestService
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IFirebaseService _firebaseService;
        public AbsenceRequestService(IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<AbsenceRequestService> logger, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ISystemConfigurationService systemConfigurationService, IFirebaseService firebaseService) : 
            base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _systemConfigurationService = systemConfigurationService;
            _firebaseService = firebaseService;
        }

        public async Task<Result<bool>> SubmitAbsentRequest(AbsenceRequestDto requestDto)
        {
            try
            {
                var absenceDateConfig = await GetAbsenceDateConfig();

                var slot = await GetSlotByIdAsync(requestDto.SlotId);
                ValidateSlotAvailability(slot, absenceDateConfig);

                var absenceRequest = requestDto.Adapt<AbsenceRequest>();

                if (!requestDto.RequestImages.IsNullOrEmpty()) {
                    string folderName = $"absence/{requestDto.ReplacementCatechistId}";
                    string[] absenceRequestImages = await _firebaseService.UploadImagesAsync(requestDto.RequestImages, folderName);
                    foreach (var image in absenceRequestImages)
                    {
                        await _unitOfWork.GetRepository<RequestImage>().InsertAsync(new RequestImage
                        {
                            AbsenceRequestId = absenceRequest.Id,
                            ImageUrl = image
                        });
                    }
                }

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
            if (DateTime.Now > allowedDate)
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

                if (absenceRequest.Status != RequestStatus.Approved)
                {
                    throw new Exception(MessageConstant.AbsentRequest.Fail.NotApproved);
                }

                absenceRequest.ReplacementCatechistId = assignCatechistRequest.ReplacementCatechistId;

                var slot = await _unitOfWork.GetRepository<Slot>()
                    .SingleOrDefaultAsync(
                        predicate: s => s.Id == absenceRequest.SlotId,
                        include: s => s.Include(s => s.CatechistInSlots)
                    );

                Validator.EnsureNonNull(slot);

                var catechistToReplace = slot.CatechistInSlots
                    .FirstOrDefault(c => c.CatechistId == absenceRequest.CatechistId);

                Validator.EnsureNonNull(catechistToReplace);

                slot.CatechistInSlots.Remove(catechistToReplace);
                _unitOfWork.GetRepository<CatechistInSlot>().DeleteAsync(catechistToReplace);


                var existCatechistInSlot = await _unitOfWork.GetRepository<CatechistInSlot>().SingleOrDefaultAsync(
                    predicate: c => c.CatechistId == assignCatechistRequest.ReplacementCatechistId
                    && c.Slot.Date == slot.Date,
                    include: c => c.Include(s => s.Slot)
                );

                if (existCatechistInSlot != null)
                {
                    _unitOfWork.GetRepository<CatechistInSlot>().DeleteAsync(existCatechistInSlot);
                }

                var newCatechistInSlot = new CatechistInSlot
                {
                    CatechistId = assignCatechistRequest.ReplacementCatechistId,
                    Type = assignCatechistRequest.Type.ToString(),
                    SlotId = absenceRequest.SlotId
                };

                await _unitOfWork.GetRepository<CatechistInSlot>().InsertAsync(newCatechistInSlot);
                slot.CatechistInSlots.Add(newCatechistInSlot);

                _unitOfWork.GetRepository<Slot>().UpdateAsync(slot);

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

        public async Task<Result<List<GetAbsentRequest>>> GetAll(RequestStatus requestStatus, Guid? cId)
        {
            try
            {
                var absenceRequests = await _unitOfWork.GetRepository<AbsenceRequest>()
                    .GetListAsync(predicate : a => a.Status == requestStatus,
                                  include: ar => ar.Include(ar => ar.Catechist)
                                                   .Include(ar => ar.ReplacementCatechist)
                                                   .Include(ar => ar.Slot));
                
                if(cId != null)
                {
                    absenceRequests = absenceRequests.Where(a => a.Catechist.Id == cId).ToList();
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
