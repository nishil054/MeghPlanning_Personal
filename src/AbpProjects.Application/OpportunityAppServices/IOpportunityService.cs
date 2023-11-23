using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.BulkData.Dto;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.Project.Dto;
using AbpProjects.TimeSheet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.OpportunityAppServices
{
    public interface IOpportunityService: IApplicationService
    {
        Task<PagedResultDto<GetOpportunityDto>> MyOpportunityList(GetOpportunityInputDto input);
        Task<PagedResultDto<GetOpportunityDto>> GeneralOpportunityList(GetOpportunityInputDto input);
        Task<PagedResultDto<GetOpportunityDto>> GetOpportunityList(GetOpportunityInputDto input);
        Task<ListResultDto<CallCategoryDto>> GetCallCategory();
        Task<ListResultDto<CallCategoryDto>> GetCallCategoryInq();
        Task<ListResultDto<CallCategoryDto>> GetCallCategoryGen();
        Task<ListResultDto<CallCategoryDto>> GetCallCategoryOpportunity();
        Task<ListResultDto<CallCategoryDto>> GetCallCategoryMyOpp();
        Task<ListResultDto<GetProjectTypeDto>> GetProjectType();
        Task<ListResultDto<GetMarketingUserDto>> GetMarketingUsers();
        Task OpportunityCreate(createDto input);
        Task<GetOpportunityDto> GetOpportunityEdit(EntityDto input);
        Task UpdateOpportunity(OpportunityUpdateDto input);
        Task<PagedResultDto<GetOpportunityReportDto>> GetOpportunityReport(GetOpportunityInputDto input);
        List<GetOpportunityReportDto> OpportunityReportExport(GetOpportunityInputDto input);
        PagedResultDto<GetDailySalesActivityReportDto> DailySalesActivityReport(GetOpportunityInputDto input);
        List<GetDailySalesActivityReportDto> DailySalesActivityReportExport(GetOpportunityExportInputDto input);
        Task<GetOpportunityDto> GetOpportunityDetails(EntityDto input);
        Task<GetFollowUpDetailDto> GetFollowUpDetail(EntityDto input);
        Task UpdateFollowUp(UpdateFollowUpDto input);
        ListResultDto<FollowupHistoryDto> FollowUpHistoryData(int Opporutnityid = 0);
        Task<ProjectTypeDto> GetProjectTypesDetails(EntityDto input);
        Task UpdateFollowUpClosed(EntityDto input);
        PagedResultDto<UserDto> GetUserMarketingLead();
        Task SaveBulkDataInDB(InsertOpportunityBulkData input);
        Task ConvertToOpportunity(EntityDto input);
        Task AssignUser(AssignUser input);
        ListResultDto<GetFollowUpCountDto> GetfollowCountLists(GetFollowUpFilterDto inputs);
        Task<PagedResultDto<GetOpportunityReportDto>> GetOpportunityClosingReport(GetOpportunityInputDto input);
        List<GetOpportunityReportDto> GetOpportunityClosingReportExport(GetOpportunityInputDto input);
        bool MobileOrEmailExsistence(GetOpportunityDto input);
        bool MobileOrEmailExsistenceById(GetOpportunityDto input);
    }
}
