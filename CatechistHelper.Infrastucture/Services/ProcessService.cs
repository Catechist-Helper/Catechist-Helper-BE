using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Process;
using CatechistHelper.Domain.Dtos.Responses.MemberOfProcess;
using CatechistHelper.Domain.Dtos.Responses.Process;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public async Task<PagingResult<GetMemberOfProcessRepsonse>> GetMembersByProcessId(Guid id, int page, int size)
        {
            try
            {
                IPaginate<MemberOfProcess> catechists =
                    await _unitOfWork.GetRepository<MemberOfProcess>()
                    .GetPagingListAsync(
                            predicate: mop => mop.ProcessId == id,
                            include: mop => mop.Include(mop => mop.Account),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        catechists.Adapt<IPaginate<GetMemberOfProcessRepsonse>>(),
                        page,
                        size,
                        catechists.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<GetProcessResponse>> Create(CreateProcessRequest request)
        {
            try
            {
                Event eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                    predicate: e => e.Id == request.EventId) ?? throw new Exception(MessageConstant.Event.Fail.NotFound);
                Process process = request.Adapt<Process>();
                Process result = await _unitOfWork.GetRepository<Process>().InsertAsync(process);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Process.Fail.Create);
                }
                return Success(result.Adapt<GetProcessResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetProcessResponse>(ex.Message);
            }
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

        public async Task<Result<bool>> Update(Guid id, UpdateProcessRequest request)
        {
            try
            {
                Event eventFromDb = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                    predicate: e => e.Id == request.EventId) ?? throw new Exception(MessageConstant.Event.Fail.NotFound);
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
