﻿
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Account;
using CatechistHelper.Domain.Dtos.Requests.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.Authentication;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CatechistController : BaseController<CatechistController>
    {

        private readonly ICatechistService _catechistService;
        public CatechistController(ILogger<CatechistController> logger, ICatechistService catechistService) : base(logger)
        {
            _catechistService = catechistService;
        }

        [HttpGet(ApiEndPointConstant.Catechist.CatechistsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetCatechistResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetCatechistResponse> result = await _catechistService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Catechist.CatechistEndpoint)]
        [ProducesResponseType(typeof(Result<GetCatechistResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetCatechistResponse> result = await _catechistService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Catechist.CatechistsEndpoint)]
        [ProducesResponseType(typeof(Result<GetCatechistResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateCatechistRequest request)
        {
            Result<GetCatechistResponse> result = await _catechistService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Catechist.CatechistEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateCatechistRequest request)
        {
            Result<bool> result = await _catechistService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Catechist.CatechistEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _catechistService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}