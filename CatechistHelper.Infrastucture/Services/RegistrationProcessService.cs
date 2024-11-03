using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.RegistrationProcess;
using CatechistHelper.Domain.Dtos.Responses.RegistrationProcess;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class RegistrationProcessService : BaseService<RegistrationProcessService>, IRegistrationProcessService
    {
        public RegistrationProcessService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<RegistrationProcessService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetRegistrationProcessResponse>> Create(CreateRegistrationProcessRequest request)
        {
            try
            {
                Registration registraion = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(request.RegistrationId)) ?? throw new Exception(MessageConstant.Registration.Fail.NotFoundRegistration);
                RegistrationProcess registrationProcess = request.Adapt<RegistrationProcess>();
                RegistrationProcess result = await _unitOfWork.GetRepository<RegistrationProcess>().InsertAsync(registrationProcess);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.RegistrationProcess.Fail.CreateRegistrationProcess);
                }
                return Success(_mapper.Map<GetRegistrationProcessResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetRegistrationProcessResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateRegistrationProcessRequest request)
        {
            try
            {
                RegistrationProcess registrationProcess = await _unitOfWork.GetRepository<RegistrationProcess>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.RegistrationProcess.Fail.NotFoundRegistrationProcess);
                request.Adapt(registrationProcess);
                _unitOfWork.GetRepository<RegistrationProcess>().UpdateAsync(registrationProcess);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.RegistrationProcess.Fail.UpdateRegistrationProcess);
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
                RegistrationProcess registrationProcess = await _unitOfWork.GetRepository<RegistrationProcess>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.RegistrationProcess.Fail.NotFoundRegistrationProcess);
                _unitOfWork.GetRepository<RegistrationProcess>().DeleteAsync(registrationProcess);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.RegistrationProcess.Fail.DeleteRegistrationProcess);
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
