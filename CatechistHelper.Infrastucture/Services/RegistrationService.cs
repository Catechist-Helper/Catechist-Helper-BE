﻿using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Registration;
using CatechistHelper.Domain.Dtos.Responses.Registration;
using CatechistHelper.Domain.Dtos.Responses.Interview;
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
using CatechistHelper.Domain.Models;
using CatechistHelper.Domain.Dtos.Responses.RegistrationProcess;
using Microsoft.IdentityModel.Tokens;

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
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: r => r.Id.Equals(id),
                    include: r => r.Include(r => r.CertificateOfCandidates)
                                   .Include(r => r.Interview).ThenInclude(i => i.Accounts)
                                           .ThenInclude(i => i.RecruiterInInterviews)
                                   .Include(r => r.RegistrationProcesses));
                return Success(registration.Adapt<GetRegistrationResponse>());
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
                IPaginate<Registration> registrations =
                    await _unitOfWork.GetRepository<Registration>()
                    .GetPagingListAsync(
                            predicate: BuildGetPaginationQuery(filter),
                            orderBy: r => r.OrderBy(r => r.Status).ThenByDescending(r => r.CreatedAt),
                            include: r => r.Include(r => r.CertificateOfCandidates)
                                           .Include(r => r.Interview).ThenInclude(i => i.Accounts)
                                           .ThenInclude(i => i.RecruiterInInterviews)
                                           .Include(r => r.RegistrationProcesses),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        registrations.Adapt<IPaginate<GetRegistrationResponse>>(),
                        page,
                        size,
                        registrations.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        private Expression<Func<Registration, bool>> BuildGetPaginationQuery(RegistrationFilter? filter)
        {
            Expression<Func<Registration, bool>> filterQuery = r => r.IsDeleted == false;
            if (filter.StartDate != null && filter.EndDate == null)
            {
                filterQuery = filterQuery.AndAlso(r =>
                    r.CreatedAt >= filter.StartDate && r.CreatedAt <= filter.StartDate.Value.AddDays(1));
            }
            else if (filter.StartDate != null)
            {
                filterQuery = filterQuery.AndAlso(r => r.CreatedAt >= filter.StartDate);
            }
            if (filter.EndDate != null)
            {
                filterQuery = filterQuery.AndAlso(r => r.CreatedAt <= filter.EndDate);
            }
            if (filter.Year != null)
            {
                int year = int.Parse(filter.Year);
                filterQuery = filterQuery.AndAlso(r => r.CreatedAt.Year == year);
            }
            if (filter.Status != null)
            {
                filterQuery = filterQuery.AndAlso(r => r.Status.Equals(filter.Status));
            }
            if (filter.InterviewStartDate != null)
            {
                filterQuery = filterQuery.AndAlso(r => r.Interview != null && r.Interview.MeetingTime >= filter.InterviewStartDate);
            }
            if (filter.InterviewEndDate != null)
            {
                filterQuery = filterQuery.AndAlso(r => r.Interview != null && r.Interview.MeetingTime <= filter.InterviewEndDate);
            }
            return filterQuery;
        }

        public async Task<Result<GetRegistrationResponse>> Create(CreateRegistrationRequest request)
        {
            try
            {
                var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: r=> r.Email == request.Email);

                Validator.EnsureNonExist(account);

                Registration registration = request.Adapt<Registration>();
                // Add certificates
                if (request.CertificateOfCandidates != null)
                {
                    string[] certificateOfCandidates = await _firebaseService.UploadImagesAsync(request.CertificateOfCandidates, $"registration/{registration.Id}");

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
                // Send mail after regis
                MailUtil.SendEmail(request.Email, ContentMailUtil.Title_ThankingForRegistration,
                    ContentMailUtil.ThankingForRegistration(request.FullName), "");
                return Success(_mapper.Map<GetRegistrationResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetRegistrationResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateRegistrationRequest request)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: a => a.Include(x => x.Interview));

                registration.Status = request.Status;
      
                if (request.Status == Domain.Enums.RegistrationStatus.Rejected_Duyet_Don)
                {
                    MailUtil.SendEmail(registration.Email, ContentMailUtil.Title_AnnounceRejectRegistration,
                        ContentMailUtil.AnnounceRejectRegistration(registration.FullName, request.Reason ?? ""), "");
                }

                if (!request.Reason.IsNullOrEmpty())
                {
                    registration.Note = request.Reason;
                }
                if (!request.Note.IsNullOrEmpty())
                {
                    registration.Note = request.Note;
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
                    predicate: a => a.Id.Equals(id),
                    include: r => r.Include(r => r.Interview).ThenInclude(i => i.RecruiterInInterviews)
                                   .Include(r => r.CertificateOfCandidates)
                                   .Include(r => r.RegistrationProcesses)) ?? throw new Exception(MessageConstant.Registration.Fail.NotFoundRegistration);
                _unitOfWork.GetRepository<Registration>().DeleteAsync(registration);
                await _firebaseService.DeleteImagesAsync(registration.CertificateOfCandidates.Select(coc => coc.ImageUrl).ToList());
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

        public async Task<Result<GetInterviewResponse>> GetInterviewOfRegistration(Guid id)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: r => r.Id.Equals(id)) ?? throw new Exception(MessageConstant.Registration.Fail.NotFoundRegistration);
                Interview interview = await _unitOfWork.GetRepository<Interview>().SingleOrDefaultAsync(
                    predicate: i => i.RegistrationId == id,
                    include: i => i.Include(i => i.Accounts));
                return Success(interview.Adapt<GetInterviewResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetInterviewResponse>(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<GetRegistrationProcessResponse>>> GetRegistrationProcessOfApplication(Guid id)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                if (registration == null) return NotFound<IEnumerable<GetRegistrationProcessResponse>>(MessageConstant.Registration.Fail.NotFoundRegistration);

                IEnumerable<RegistrationProcess> registrationProcesses = await _unitOfWork.GetRepository<RegistrationProcess>().GetListAsync(
                    predicate: i => i.RegistrationId == id);

                return Success(registrationProcesses.Adapt<IEnumerable<GetRegistrationProcessResponse>>());
            }
            catch (Exception ex)
            {
                return BadRequest<IEnumerable<GetRegistrationProcessResponse>>(ex.Message);
            }
        }
    }
}
