using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Registration;
using CatechistHelper.Domain.Dtos.Responses.Registration;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Dtos.Responses.InterviewProcess;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using CatechistHelper.Application.Extensions;
using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Infrastructure.Utils;

namespace CatechistHelper.Infrastructure.Services
{
    public class RegistrationService : BaseService<RegistrationService>, IRegistrationService
    {
        private readonly IFirebaseService _firebaseService;

        public RegistrationService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<RegistrationService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor, IFirebaseService firebaseService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _firebaseService = firebaseService;
        }

        public async Task<Result<GetRegistrationResponse>> Get(Guid id)
        {
            try
            {
                Registration application = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: a => a.Include(a => a.CertificateOfCandidates)
                                   .Include(a => a.Interviews)
                                   .Include(a => a.InterviewProcesses)
                                   .Include(a => a.Accounts));

                return Success(application.Adapt<GetRegistrationResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetRegistrationResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetRegistrationResponse>> GetPagination(RegistrationFilter? filter, int page, int size)
        {
            try
            {

                IPaginate<Registration> applications =
                    await _unitOfWork.GetRepository<Registration>()
                    .GetPagingListAsync(
                            predicate: BuildGetPaginationQuery(filter),
                            orderBy: a => a.OrderBy(x => x.Status).ThenByDescending(x => x.CreatedAt),
                            include: a => a.Include(a => a.CertificateOfCandidates)
                                           .Include(a => a.Interviews)
                                           .Include(a => a.InterviewProcesses)
                                           .Include(a => a.Accounts),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        applications.Adapt<IPaginate<GetRegistrationResponse>>(),
                        page,
                        size,
                        applications.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        private Expression<Func<Registration, bool>> BuildGetPaginationQuery(RegistrationFilter? filter)
        {
            Expression<Func<Registration, bool>> filterQuery = x => x.IsDeleted == false;
            if (filter.StartDate != null && filter.EndDate == null)
            {
                filterQuery = filterQuery.AndAlso(p =>
                    p.CreatedAt >= filter.StartDate && p.CreatedAt <= filter.StartDate.Value.AddDays(1));
            }
            else if (filter.StartDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.CreatedAt >= filter.StartDate);
            }
            if (filter.EndDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.CreatedAt <= filter.EndDate);
            }
            if (filter.Status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(filter.Status));
            }

            return filterQuery;
        }

        public async Task<Result<GetRegistrationResponse>> Create(CreateRegistrationRequest request)
        {
            try
            {
                Registration registration = request.Adapt<Registration>();

                string folderCandidateName = ToLowerCaseAndDashed(request.FullName);

                if (request.CertificateOfCandidates != null)
                {
                    string[] certificateOfCandidates = await _firebaseService.UploadImagesAsync(request.CertificateOfCandidates, $"registration/{folderCandidateName}");

                    foreach (string certificateOfCandidate in certificateOfCandidates)
                    {
                        await _unitOfWork.GetRepository<CertificateOfCandidate>().InsertAsync(new CertificateOfCandidate
                        {
                            Registration = registration,
                            ImageUrl = certificateOfCandidate,
                        });
                    }
                }

                Registration result = await _unitOfWork.GetRepository<Registration>().InsertAsync(registration);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Registration.Fail.CreateRegistration);
                }

                MailUtil.SendEmail(request.Email, ContentMailUtil.Title_ThankingForRegistration,
                    ContentMailUtil.ThankingForRegistration(request.FullName), "");

                return Success(_mapper.Map<GetRegistrationResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetRegistrationResponse>(ex.Message);
            }
        }

        private string ToLowerCaseAndDashed(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            name = name.ToLowerInvariant();
            name = name.Replace(' ', '-');

            return name;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateRegistrationRequest request)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: a => a.Include(x => x.Interviews));

                registration.Status = request.Status;

                if (request.Accounts != null && request.Accounts.Count != 0)
                {
                    var listRecruiterOfRegistration = await _unitOfWork.GetRepository<Recruiter>().GetListAsync(predicate: r => r.RegistrationId == registration.Id);
                    if (listRecruiterOfRegistration.Any()) _unitOfWork.GetRepository<Recruiter>().DeleteRangeAsync(listRecruiterOfRegistration);

                    foreach (Guid accountId in request.Accounts)
                    {
                        Account account = await _unitOfWork.GetRepository<Account>()
                            .SingleOrDefaultAsync(predicate: a => a.Id == accountId);

                        if (account == null)
                            return NotFound<bool>(MessageConstant.Account.Fail.NotFoundAccount);

                        await _unitOfWork.GetRepository<Recruiter>().InsertAsync(new Recruiter
                        {
                            AccountId = accountId,
                            Registration = registration,
                        });
                    }
                }             
      
                if (registration.Status == Domain.Enums.RegistrationStatus.Pending 
                    && request.Status == Domain.Enums.RegistrationStatus.Rejected_Duyet_Don)
                {
                    MailUtil.SendEmail(registration.Email, ContentMailUtil.Title_AnnounceRejectRegistration,
                        ContentMailUtil.AnnounceRejectRegistration(registration.FullName, request.Reason ?? ""), "");
                }

                _unitOfWork.GetRepository<Registration>().UpdateAsync(registration);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Registration.Fail.UpdateRegistration);
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
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));
                registration.IsDeleted = true;
                _unitOfWork.GetRepository<Registration>().UpdateAsync(registration);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Registration.Fail.DeleteRegistration);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<GetInterviewResponse>>> GetInterviewOfApplication(Guid id)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                if (registration == null) return NotFound<IEnumerable<GetInterviewResponse>>(MessageConstant.Registration.Fail.NotFoundRegistration);

                IEnumerable<Interview> interviews = await _unitOfWork.GetRepository<Interview>().GetListAsync(
                    predicate: i => i.RegistrationId == id);

                return Success(interviews.Adapt<IEnumerable<GetInterviewResponse>>());
            }
            catch (Exception ex)
            {
                return BadRequest<IEnumerable<GetInterviewResponse>>(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<GetInterviewProcessResponse>>> GetInterviewProcessOfApplication(Guid id)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                if (registration == null) return NotFound<IEnumerable<GetInterviewProcessResponse>>(MessageConstant.Registration.Fail.NotFoundRegistration);

                IEnumerable<InterviewProcess> interviewProcesses = await _unitOfWork.GetRepository<InterviewProcess>().GetListAsync(
                    predicate: i => i.RegistrationId == id);

                return Success(interviewProcesses.Adapt<IEnumerable<GetInterviewProcessResponse>>());
            }
            catch (Exception ex)
            {
                return BadRequest<IEnumerable<GetInterviewProcessResponse>>(ex.Message);
            }
        }
    }
}
