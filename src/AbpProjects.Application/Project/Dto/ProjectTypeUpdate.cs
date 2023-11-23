using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    [AutoMapTo(typeof(Projecttype_details))]
    public class ProjectTypeUpdate:EntityDto
    {
        public virtual int ProjecttypeId { get; set; }
        public virtual string ProjectType { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string hours { get; set; }
        public virtual bool? IsOutSource { get; set; }
        public virtual decimal? CostforCompany { get; set; }
        public virtual string Comments { get; set; }
    }
}
