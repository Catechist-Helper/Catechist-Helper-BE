using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInTraining;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Infrastructure.Database;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    internal class CatechistInTrainingService : BaseService<CatechistInTrainingService>, ICatechistInTraining
    {
        public CatechistInTrainingService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<CatechistInTrainingService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> AddCatechistToTrainingList(Guid trainingListId, List<CreateCatechistInTrainingRequest> request)
        {
            try
            {
                var trainingListFromDb = await _unitOfWork.GetRepository<TrainingList>().SingleOrDefaultAsync(
                    predicate: tl => tl.Id == trainingListId,
                    include: tl => tl.Include(tl => tl.CatechistInTrainings)
                                     .Include(tl => tl.NextLevel)
                ) ?? throw new Exception(MessageConstant.TrainingList.Fail.NotFoundTrainingList);
                bool isTrainingFinished = trainingListFromDb.TrainingListStatus == TrainingListStatus.Finished;
                var requestedCatechists = await _unitOfWork.GetRepository<Catechist>().GetListAsync(
                    predicate: c => request.Select(cit => cit.Id).Contains(c.Id));
                if (requestedCatechists.Count != request.Count)
                {
                    throw new Exception(MessageConstant.Catechist.Fail.NotFoundCatechist);
                }
                // Remove catechists not in the request
                var catechistsToRemove = trainingListFromDb.CatechistInTrainings
                    .Where(cit => !request.Select(cit => cit.Id).Contains(cit.CatechistId))
                    .ToList();
                if (catechistsToRemove.Any())
                {
                    _unitOfWork.GetRepository<CatechistInTraining>().DeleteRangeAsync(catechistsToRemove);
                }
                // Lists for inserts and updates
                var catechistsToInsert = new List<CatechistInTraining>();
                var catechistsToUpdate = new List<CatechistInTraining>();
                foreach (var catechist in request)
                {
                    var catechistFromDb = trainingListFromDb.CatechistInTrainings
                        .FirstOrDefault(cit => cit.CatechistId == catechist.Id);
                    if (catechistFromDb != null)
                    {
                        if (catechistFromDb.CatechistInTrainingStatus == catechist.Status)
                        {
                            continue;
                        }
                        if (isTrainingFinished || catechist.Status == CatechistInTrainingStatus.Failed)
                        {
                            catechistFromDb.CatechistInTrainingStatus = catechist.Status;
                            catechistsToUpdate.Add(catechistFromDb);
                        }
                        else
                        {
                            throw new Exception(MessageConstant.TrainingList.Fail.NotFinished);
                        }
                    }
                    else
                    {
                        catechistsToInsert.Add(new CatechistInTraining
                        {
                            TrainingListId = trainingListId,
                            CatechistId = catechist.Id,
                            CatechistInTrainingStatus = catechist.Status,
                        });
                    }

                    if (catechist.Status == CatechistInTrainingStatus.Completed)
                    {
                        var catechistEntity = requestedCatechists.FirstOrDefault(c => c.Id == catechist.Id);
                        if (catechistEntity != null)
                        {
                            catechistEntity.LevelId = trainingListFromDb.NextLevelId;
                            await _unitOfWork.GetRepository<CertificateOfCatechist>().InsertAsync(new CertificateOfCatechist
                            {
                                CatechistId = catechistEntity.Id,
                                CertificateId = trainingListFromDb.CertificateId,
                                GrantedDate = DateTime.UtcNow,
                            });
                            _unitOfWork.GetRepository<Catechist>().UpdateAsync(catechistEntity);
                        }
                    }
                }
                // Batch inserts and updates
                if (catechistsToInsert.Any())
                {
                    await _unitOfWork.GetRepository<CatechistInTraining>().InsertRangeAsync(catechistsToInsert);
                }
                if (catechistsToUpdate.Any())
                {
                    _unitOfWork.GetRepository<CatechistInTraining>().UpdateRange(catechistsToUpdate);
                }
                await _unitOfWork.CommitAsync();
                return Success(true);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }
    }
}
