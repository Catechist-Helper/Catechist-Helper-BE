﻿using CatechistHelper.API.Utils;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Interview;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Azure.Communication.Rooms;
using Azure.Communication.Identity;
using Azure.Communication;

namespace CatechistHelper.Infrastructure.Services
{
    public class InterviewService : BaseService<InterviewService>, IInterviewService
    {
        private readonly IConfiguration _configuration;

        public InterviewService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<InterviewService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _configuration = configuration;
        }

        private async Task ValidateInterviewScheduling(DateTime meetingTime)
        {
            // Get the restricted update days from configuration using the enum key.
            var configEntry = await _unitOfWork.GetRepository<SystemConfiguration>()
                .SingleOrDefaultAsync(predicate: sc => sc.Key == EnumUtil.GetDescriptionFromEnum(SystemConfigurationEnum.RestrictedUpdateDaysBeforeInterview));
            _ = int.TryParse(configEntry.Value, out var days);
            int minDaysBeforeInterview = days;
            DateTime minimumAllowedDate = DateTime.UtcNow.AddDays(minDaysBeforeInterview);
            // Ensure the requested meeting time meets the minimum scheduling requirement.
            if (meetingTime < minimumAllowedDate)
            {
                throw new Exception($"Interviews must be scheduled at least {minDaysBeforeInterview} days in advance.");
            }
        }

        public async Task<Result<GetInterviewResponse>> Create(CreateInterviewRequest request)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(request.RegistrationId)) ?? throw new Exception(MessageConstant.Registration.Fail.NotFoundRegistration);
                await ValidateInterviewScheduling(request.MeetingTime);
                Interview interview = request.Adapt<Interview>();
                // Add recruiters
                if (request.Accounts != null && request.Accounts.Count != 0)
                {
                    foreach (Guid accountId in request.Accounts)
                    {
                        Account account = await _unitOfWork.GetRepository<Account>()
                            .SingleOrDefaultAsync(predicate: a => a.Id == accountId) ?? throw new Exception(MessageConstant.Account.Fail.NotFoundAccount);

                        RecruiterInInterview rii = new()
                        {
                            Interview = interview,
                            AccountId = account.Id
                        };

                        interview.RecruiterInInterviews.Add(rii);
                    }
                }

                var interviewAddress = ContentMailUtil.INTERVIEW_ADDRESS;

                if (request.InterviewType == InterviewType.Online)
                {
                    interview.RegistrationId = registration.Id;
                    interview = await CreateOnlineInterview(request.MeetingTime, interview, registration);
                }
                else
                {
                    string formattedMeetingTime = interview.MeetingTime.ToString("HH:mm, dd/MM/yyyy");
                    MailUtil.SendEmail(
                        registration.Email,
                        ContentMailUtil.Title_AnnounceInterviewSchedule,
                        ContentMailUtil.AnnounceInterviewSchedule(
                            registration.FullName,
                            formattedMeetingTime,
                            interviewAddress
                        )
                    );
                }

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
                    predicate: a => a.Id.Equals(id)
                    , include: a => a.Include(x => x.Registration))
                    ?? throw new Exception(MessageConstant.Interview.Fail.NotFoundInterview);
                if (request.MeetingTime != null)
                {
                    await ValidateInterviewScheduling((DateTime)request.MeetingTime);
                }

                // Add recruiters
                if (request.Accounts != null && request.Accounts.Count != 0)
                {
                    var recruiters = await _unitOfWork.GetRepository<RecruiterInInterview>().GetListAsync(predicate: r => r.InterviewId == id);
                    string? interviewURL = null;
                    if (recruiters.Any()) {
                        var recruiter = recruiters.FirstOrDefault();
                        if (recruiter != null && !string.IsNullOrEmpty(recruiter.OnlineRoomUrl))
                        {
                            interviewURL = recruiter.OnlineRoomUrl;
                        }
                        _unitOfWork.GetRepository<RecruiterInInterview>().DeleteRangeAsync(recruiters);
                    }
                    foreach (Guid accountId in request.Accounts)
                    {
                        Account account = await _unitOfWork.GetRepository<Account>()
                            .SingleOrDefaultAsync(predicate: a => a.Id == accountId) ?? throw new Exception(MessageConstant.Account.Fail.NotFoundAccount);

                        Validator.EnsureNonNull(account);

                        await _unitOfWork.GetRepository<RecruiterInInterview>().InsertAsync(new RecruiterInInterview
                        {
                            InterviewId = interview.Id,
                            AccountId = account.Id,
                            OnlineRoomUrl = interviewURL,
                        });
                    }
                }

                if (interview.Registration.Status == RegistrationStatus.Approved_Duyet_Don
                    && !interview.IsPassed
                    && request.MeetingTime != interview.MeetingTime)
                {
                    string? formattedMeetingTime = request.MeetingTime?.ToString("HH:mm, dd/MM/yyyy");
                    MailUtil.SendEmail(
                        interview.Registration.Email,
                        ContentMailUtil.Title_AnnounceUpdateInterviewSchedule,
                        ContentMailUtil.AnnounceUpdateInterviewSchedule(
                            interview.Registration.FullName,
                            request.Reason ?? "",
                            formattedMeetingTime,
                            ContentMailUtil.INTERVIEW_ADDRESS
                        ),
                        ""
                    );
                }
                if (interview.Registration.Status == RegistrationStatus.Approved_Phong_Van
                    && request.IsPassed)
                {
                    MailUtil.SendEmail(
                        interview.Registration.Email,
                        ContentMailUtil.Title_AnnounceApproveInterview,
                        ContentMailUtil.AnnounceApproveInterview(
                            interview.Registration.FullName
                        ),
                        ""
                    );
                }
                if (interview.Registration.Status == RegistrationStatus.Rejected_Phong_Van
                    && !request.IsPassed)
                {
                    MailUtil.SendEmail(
                        interview.Registration.Email,
                        ContentMailUtil.Title_AnnounceRejectInterview,
                        ContentMailUtil.AnnounceRejectInterview(
                            interview.Registration.FullName
                        ),
                        ""
                    );
                }

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
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.Interview.Fail.NotFoundInterview);

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

        public async Task<Interview> CreateOnlineInterview(DateTimeOffset validFrom, Interview interview, Registration registration)
        {
            // Fetch connection string from configuration
            var connectionString = _configuration["AzureCommunication:ConnectionString"];
            RoomsClient roomsClient = new(connectionString);
            CommunicationIdentityClient identityClient = new(connectionString);

            // Create a candidate user
            var candidate = await identityClient.CreateUserAsync();

            // Create identifiers and participants for interviewers
            var identifiers = new List<CommunicationUserIdentifier>();
            var participants = new List<RoomParticipant>();

            foreach (var recruiter in interview.RecruiterInInterviews)
            {
                var user = await identityClient.CreateUserAsync();
                identifiers.Add(user);
                participants.Add(new RoomParticipant(user) { Role = ParticipantRole.Presenter });
            }

            // Add candidate to participants
            participants.Add(new RoomParticipant(candidate));

            // Define room validity period and create the room
            var validUntil = validFrom.AddDays(1);
            CommunicationRoom createdRoom = await roomsClient.CreateRoomAsync(
                new CreateRoomOptions
                {
                    ValidFrom = validFrom,
                    ValidUntil = validUntil,
                    Participants = participants
                }
            );

            string roomId = createdRoom.Id;

            // Generate meeting URLs for interviewers and assign to RecruiterInInterviews
            var meetingUrl = _configuration["FrontendUrl:Meeting"];

            for (int i = 0; i < interview.RecruiterInInterviews.Count; i++)
            {
                var token = await identityClient.GetTokenAsync(identifiers[i], [CommunicationTokenScope.VoIP]);
                var interviewUrl = $"{meetingUrl}?roomid={roomId}&token={token.Value.Token}";

                interview.RecruiterInInterviews.ElementAt(i).OnlineRoomUrl = interviewUrl;

                var account = await _unitOfWork.GetRepository<Account>()
                    .SingleOrDefaultAsync(predicate: a => a.Id == interview.RecruiterInInterviews.ElementAt(i).AccountId);

                MailUtil.SendEmail(
                    account.Email,
                    ContentMailUtil.Title_AnnounceInterviewSchedule,
                    ContentMailUtil.NotifyInterviewerSchedule(account.FullName, registration.FullName, HolidayService.TimeToString(interview.MeetingTime), interviewUrl)
                );
            }


            var candidateToken = await identityClient.GetTokenAsync(candidate, [CommunicationTokenScope.VoIP]);
            var candidateUrl = $"{meetingUrl}?roomid={roomId}&token={candidateToken.Value.Token}";

            MailUtil.SendEmail(
                registration.Email,
                ContentMailUtil.Title_AnnounceInterviewSchedule,
                ContentMailUtil.AnnounceOnlineInterviewSchedule(registration.FullName, HolidayService.TimeToString(interview.MeetingTime), candidateUrl)
            );

            return interview;
        }

        public async Task<Result<bool>> PostEvaluation(Guid id, CreateEvaluationRequest request)
        {
            try
            {
                RecruiterInInterview recruiterInInterview = await _unitOfWork.GetRepository<RecruiterInInterview>()
                    .SingleOrDefaultAsync(predicate: r => r.AccountId == request.RecruiterAccountId && r.InterviewId == id);

                recruiterInInterview.Evaluation = request.Evaluation;

                _unitOfWork.GetRepository<RecruiterInInterview>().UpdateAsync(recruiterInInterview);
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
