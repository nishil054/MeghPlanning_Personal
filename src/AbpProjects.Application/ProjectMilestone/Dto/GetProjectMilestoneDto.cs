using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectMilestone.Dto
{
    public class GetProjectMilestoneDto: EntityDto
    {
        //public virtual int ProjectTypeId { get; set; }
        public virtual string Title { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual string Description { get; set; }
        public virtual int ProjectTypeId { get; set; }

        //public void Normalize()
        //{
        //    if (string.IsNullOrEmpty(Sorting))
        //    {
        //        Sorting = "Title";
        //    }
        //}

    }
}
