using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter.Dto
{
    [AutoMapFrom(typeof(KnowledgeDocuments))]
    public class GetKnowledgeDocumentsDto : EntityDto<long>
    {
        public int KnowledgeCenterId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string DocumentName { get; set; }
    }
}
