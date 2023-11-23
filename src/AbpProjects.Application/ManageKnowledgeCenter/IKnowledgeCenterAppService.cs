using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Category.Dto;
using AbpProjects.ManageKnowledgeCenter.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter
{
    public interface IKnowledgeCenterAppService : IApplicationService
    {
        //Task<PagedResultDto<GetKnowledgeCenterDto>> getKnowledgeCenter(GetKnowledgeCenterInput Input);
        PagedResultDto<GetKnowledgeCenterDto> getKnowledgeCenter(GetKnowledgeCenterInput Input);
        List<GetTeamListDto> GetTeams();
        Task<int> CreateKnowledgeCenter(CreateKnowledgeCenterDto input);
        bool KnowledgeCenterExsistence(CreateKnowledgeCenterDto input);
        Task<GetKnowledgeCenterDto> GetknowledgeCenterForEdit(EntityDto input);
        ListResultDto<GetKnowledgeDocumentsDto> getknowledgeCenterDocuments(EntityDto input);
        Task DeleteknowledgeCenterFiles(EntityDto input);
        bool KnowledgeCenterExsistenceById(EditKnowledgeCenterDto input);
        Task UpdateKnowledgeCenter(EditKnowledgeCenterDto input);
        Task<GetKnowledgeCenterDto> GetknowledgeCenterForDetail(EntityDto input);
        Task<GetKnowledgeDocumentsDto> GetknowledgeDocuments(EntityDto input);
        Task DeleteKnowledgeCenter(EntityDto input);
        List<CategoryDto> GetCategories();
    }
}
