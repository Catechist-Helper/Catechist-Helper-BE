using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Major;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Dtos.Responses.Major;
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
    public class MajorService : BaseService<MajorService>, IMajorService
    {
        public MajorService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<MajorService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetMajorResponse>> Get(Guid id)
        {
            try
            {
                Major major = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                return Success(major.Adapt<GetMajorResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetMajorResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetMajorResponse>> GetPagination(int page, int size)
        {
            try
            {
                IPaginate<Major> majors =
                    await _unitOfWork.GetRepository<Major>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        majors.Adapt<IPaginate<GetMajorResponse>>(),
                        page,
                        size,
                        majors.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<GetMajorResponse>> Create(CreateMajorRequest request)
        {
            try
            {
                Major major = request.Adapt<Major>();

                Major result = await _unitOfWork.GetRepository<Major>().InsertAsync(major);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Major.Fail.CreateMajor);
                }
                return Success(result.Adapt<GetMajorResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetMajorResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateMajorRequest request)
        {
            try
            {
                Major major = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.Major.Fail.NotFoundMajor);

                request.Adapt(major);

                _unitOfWork.GetRepository<Major>().UpdateAsync(major);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Major.Fail.UpdateMajor);
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
                Major major = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.Major.Fail.NotFoundMajor);
                _unitOfWork.GetRepository<Major>().DeleteAsync(major);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Major.Fail.DeleteMajor);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<bool>> CreateLevelOfMajor(Guid MajorId, Guid LevelId)
        {
            try
            {
                Major major = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(
                   predicate: c => c.Id.Equals(MajorId)) ?? throw new Exception(MessageConstant.Major.Fail.NotFoundMajor);
                Level certificate = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                    predicate: c => c.Id.Equals(LevelId)) ?? throw new Exception(MessageConstant.Level.Fail.NotFoundLevel);
                TeachingQualification teachingQualification = new TeachingQualification
                {
                    MajorId = MajorId,
                    LevelId = LevelId
                };
                TeachingQualification result = await _unitOfWork.GetRepository<TeachingQualification>().InsertAsync(teachingQualification);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.CreateAccount);
                }
                return Success(isSuccessful);
            } catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<PagingResult<GetLevelResponse>> GetLevelOfMajor(Guid id, int page, int size)
        {
            try
            {
                Major major = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(
                    predicate: c => c.Id.Equals(id)) ?? throw new Exception(MessageConstant.Major.Fail.NotFoundMajor);
                IPaginate<Level> levels = await _unitOfWork.GetRepository<TeachingQualification>().GetPagingListAsync(
                                predicate: t => t.MajorId.Equals(id),
                                selector: t => t.Level,
                                page: page,
                                size: size
                            );
                return SuccessWithPaging(
                        levels.Adapt<IPaginate<GetLevelResponse>>(),
                        page,
                        size,
                        levels.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<PagingResult<GetCatechistResponse>> GetQualifiedCatechistByMajorId(Guid id, int page, int size)
        {
            try
            {
                ICollection<Guid> levels = await _unitOfWork.GetRepository<TeachingQualification>().GetListAsync(
                            predicate: t => t.MajorId.Equals(id),
                            selector: t => t.LevelId
                        );
                IPaginate<Catechist> catechists =
                    await _unitOfWork.GetRepository<Catechist>()
                    .GetPagingListAsync(
                            predicate: c => levels.Contains(c.LevelId)
                                            && c.IsDeleted == false
                                            && c.IsTeaching == true,
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        catechists.Adapt<IPaginate<GetCatechistResponse>>(),
                        page,
                        size,
                        catechists.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }
    }
}
