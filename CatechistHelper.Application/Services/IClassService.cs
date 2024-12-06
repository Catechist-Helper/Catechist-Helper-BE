using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Class;
using CatechistHelper.Domain.Dtos.Responses.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Slot;
using CatechistHelper.Domain.Models;

namespace CatechistHelper.Application.Services
{
    public interface IClassService
    {
        Task<PagingResult<GetClassResponse>> GetPagination(ClassFilter? filter, int page, int size);
        Task<PagingResult<GetCatechistInClassResponse>> GetCatechistInClassById(Guid id, int page, int size);
        Task<PagingResult<GetSlotResponse>> GetSlotsByClassId(Guid id, int page, int size);
        Task<Result<bool>> UpdateCatechistInClass(Guid id, CatechistInClassRequest classRequest);
        Task<Result<bool>> UpdateClassRoom(Guid id, RoomOfClassRequest request);
        Task<Result<bool>> CreateClass(ClassRequest request);
        Task<Result<bool>> UpdateClass(Guid id, ClassRequest request);
        Task<Result<bool>> DeleteClass(Guid id);
    }
}
