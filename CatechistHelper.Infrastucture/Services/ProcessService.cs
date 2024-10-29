using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Process;
using CatechistHelper.Domain.Dtos.Requests.RoleEvent;
using CatechistHelper.Domain.Dtos.Responses.Process;
using CatechistHelper.Domain.Dtos.Responses.RoleEvent;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class ProcessService : BaseService<ProcessService>, IProcessService
    {
        public ProcessService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<ProcessService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetProcessResponse>> Create(CreateProcessRequest request)
        {
            Process process = request.Adapt<Process>();
            Process result = await _unitOfWork.GetRepository<Process>().InsertAsync(process);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Process.Fail.Create);
            }
            return Success(result.Adapt<GetProcessResponse>());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Process process = await _unitOfWork.GetRepository<Process>().SingleOrDefaultAsync(
                    predicate: re => re.Id.Equals(id)) ?? throw new Exception(MessageConstant.Process.Fail.NotFound);
                _unitOfWork.GetRepository<Process>().DeleteAsync(process);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Process.Fail.Delete);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetProcessResponse>> Get(Guid id)
        {
            Process process = await _unitOfWork.GetRepository<Process>().SingleOrDefaultAsync(
                predicate: e => e.Id == id) ?? throw new Exception(MessageConstant.Process.Fail.NotFound);
            return Success(process.Adapt<GetProcessResponse>());
        }

        public async Task<PagingResult<GetProcessResponse>> GetPagination(int page, int size)
        {
            IPaginate<Process> processes =
                   await _unitOfWork.GetRepository<Process>().GetPagingListAsync(
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    processes.Adapt<IPaginate<GetProcessResponse>>(),
                    page,
                    size,
                    processes.Total);
        }

        public async Task<Result<bool>> Update(Guid id, UpdateProcessRequest request)
        {
            try
            {
                Process process = await _unitOfWork.GetRepository<Process>().SingleOrDefaultAsync(
                    predicate: m => m.Id.Equals(id)) ?? throw new Exception(MessageConstant.Process.Fail.NotFound);
                request.Adapt(process);
                _unitOfWork.GetRepository<Process>().UpdateAsync(process);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Process.Fail.Update);
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
