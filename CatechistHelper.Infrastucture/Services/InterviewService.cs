using CatechistHelper.API.Utils;
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

        public async Task<Result<GetInterviewResponse>> Create(CreateInterviewRequest request)
        {
            try
            {
                Registration registration = await _unitOfWork.GetRepository<Registration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(request.RegistrationId)) ?? throw new Exception(MessageConstant.Registration.Fail.NotFoundRegistration);

                var configEntry = await _unitOfWork.GetRepository<SystemConfiguration>()
                    .SingleOrDefaultAsync(predicate: sc => sc.Key == EnumUtil.GetDescriptionFromEnum(SystemConfigurationEnum.RestrictedUpdateDaysBeforeInterview));
                _ = int.TryParse(configEntry.Value, out var days);
                int minDaysBeforeInterview = days;
                DateTime minimumAllowedDate = DateTime.UtcNow.AddDays(minDaysBeforeInterview);
                if (request.MeetingTime < minimumAllowedDate)
                {
                    throw new Exception($"Interviews must be scheduled at least {minDaysBeforeInterview} days in advance.");
                }
                Interview interview = request.Adapt<Interview>();

                interview.Registration = registration;

                var interviewAddress = ContentMailUtil.INTERVIEW_ADDRESS;

                if (request.InterviewType == InterviewType.Online)
                {
                    interview = await CreateRoom(request.MeetingTime, interview);
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
                        ),
                        ""
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
                var configEntry = await _unitOfWork.GetRepository<SystemConfiguration>()
                    .SingleOrDefaultAsync(predicate: sc => sc.Key == EnumUtil.GetDescriptionFromEnum(SystemConfigurationEnum.RestrictedUpdateDaysBeforeInterview));
                _ = int.TryParse(configEntry.Value, out var days);
                int minDaysBeforeInterview = days;
                DateTime minimumAllowedDate = DateTime.UtcNow.AddDays(minDaysBeforeInterview);
                if (interview.MeetingTime < minimumAllowedDate)
                {
                    throw new Exception($"Interviews must be scheduled at least {minDaysBeforeInterview} days in advance.");
                }
                if (interview.Registration.Status == RegistrationStatus.Approved_Duyet_Don
                    && !interview.IsPassed
                    && request.MeetingTime != interview.MeetingTime)
                {
                    string formattedMeetingTime = request.MeetingTime.ToString("HH:mm, dd/MM/yyyy");
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

        public async Task<Interview> CreateRoom(DateTimeOffset validFrom, Interview interview)
        {
            var connectionString = _configuration["AzureCommunication:ConnectionString"];
            var roomsClient = new RoomsClient(connectionString);
            var identityClient = new CommunicationIdentityClient(connectionString);

            var candidate = await CreateCandidate(identityClient);
            var identifiers = await CreateIdentifiersForRecruiters(interview.RecruiterInInterviews, identityClient);

            var participants = CreateRoomParticipants(identifiers, candidate);
            var roomId = await CreateCommunicationRoom(validFrom, participants, roomsClient);

            await AssignRoomUrlsToRecruiters(interview.RecruiterInInterviews, identifiers, roomId, identityClient);

            var candidateUrl = await GenerateCandidateUrl(candidate, roomId, identityClient);
            NotifyCandidate(interview.Registration.Email, candidateUrl);

            return interview;
        }

        private async Task<CommunicationUserIdentifier> CreateCandidate(CommunicationIdentityClient identityClient)
        {
            return await identityClient.CreateUserAsync();
        }

        private async Task<List<CommunicationUserIdentifier>> CreateIdentifiersForRecruiters(
            ICollection<RecruiterInInterview> recruiters,
            CommunicationIdentityClient identityClient)
        {
            var identifiers = new List<CommunicationUserIdentifier>();

            foreach (var recruiter in recruiters)
            {
                var user = await identityClient.CreateUserAsync();
                identifiers.Add(user);
            }

            return identifiers;
        }

        private List<RoomParticipant> CreateRoomParticipants(
            List<CommunicationUserIdentifier> identifiers,
            CommunicationUserIdentifier candidate)
        {
            var participants = identifiers
                .Select(identifier => new RoomParticipant(identifier) { Role = ParticipantRole.Presenter })
                .ToList();

            participants.Add(new RoomParticipant(candidate));
            return participants;
        }

        private async Task<string> CreateCommunicationRoom(
            DateTimeOffset validFrom,
            List<RoomParticipant> participants,
            RoomsClient roomsClient)
        {
            DateTimeOffset validUntil = validFrom.AddDays(1);
            CommunicationRoom createdRoom = await roomsClient.CreateRoomAsync(
                options : new CreateRoomOptions
                {
                    ValidFrom = validFrom,
                    ValidUntil = validUntil,
                    Participants = participants
                }
            );

            return createdRoom.Id;
        }

        private async Task AssignRoomUrlsToRecruiters(
            ICollection<RecruiterInInterview> recruiters,
            List<CommunicationUserIdentifier> identifiers,
            string roomId,
            CommunicationIdentityClient identityClient)
        {
            var meetingUrl = _configuration["FrontendUrl:Meeting"];
            int index = 0;

            foreach (var recruiter in recruiters)
            {
                var token = await identityClient.GetTokenAsync(identifiers[index], new[] { CommunicationTokenScope.VoIP });
                var interviewUrl = $"{meetingUrl}?roomid={roomId}&token={token.Value.Token}";
                recruiter.RoomUrl = interviewUrl;
                index++;
            }
        }

        private async Task<string> GenerateCandidateUrl(
            CommunicationUserIdentifier candidate,
            string roomId,
            CommunicationIdentityClient identityClient)
        {
            var meetingUrl = _configuration["FrontendUrl:Meeting"];
            var token = await identityClient.GetTokenAsync(candidate, new[] { CommunicationTokenScope.VoIP });
            return $"{meetingUrl}?roomid={roomId}&token={token.Value.Token}";
        }

        private void NotifyCandidate(string email, string candidateUrl)
        {
            MailUtil.SendEmail(
                email,
                ContentMailUtil.Title_AnnounceInterviewSchedule,
                candidateUrl,
                ""
            );
        }




    }
}
