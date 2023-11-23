using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Document.Dto
{
    [AutoMapFrom(typeof(document))]
    public  class EditDocumentDto : EntityDto
    {
        public virtual string Title { get; set; }
        public virtual string Attachment { get; set; }
    }
}
