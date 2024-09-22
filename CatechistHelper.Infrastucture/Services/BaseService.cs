using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Application.Repositories;
using MapsterMapper;
using System.Net;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace CatechistHelper.Infrastructure.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<ApplicationDbContext> _unitOfWork;
        protected ILogger<T> _logger;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;

        public BaseService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<T> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetUsernameFromJwt()
        {
            string username = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return username;
        }

        protected string GetRoleFromJwt()
        {
            string role = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role)!;
            return role;
        }

        protected Result<TEntity> Success<TEntity>(TEntity data)
        {
            return new Result<TEntity>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        protected PagingResult<TEntity> SuccessWithPaging<TEntity>(IPaginate<TEntity> data, int page, int size, int total)
        {
            return new PagingResult<TEntity>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                PageNumber = page,
                PageSize = size,
                TotalCount = total
            };
        }

        protected PagingResult<TEntity> Success<TEntity>(IPaginate<TEntity> data)
        {
            return new PagingResult<TEntity>(data)
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        protected Result<TEntity> Fail<TEntity>(string message)
        {
            return new Result<TEntity>
            {
                Message = message,
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }

        protected Result<TEntity> BadRequest<TEntity>(string message)
        {
            return new Result<TEntity>
            {
                Message = message,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        protected Result<TEntity> BadRequests<TEntity>(string message)
        {
            return new Result<TEntity>
            {
                Message = message,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        protected Result<TEntity> NotFound<TEntity>(string message)
        {
            return new Result<TEntity>
            {
                Message = message,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        protected PagingResult<TEntity> NotFounds<TEntity>(string message)
        {
            return new PagingResult<TEntity>
            {
                Message = message,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

    }
}
