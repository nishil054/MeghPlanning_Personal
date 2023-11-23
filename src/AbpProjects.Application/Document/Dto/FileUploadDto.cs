using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.FileUploadByDirective;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Document.Dto
{
    [AutoMapTo(typeof(Documentchild))]
    public class FileUploadDto : EntityDto
    {
        public virtual int DocumentId { get; set; }
        public virtual List<string> FileName { get; set; }
    }
}
