using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Responses.BudgetTransaction;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class BudgetTransactionService : BaseService<BudgetTransactionService>, IBudgetTransactionService
    {
        private readonly IFirebaseService _firebaseService;
        public BudgetTransactionService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<BudgetTransactionService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, IFirebaseService firebaseService)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _firebaseService = firebaseService;
        }

        public async Task<Result<GetBudgetTransactionResponse>> Create(CreateBudgetTransactionRequest request)
        {
            BudgetTransaction budgetTransaction = request.Adapt<BudgetTransaction>();

            if (request.TransactionImages.Count > 0)
            {
                string[] budgetTransactionImages = await _firebaseService.UploadImagesAsync(request.TransactionImages, $"event/{request.EventId}");
                foreach (var image in budgetTransactionImages)
                {
                    await _unitOfWork.GetRepository<TransactionImage>().InsertAsync(new TransactionImage
                    {
                        BudgetTransaction = budgetTransaction,
                        ImageUrl = image
                    });
                }
            }

            BudgetTransaction result = await _unitOfWork.GetRepository<BudgetTransaction>().InsertAsync(budgetTransaction);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.BudgetTransaction.Fail.Create);
            }
            return Success(result.Adapt<GetBudgetTransactionResponse>());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                BudgetTransaction budgetTransaction = await _unitOfWork.GetRepository<BudgetTransaction>().SingleOrDefaultAsync(
                    predicate: bt => bt.Id.Equals(id)) ?? throw new Exception(MessageConstant.BudgetTransaction.Fail.NotFound);
                _unitOfWork.GetRepository<BudgetTransaction>().DeleteAsync(budgetTransaction);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.BudgetTransaction.Fail.Delete);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetBudgetTransactionResponse>> Get(Guid id)
        {
            BudgetTransaction budgetTransaction = await _unitOfWork.GetRepository<BudgetTransaction>().SingleOrDefaultAsync(
                predicate: bt => bt.Id == id) ?? throw new Exception(MessageConstant.BudgetTransaction.Fail.NotFound);
            return Success(budgetTransaction.Adapt<GetBudgetTransactionResponse>());
        }

        public async Task<Result<bool>> Update(Guid id, UpdateBudgetTransactionRequest request)
        {
            try
            {
                BudgetTransaction budgetTransaction = await _unitOfWork.GetRepository<BudgetTransaction>().SingleOrDefaultAsync(
                    predicate: m => m.Id.Equals(id)) ?? throw new Exception(MessageConstant.BudgetTransaction.Fail.NotFound);
                request.Adapt(budgetTransaction);
                _unitOfWork.GetRepository<BudgetTransaction>().UpdateAsync(budgetTransaction);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.BudgetTransaction.Fail.Update);
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
