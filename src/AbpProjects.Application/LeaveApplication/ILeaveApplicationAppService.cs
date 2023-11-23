using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.LeaveApplication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveApplication
{
    public interface ILeaveApplicationAppService : IApplicationService
    {
        List<LeaveTypeDto> GetLeaveType();
        List<LeaveStatusDto> GetLeaveStatus();
        Task CreateLeave(CreateLeaveDto input);
        PagedResultDto<LeaveDto> GetLeaveData(GetInputDto input);
        PagedResultDto<LeaveDto> GetLeaveDataReport(GetInputDto input);
        List<LeaveDto> GetLeaveDataReportExport(GetInputDto input);
        ListResultDto<LeaveDto> GetLeaveDataById(EntityDto input);
        Task UpdateLeaveCancelRequest(UpdateStatusDto input);

        //Leave Application To Do
        PagedResultDto<LeaveDto> GetLeaveToDoData(GetInputDto input);
        Task ApproveLeaveRequest(EntityDto input);
        Task RejectLeaveRequest(EntityDto input);
    }
}
