using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Responses.BudgetTransaction;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class BudgetTransactionController : BaseController<BudgetTransactionController>
    {
        private readonly IBudgetTransactionService _budgetTransactionService;

        public BudgetTransactionController(
            ILogger<BudgetTransactionController> logger, 
            IBudgetTransactionService budgetTransactionService) 
            : base(logger)
        {
            _budgetTransactionService = budgetTransactionService;
        }

        [HttpGet(ApiEndPointConstant.BudgetTransaction.BudgetTransactionEndpoint)]
        [ProducesResponseType(typeof(Result<GetBudgetTransactionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetBudgetTransactionResponse> result = await _budgetTransactionService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.BudgetTransaction.BudgetTransactionsEndpoint)]
        [ProducesResponseType(typeof(Result<GetBudgetTransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateBudgetTransactionRequest request)
        {
            Result<GetBudgetTransactionResponse> result = await _budgetTransactionService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.BudgetTransaction.BudgetTransactionEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBudgetTransactionRequest request)
        {
            Result<bool> result = await _budgetTransactionService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.BudgetTransaction.BudgetTransactionEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _budgetTransactionService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
