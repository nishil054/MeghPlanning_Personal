using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.OpportunityAppServices.Dto
{
    public class UpdateFollowUpDto
    {
        public virtual int opporutnityid { get; set; }
        public virtual DateTime? nextactiondate { get; set; }
        public virtual DateTime? expectedclosingdate { get; set; }
        public virtual string Comment { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual int AssignUserId { get; set; }
        public string[] ProjectType { get; set; }
        public virtual decimal ProjectValue { get; set; }
        public virtual int FollowuptypeId { get; set; }
    }
    public class AssignUser:EntityDto
    {
        public int AssignUserId { get; set; }
    }
}
