using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.NotificationServices.Dto;
using AbpProjects.Project.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ListResultDto<ProjectDto>> GetProjects();
        Task CreateProject(CreateProjectDto input);
        Task<ProjectDto> GetProjectEdit(EntityDto input);
        Task UpdateProject(EditProjectDto input);
        Task DeleteProject(EntityDto input);
        //bool ProjectExsistence(CreateProjectDto input);
        bool ProjectExsistence(ExsistProjectValidationDto input);
        //bool ProjectExsistenceById(CreateProjectDto input);
        PagedResultDto<ProjectDto> GetProjectList(GetProjectDto input);
        PagedResultDto<ProjectDto> GetAllProjectList(GetProjectDto input);
        PagedResultDto<ProjectDto> GetActiveProjectList(GetProjectDto input);
        PagedResultDto<ProjectDto> GetAmcProjectList(GetProjectDto input);
        List<ProjectDto> ExportGetCompletedProjectList(ImportGetProjectDto input);
        PagedResultDto<ProjectDto> GetCompletedProjectList(GetProjectDto input);
        PagedResultDto<ProjectDto> GetInvoiceCollectionProjectList(GetProjectDto input);
        PagedResultDto<ProjectDto> GetOnGoingProjectList(GetProjectDto input);
        List<ProjectDto> ExportGetHoldProjectList(ImportGetProjectDto input);
        PagedResultDto<ProjectDto> GetOnHoldProjectList(GetProjectDto input);

        //Projects Without Client
        List<ProjectDto> ExportGetProjectsWithoutClientList(ImportGetProjectDto input);
        PagedResultDto<ProjectDto> GetProjectsWithoutClientList(GetProjectDto input);
        PagedResultDto<ProjectDto> GetProjectData(GetProjectDto input);
        Task<ListResultDto<ProjectDetailsDto>> GetprojectDetailsList(EntityDto input);
        Task<ListResultDto<GetStatusDto>> GetStatus();
        Task<ListResultDto<GetStatusDto>> GetActiveProjectStatus();
        Task<ListResultDto<GetStatusDto>> GetAmcProjectStatus();
        Task<ListResultDto<GetStatusDto>> GetCompletedProjectStatus();
        Task<ListResultDto<GetStatusDto>> GetInvoiceCollectionProjectStatus();
        Task<ListResultDto<GetStatusDto>> GetOnGoingProjectStatus();
        Task<ListResultDto<GetStatusDto>> GetOnHoldStatus();
        Task<ProjectViewDto> GetProjectViewById(EntityDto input);
        Task<string> GetProjectName(EntityDto input);

        Task<EditProjectTypeByProject> GetProjectTypeByProjectEdit(MasterInputs input);

        Task UpdateProjectType(ProjectTypeUpdate input);

        Task CreateProjectType(createProjectTypeByProjectDto input);
        Task DeleteProjectType(EntityDto input);
        Task UpdateProjectStatus(UpdateProjectStatusDto input);

        Task CreateInvoiceRequest(InvoiceRequestDto input);
        //Delete Invoice Request
        Task DeleteInvoiceRequest(EntityDto input);
        ListResultDto<ListInvoiceRequestDto> GetInvoiceRequest(int projectid);
        PagedResultDto<ListInvoiceRequestDto> GetInvoicerequestListByPrject(GetInvoiceInput input);
        PagedResultDto<ListInvoiceRequestDto> GetInvoicerequestListByService(GetInvoiceInput input);
        //List<StatusddlList> GetStatusList();
        Task UpdateProjectPriority(UpdateProjectStatusDto input);
        Task Updatetocancelservice(int id);
        Task Enabledisable(int id);

        List<ProjectDto> GetAllProjectListData(ImportGetProjectDto input);
        List<ProjectDto> GetActiveProjectListData(ImportGetProjectDto input);
        List<ProjectDto> GetAmcProjectListAMC(ImportGetProjectDto input);
        List<ProjectDto> GetOnGoingProjectListOnGoing(ImportGetProjectDto input);
        List<ProjectDto> GetInvoiceCollectionProjectListINVOICE(ImportGetProjectDto input);
    }
}
