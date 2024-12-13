﻿using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.TrainingList;
using CatechistHelper.Domain.Dtos.Responses.CatechistInTraining;
using CatechistHelper.Domain.Dtos.Responses.TrainingList;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class TrainingListService : BaseService<TrainingListService>, ITrainingListService
    {
        public TrainingListService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<TrainingListService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetTrainingListResponse>> Create(CreateTrainingListRequest request)
        {
            try
            {
                Level previouseLevel = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                    predicate: l => l.Id == request.PreviousLevelId) ?? throw new Exception(MessageConstant.Level.Fail.NotFoundLevel);
                Level nextLevel = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                    predicate: l => l.Id == request.NextLevelId,
                    include: l => l.Include(l => l.Certificates)) ?? throw new Exception(MessageConstant.Level.Fail.NotFoundLevel);
                Certificate certificate = await _unitOfWork.GetRepository<Certificate>().SingleOrDefaultAsync(
                    predicate: c => c.Id == request.CertificateId) ?? throw new Exception(MessageConstant.Certificate.Fail.NotFoundCertificate);
                if (!nextLevel.Certificates.Any(c => c.LevelId == certificate.LevelId))
                {
                    throw new Exception(MessageConstant.Certificate.Fail.UnsuitableLevel);
                }
                if (previouseLevel.HierarchyLevel >= nextLevel.HierarchyLevel)
                {
                    throw new Exception(MessageConstant.TrainingList.Fail.InvalidLevel);
                }
                if (nextLevel.HierarchyLevel > previouseLevel.HierarchyLevel + 1)
                {
                    throw new Exception(MessageConstant.TrainingList.Fail.InvalidNextLevel);
                }
                TrainingList trainingList = request.Adapt<TrainingList>();
                TrainingList result = await _unitOfWork.GetRepository<TrainingList>().InsertAsync(trainingList);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.TrainingList.Fail.CreateTrainingList);
                }
                return Success(result.Adapt<GetTrainingListResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetTrainingListResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                TrainingList trainingList = await _unitOfWork.GetRepository<TrainingList>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.TrainingList.Fail.NotFoundTrainingList);
                trainingList.IsDeleted = true;
                _unitOfWork.GetRepository<TrainingList>().UpdateAsync(trainingList);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.TrainingList.Fail.DeleteTrainingList);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetTrainingListResponse>> Get(Guid id)
        {
            try
            {
                TrainingList trainingList = await _unitOfWork.GetRepository<TrainingList>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: tl => tl.Include(tl => tl.PreviousLevel)
                                     .Include(tl => tl.NextLevel)
                                     .Include(tl => tl.Certificate)) ?? throw new Exception(MessageConstant.TrainingList.Fail.NotFoundTrainingList);

                return Success(trainingList.Adapt<GetTrainingListResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetTrainingListResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetCatechistInTrainingResponse>> GetAllCatechistInTrainingById(Guid id, int page, int size)
        {
            try
            {
                IPaginate<CatechistInTraining> catechists =
                    await _unitOfWork.GetRepository<CatechistInTraining>()
                    .GetPagingListAsync(
                            predicate: c => c.TrainingListId.Equals(id),
                            include: c => c.Include(n => n.Catechist.ChristianName)
                                .Include(n => n.Catechist.Level)
                                .Include(n => n.Catechist.Account)
                                .Include(n => n.Catechist.Certificates),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        catechists.Adapt<IPaginate<GetCatechistInTrainingResponse>>(),
                        page,
                        size,
                        catechists.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<PagingResult<GetTrainingListResponse>> GetPagination(Expression<Func<TrainingList, bool>>? predicate, int page, int size)
        {
            try
            {
                IPaginate<TrainingList> trainingLists =
                    await _unitOfWork.GetRepository<TrainingList>()
                    .GetPagingListAsync(
                            predicate: tl => tl.IsDeleted == false,
                            include: tl => tl.Include(tl => tl.PreviousLevel)
                                             .Include(tl => tl.NextLevel)
                                             .Include(tl => tl.Certificate),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        trainingLists.Adapt<IPaginate<GetTrainingListResponse>>(),
                        page,
                        size,
                        trainingLists.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateTrainingListRequest request)
        {
            try
            {
                TrainingList trainingList = await _unitOfWork.GetRepository<TrainingList>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: tl => tl.Include(tl => tl.CatechistInTrainings)) ?? throw new Exception(MessageConstant.TrainingList.Fail.NotFoundTrainingList);
                Level previouseLevel = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                   predicate: l => l.Id == request.PreviousLevelId) ?? throw new Exception(MessageConstant.Level.Fail.NotFoundLevel);
                Level nextLevel = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                    predicate: l => l.Id == request.NextLevelId,
                    include: l => l.Include(l => l.Certificates)) ?? throw new Exception(MessageConstant.Level.Fail.NotFoundLevel);
                Certificate certificate = await _unitOfWork.GetRepository<Certificate>().SingleOrDefaultAsync(
                    predicate: c => c.Id == request.CertificateId) ?? throw new Exception(MessageConstant.Certificate.Fail.NotFoundCertificate);
                if (!nextLevel.Certificates.Any(c => c.LevelId == certificate.LevelId))
                {
                    throw new Exception(MessageConstant.Certificate.Fail.UnsuitableLevel);
                }
                if (previouseLevel.HierarchyLevel >= nextLevel.HierarchyLevel)
                {
                    throw new Exception(MessageConstant.TrainingList.Fail.InvalidLevel);
                }
                if (nextLevel.HierarchyLevel > previouseLevel.HierarchyLevel + 1)
                {
                    throw new Exception(MessageConstant.TrainingList.Fail.InvalidNextLevel);
                }
                if (request.TrainingListStatus == TrainingListStatus.Training)
                {
                    foreach (var catechist in trainingList.CatechistInTrainings)
                    {
                        catechist.CatechistInTrainingStatus = CatechistInTrainingStatus.Training;
                    }
                }
                request.Adapt(trainingList);
                _unitOfWork.GetRepository<TrainingList>().UpdateAsync(trainingList);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.TrainingList.Fail.UpdateTrainingList);
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
