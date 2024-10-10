using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Account;
using CatechistHelper.Domain.Dtos.Requests.Authentication;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.Authentication;
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

        [HttpPost(ApiEndPointConstant.Account.AccountsEndPoint)]
        [ProducesResponseType(typeof(Result<GetAccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateAccountRequest request)
        {
            Result<GetAccountResponse> result = await _accountService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateAccountRequest request)
        {
            Result<bool> result = await _accountService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _accountService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost(ApiEndPointConstant.Authentication.LoginEndPoint)]
        [ProducesResponseType(typeof(Result<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Result<LoginResponse> result = await _accountService.LoginAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
