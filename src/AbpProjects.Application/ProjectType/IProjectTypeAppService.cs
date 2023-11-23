using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ProjectType.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectType
{
 public  interface IProjectTypeAppService : IApplicationService
    {
        Task CreateProjectType(CreateProjectTypeDto input);
        bool ProjectTypeExsistence(ProjectTypeDto input);
        bool ProjectTypeExsistenceById(ProjectTypeDto input);
        List<ProjectTypeDto> GetProjectType();
        Task<ProjectTypeDto> GetDataById(EntityDto input);
        Task UpdateProjectType(EditProjectTypeDto input);
        Task DeleteProjectType(EntityDto input);
        PagedResultDto<ProjectTypeDto> GetProjectTypeList(GetProjectTypeDto input);
        PagedResultDto<ProjectTypeDto> GetProjectTypeData(GetProjectTypeDto input);
    }
}
