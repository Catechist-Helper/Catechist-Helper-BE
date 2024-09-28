using CatechistHelper.Domain.Pagination;
using System.Net;

namespace CatechistHelper.Domain.Common
{
    public class Result<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public Result()
        {

        }
    }

    public class PagingResult<T> : Result<IPaginate<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagingResult()
        {

        }

        public PagingResult(IPaginate<T> data)
        {
            PageNumber = data.Page;
            PageSize = data.Size;
            TotalCount = data.Total;
        }
    }
}
