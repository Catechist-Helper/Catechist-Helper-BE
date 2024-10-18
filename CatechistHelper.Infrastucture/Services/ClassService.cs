using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Entities;
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
    public class ClassService : BaseService<ClassService>, IClassService
    {
        public ClassService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<ClassService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<PagingResult<GetClassResponse>> GetPagination(Expression<Func<Class, bool>>? predicate, int page, int size)
        {
            try
            {
                var result = await GetAll(page, size);
                return SuccessWithPaging(
                            result.Adapt<IPaginate<GetClassResponse>>(),
                            page,
                            size,
                            result.Total);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null!;
        }


        public async Task<IPaginate<Class>> GetAll(int page, int size, string? sortBy = null)
        {
            var classes = await _unitOfWork.GetRepository<Class>()
                    .GetPagingListAsync(
                            predicate: a => !a.IsDeleted,
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size,
                            include: a => a.Include(p => p.PastoralYear)
                                           .Include(p=> p.Grade)
                    );

            return classes;
        }


    }
}
