using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Certificate;
using CatechistHelper.Domain.Dtos.Responses.Certificate;
using CatechistHelper.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface ICertificateService
    {
        Task<PagingResult<GetCertificateResponse>> GetPagination(Expression<Func<Certificate, bool>>? predicate, int page, int size);
        Task<Result<GetCertificateResponse>> Get(Guid id);
        Task<Result<GetCertificateResponse>> Create(CreateCertificateRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
