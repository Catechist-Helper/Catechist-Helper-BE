﻿using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.CertificateOfCandidate;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Dtos.Responses.RegistrationProcess;
using CatechistHelper.Domain.Enums;
using System.Text.Json.Serialization;

namespace CatechistHelper.Domain.Dtos.Responses.Registration
{
    public class GetRegistrationResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsTeachingBefore { get; set; }
        public int YearOfTeaching { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public RegistrationStatus Status { get; set; }
        public List<GetCertificateOfCandidateResponse> CertificateOfCandidates { get; set; }
        public GetInterviewResponse Interview { get; set; }
        public List<GetRegistrationProcessResponse> RegistrationProcesses { get; set; }
    }
}
