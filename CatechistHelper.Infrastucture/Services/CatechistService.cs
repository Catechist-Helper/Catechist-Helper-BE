﻿using Azure.Core;
using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.CatechistInTraining;
using CatechistHelper.Domain.Dtos.Responses.CertificateOfCatechist;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace CatechistHelper.Infrastructure.Services
{
    public class CatechistService : BaseService<CatechistService>, ICatechistService
    {
        private readonly IFirebaseService _firebaseService;

        public CatechistService(
            IFirebaseService firebaseService,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<CatechistService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            this._firebaseService = firebaseService;
        }

        public async Task<Result<GetCatechistResponse>> Create(CreateCatechistRequest request)
        {
            try
            {
                var result = await CreateAsync(request);
                return Success(result.Adapt<GetCatechistResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetCatechistResponse>(ex.Message);
            }
        }

        public async Task<Catechist> CreateAsync(CreateCatechistRequest request)
        {
            var catechist = request.Adapt<Catechist>();     
            string convertName = ConvertName(request.FullName);
            var maxCodeWithNumber = await _unitOfWork.GetRepository<Catechist>().SingleOrDefaultAsync(
                predicate: x => x.Code.StartsWith(convertName),
                selector: x => x.Code,
                orderBy: x => x.OrderByDescending(x => x.Code));
            // Extract the numeric part of the code (if any)
            int nextNumber = 1; // Default starting number
            if (maxCodeWithNumber != null && maxCodeWithNumber.Length > convertName.Length)
            {
                // Parse the numeric part
                string numericPart = maxCodeWithNumber.Substring(convertName.Length);
                if (int.TryParse(numericPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Generate the next unique code
            string uniqueCode = $"{convertName}{nextNumber}";
            catechist.Code = uniqueCode;
            if (request.ImageUrl != null)
            {
                string avatar = await _firebaseService.UploadImageAsync(request.ImageUrl, $"catechist/");
                catechist.ImageUrl = avatar;
            }

            var result = await _unitOfWork.GetRepository<Catechist>().InsertAsync(catechist);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Catechist.Fail.CreateCatechist);
            }
            return result;
        }

        private static string ConvertName(string fullName)
        {
            string RemoveDiacritics(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                // Normalize to FormD, which separates base characters and diacritics
                var normalizedText = text.Normalize(NormalizationForm.FormD);

                // Use a StringBuilder to construct the text without diacritics
                var stringBuilder = new StringBuilder();
                foreach (char c in normalizedText)
                {
                    // Only include characters that are not combining marks
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                // Return the normalized string to FormC (standard form)
                return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            }

            // Split the full name into parts
            string[] nameParts = fullName.Split(' ');

            // Remove diacritics from each part of the name
            for (int i = 0; i < nameParts.Length; i++)
            {
                nameParts[i] = RemoveDiacritics(nameParts[i]);
            }

            // Get the first name
            string firstName = nameParts[nameParts.Length - 1];

            // Get the initials from the other names (excluding the first name)
            string initials = "";
            for (int i = 0; i < nameParts.Length - 1; i++)
            {
                initials += nameParts[i][0];
            }

            // Combine the first name with the initials
            return firstName + initials;
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                var catechist = await GetById(id);

                catechist.IsDeleted = true;

                _unitOfWork.GetRepository<Catechist>().UpdateAsync(catechist);
                return Success(await _unitOfWork.CommitAsync() > 0);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetCatechistResponse>> Get(Guid id)
        {
            var result = await GetById(id);
            return Success(result.Adapt<GetCatechistResponse>());
        }

        public async Task<Catechist> GetById(Guid id)
        {
            var catechist = await _unitOfWork.GetRepository<Catechist>()
                .SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && !c.IsDeleted,
                include: c => c.Include(n => n.ChristianName)
                                .Include(n => n.Level)
                                .Include(n => n.Account)
                                .Include(n => n.Certificates)
                                .Include(c => c.TrainingLists)
                );
            ArgumentNullException.ThrowIfNull(nameof(id));
            return catechist;
        }

        public async Task<IPaginate<Catechist>> GetAll(int page, int size, string? sortBy = null)
        {
            var catechists = await _unitOfWork.GetRepository<Catechist>()
                    .GetPagingListAsync(
                include: c => c.Include(n => n.ChristianName)
                                .Include(n => n.Level)
                                .Include(n => n.Account)
                                .Include(n => n.Certificates),
                            predicate: a => !a.IsDeleted,
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt)
                                           .OrderByDescending(x => x.IsTeaching),
                            page: page,
                            size: size
                    );

            return catechists;
        }

        public async Task<PagingResult<GetCatechistResponse>> GetPagination(Expression<Func<Catechist, bool>>? predicate, int page, int size)
        {
            try
            {
                Validator.CheckPageInput(page, size);
                var result = await GetAll(page, size);
                return SuccessWithPaging(
                            result.Adapt<IPaginate<GetCatechistResponse>>(),
                            page,
                            size,
                            result.Total);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null!;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateCatechistRequest request)
        {
            try
            {
                var catechist = await _unitOfWork.GetRepository<Catechist>().SingleOrDefaultAsync(
                        predicate: c => c.Id.Equals(id) && !c.IsDeleted,
                        include: c => c.Include(n => n.Account)
                );

                if (catechist.IsTeaching && !request.IsTeaching)
                {
                    var classes = await _unitOfWork.GetRepository<CatechistInClass>()
                        .GetListAsync(predicate: c => c.CatechistId == id && c.Class.ClassStatus == ClassStatus.Active,
                                      include: c => c.Include(c => c.Class));

                    if (classes.Count > 0)
                    {
                        // Get the class names
                        var classNames = string.Join(", ", classes.Select(c => c.Class.Name));

                        // Throw an exception with the class names listed
                        throw new Exception($"Catechist has slots in active classes: {classNames}");
                    }
                }

                if (catechist.ImageUrl != null)
                {
                    await _firebaseService.DeleteImageAsync(catechist.ImageUrl);
                    catechist.ImageUrl = null;
                }

                if (request.ImageUrl != null)
                {
                    string image = await _firebaseService.UploadImageAsync(request.ImageUrl, "catechist/");
                    catechist.ImageUrl = image;
                    catechist.Account.Avatar = image;
                }

                catechist.Account.Phone = request.Phone;   

                request.Adapt(catechist);
                _unitOfWork.GetRepository<Catechist>().UpdateAsync(catechist);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Catechist.Fail.UpdateCatechist);
                }

                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<bool>> UpdateImage(Guid id, UpdateImageRequest request)
        {
            try
            {
                var catechist = await GetById(id);
                if (catechist.ImageUrl != null)
                {
                    await _firebaseService.DeleteImageAsync(catechist.ImageUrl);
                }

                if (request.ImageUrl != null)
                {

                    string avatar = await _firebaseService.UploadImageAsync(request.ImageUrl, $"catechist/");
                    catechist.ImageUrl = avatar;
                }

                _unitOfWork.GetRepository<Catechist>().UpdateAsync(catechist);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Catechist.Fail.UpdateCatechist);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<PagingResult<GetCertificateOfCatechistResponse>> GetCertificateOfCatechistById(Guid id, int page, int size)
        {
            try
            {
                IPaginate<CertificateOfCatechist> certificates =
                    await _unitOfWork.GetRepository<CertificateOfCatechist>()
                    .GetPagingListAsync(
                            predicate: c => c.CatechistId.Equals(id),
                            include: c => c.Include(c => c.Certificate),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        certificates.Adapt<IPaginate<GetCertificateOfCatechistResponse>>(),
                        page,
                        size,
                        certificates.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<PagingResult<ClassOfCatechist>> GetCatechistClasses(Guid id, string pastoralYear, int page, int size, ClassStatus status)
        {
            try
            {
                IPaginate<CatechistInClass> catechistInClasses = await _unitOfWork.GetRepository<CatechistInClass>()
                    .GetPagingListAsync(
                        predicate: c => c.CatechistId == id && c.Class.PastoralYear.Name == pastoralYear && c.Class.ClassStatus == status,
                        include: c => c.Include(cl => cl.Class)
                                       .ThenInclude(y => y.PastoralYear),
                        page: page,
                        size: size);

                // Adapt result to ClassOfCatechist
                return SuccessWithPaging(
                    catechistInClasses.Adapt<IPaginate<ClassOfCatechist>>(),
                    page,
                    size,
                    catechistInClasses.Total);
            }
            catch (Exception ex)
            {

            }
            // In case of an exception, you can return a failure result or null
            return null!;
        }

        public async Task<PagingResult<GradeOfCatechist>> GetCatechistGrades(Guid id, int page, int size)
        {
            try
            {
                IPaginate<CatechistInGrade> catechistInGrades = await _unitOfWork.GetRepository<CatechistInGrade>()
                    .GetPagingListAsync(
                        predicate: c => c.CatechisteId == id,
                        include: c => c.Include(cl => cl.Grade),
                        page: page,
                        size: size);

                // Adapt result to ClassOfCatechist
                return SuccessWithPaging(
                    catechistInGrades.Adapt<IPaginate<GradeOfCatechist>>(),
                    page,
                    size,
                    catechistInGrades.Total);
            }
            catch (Exception ex)
            {

            }
            // In case of an exception, you can return a failure result or null
            return null!;
        }

        public async Task<Result<GetTrainingInfomationResponse>> GetTrainingInformationOfCatechist(Guid id)
        {
            try
            {
                var catechist = await _unitOfWork.GetRepository<Catechist>()
                    .SingleOrDefaultAsync(predicate: c => c.Id == id,
                                          include: c=> c.Include(c=> c.CatechistInTrainings)
                                                          .ThenInclude(c=> c.TrainingList)
                                                            .ThenInclude(c=> c.NextLevel)
                                                         .Include(c => c.CatechistInTrainings)
                                                          .ThenInclude(c => c.TrainingList)
                                                            .ThenInclude(c => c.PreviousLevel));

                var response = catechist.Adapt<GetTrainingInfomationResponse>();
                return Success(response);
            }
            catch (Exception ex)
            {
                return Fail<GetTrainingInfomationResponse>(ex.Message);
            }
        }

    }
}
