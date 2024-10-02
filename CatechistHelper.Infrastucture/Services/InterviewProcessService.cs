using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.InterviewProcess;
using CatechistHelper.Domain.Dtos.Responses.InterviewProcess;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class InterviewProcessService : BaseService<InterviewProcessService>, IInterviewProcessService
    {
        public InterviewProcessService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<InterviewProcessService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetInterviewProcessResponse>> Create(CreateInterviewProcessRequest request)
        {
            try
            {
                Registration registraion = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(request.RegistrationId)) ?? throw new Exception(MessageConstant.Registration.Fail.NotFoundRegistration);

                InterviewProcess interviewProcess = request.Adapt<InterviewProcess>();

                InterviewProcess result = await _unitOfWork.GetRepository<InterviewProcess>().InsertAsync(interviewProcess);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.InterviewProcess.Fail.CreateInterviewProcess);
                }
                return Success(_mapper.Map<GetInterviewProcessResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetInterviewProcessResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateInterviewProcessRequest request)
        {
            try
            {
                InterviewProcess interviewProcess = await _unitOfWork.GetRepository<InterviewProcess>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.InterviewProcess.Fail.NotFoundInterviewProcess);

                request.Adapt(interviewProcess);

                _unitOfWork.GetRepository<InterviewProcess>().UpdateAsync(interviewProcess);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.InterviewProcess.Fail.UpdateInterviewProcess);
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
                InterviewProcess interviewProcess = await _unitOfWork.GetRepository<InterviewProcess>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.InterviewProcess.Fail.NotFoundInterviewProcess);

                _unitOfWork.GetRepository<InterviewProcess>().DeleteAsync(interviewProcess);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.InterviewProcess.Fail.DeleteInterviewProcess);
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
