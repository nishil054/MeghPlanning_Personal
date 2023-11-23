using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ManageLeaves.Dto;
using AbpProjects.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageLeaves
{
    public interface IManageLeavesAppService : IApplicationService
    {
        //PagedResultDto<UserDto> GetUserdata(GetUserInputDto Input);
        List<UserDto> GetUserdata(GetUserInputDto Input);
        Task UpdateBulkLeaves(List<EmployeeLeaveDto> inputList);

    }
}
