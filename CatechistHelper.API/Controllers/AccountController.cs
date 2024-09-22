using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Responses;
using CatechistHelper.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }

        [HttpGet(ApiEndPointConstant.Account.AccountsEndPoint)]
        [ProducesResponseType(typeof(PagingResult<GetAccountResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetAccountResponse> result = await _accountService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesResponseType(typeof(Result<List<GetAccountResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetAccountResponse> result = await _accountService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
