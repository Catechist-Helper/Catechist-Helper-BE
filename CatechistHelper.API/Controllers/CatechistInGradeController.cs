﻿using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInGrade;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CatechistInGradeController : BaseController<CatechistInGradeController>
    {
        private readonly ICatechistInGrade _catechistInGrade;

        public CatechistInGradeController(
            ICatechistInGrade catechistInGrade,
            ILogger<CatechistInGradeController> logger)
            : base(logger)
        {
            _catechistInGrade = catechistInGrade;
        }

        [HttpPost(ApiEndPointConstant.CatechistInGrade.CatechistInGradesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCatechistToGrade([FromBody] CreateCatechistInGradeRequest request)
        {
            Result<bool> result = await _catechistInGrade.AddCatechistToGrade(request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}