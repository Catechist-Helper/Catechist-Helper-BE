using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Interview;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class InterviewService : BaseService<InterviewService>, IInterviewService
    {
        public InterviewService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<InterviewService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetInterviewResponse>> Create(CreateInterviewRequest request)
        {
            try
            {
                Registration registraion = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(request.RegistrationId)) ?? throw new Exception(MessageConstant.Registration.Fail.NotFoundRegistration);

                Interview interview = request.Adapt<Interview>();

                Interview result = await _unitOfWork.GetRepository<Interview>().InsertAsync(interview);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Interview.Fail.CreateInterview);
                }
                return Success(_mapper.Map<GetInterviewResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetInterviewResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateInterviewRequest request)
        {
            try
            {
                Interview interview = await _unitOfWork.GetRepository<Interview>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                request.Adapt(interview);

                _unitOfWork.GetRepository<Interview>().UpdateAsync(interview);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Interview.Fail.UpdateInterview);
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
                Interview interview = await _unitOfWork.GetRepository<Interview>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                _unitOfWork.GetRepository<Interview>().DeleteAsync(interview);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Interview.Fail.DeleteInterview);
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
