﻿using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInClass;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CatechistInClassController : BaseController<CatechistInClassController>
    {
        private readonly ICatechistInClassService _catechistInClassService;

        public CatechistInClassController(ICatechistInClassService catechistInClassService,
            ILogger<CatechistInClassController> logger) : base(logger)
        {
            _catechistInClassService = catechistInClassService;
        }

        [HttpPost(ApiEndPointConstant.CatechistInClass.CatechistInClassesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCatechistInClassRequest request)
        {
            Result<bool> result = await _catechistInClassService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.CatechistInClass.CatechistInClassEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCatechistInClassRequest request)
        {
            Result<bool> result = await _catechistInClassService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}