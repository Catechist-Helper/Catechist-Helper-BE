using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Certificate;
using CatechistHelper.Domain.Dtos.Responses.Certificate;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class CertificateService : BaseService<CertificateService>, ICertificateService
    {
        private readonly IFirebaseService _firebaseService;
        public CertificateService(
            IFirebaseService firebaseService,
            IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<CertificateService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _firebaseService = firebaseService;
        }

        public async Task<Result<GetCertificateResponse>> Create(CreateCertificateRequest request)
        {
            Level level = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                predicate: r => r.Id.Equals(request.LevelId)) ?? throw new Exception(MessageConstant.PostCategory.Fail.NotFoundPostCategory); ;

            try
            {
                Certificate certificate = request.Adapt<Certificate>();
                if (request.Image != null)
                {
                    string image = await _firebaseService.UploadImageAsync(request.Image, $"certificate/");
                    certificate.Image = image;
                }
                Certificate result = await _unitOfWork.GetRepository<Certificate>().InsertAsync(certificate);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Post.Fail.CreatePost);
                }
                return Success(_mapper.Map<GetCertificateResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetCertificateResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Certificate certificate = await _unitOfWork.GetRepository<Certificate>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));
                _unitOfWork.GetRepository<Certificate>().UpdateAsync(certificate);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Certificate.Fail.DeleteCertificate);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    // 547 is the SQL Server error code for a foreign key violation
                    return Fail<bool>(MessageConstant.Common.DeleteFail);
                }
                else
                {
                    return Fail<bool>(ex.Message);
                }
            }
        }

        public async Task<Result<GetCertificateResponse>> Get(Guid id)
        {
            try
            {
                Certificate certificate = await _unitOfWork.GetRepository<Certificate>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: a => a.Include(a => a.Level));
                return Success(certificate.Adapt<GetCertificateResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetCertificateResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetCertificateResponse>> GetPagination(Expression<Func<Certificate, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<Certificate> certificates =
                    await _unitOfWork.GetRepository<Certificate>()
                    .GetPagingListAsync(
                            include: a => a.Include(a => a.Level),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        certificates.Adapt<IPaginate<GetCertificateResponse>>(),
                        page,
                        size,
                        certificates.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }
    }
}
