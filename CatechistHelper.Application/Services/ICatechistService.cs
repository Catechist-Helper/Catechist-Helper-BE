﻿using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.CatechistInTraining;
using CatechistHelper.Domain.Dtos.Responses.CertificateOfCatechist;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistService
    {
        Task<PagingResult<GetCatechistResponse>> GetPagination(Expression<Func<Catechist, bool>>? predicate, int page, int size);
        Task<Result<GetCatechistResponse>> Get(Guid id);
        Task<PagingResult<GetCertificateOfCatechistResponse>> GetCertificateOfCatechistById(Guid id, int page, int size);
        Task<Result<GetCatechistResponse>> Create(CreateCatechistRequest request);
        Task<Result<bool>> Update(Guid id, UpdateCatechistRequest request);
        Task<Result<bool>> Delete(Guid id);
        Task<Result<bool>> UpdateImage(Guid id, UpdateImageRequest request);
        Task<PagingResult<ClassOfCatechist>> GetCatechistClasses(Guid id, string pastoralYear,int page, int size, ClassStatus status);
        Task<PagingResult<GradeOfCatechist>> GetCatechistGrades(Guid id, int page, int size);
        Task<Result<GetTrainingInfomationResponse>> GetTrainingInformationOfCatechist(Guid id);
    }
}
