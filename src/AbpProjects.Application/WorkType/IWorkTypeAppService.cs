using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.WorkType.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.WorkType
{
  public  interface IWorkTypeAppService : IApplicationService
    {
        Task CreateWorkType(CreateWorkTypeDto input);
        bool WorkTypeExsistence(WorkTypeDto input);
        bool WorkTypeExsistenceById(WorkTypeDto input);
        List<WorkTypeDto> GetWorkType();
        Task<WorkTypeDto> GetDataById(EntityDto input);
        Task UpdateWorkType(EditWorkTypeDto input);
        Task DeleteWorkType(EntityDto input);
        PagedResultDto<WorkTypeDto> GetWorkTypeList(GetWorkTypeDto input);
        PagedResultDto<WorkTypeDto> GetWorkTypeData(GetWorkTypeDto input);
    }
}
