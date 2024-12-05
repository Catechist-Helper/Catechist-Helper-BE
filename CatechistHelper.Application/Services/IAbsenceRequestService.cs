using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Responses.AbsenceRequest;
using CatechistHelper.Domain.Entities;

namespace CatechistHelper.Application.Services
{
    public interface IAbsenceRequestService
    {
        Task<Result<bool>> SubmitAbsentRequest(AbsenceRequestDto requestDto);
        Task<Result<bool>> ProcessAbsentRequest(AbsenceApproveRequest absenceApprove);
        Task<Result<bool>> AssignNewCatechist(AssignCatechistRequest assignCatechistRequest);
        Task<Result<List<GetAbsentRequest>>> GetAll(RequestStatus requestStatus, Guid? cId);
    }
}
