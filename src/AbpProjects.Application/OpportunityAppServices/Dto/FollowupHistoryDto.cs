using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.OpportunityAppServices.Dto
{
    public class FollowupHistoryDto
    {
        public virtual int Id { get; set; }
        public virtual int Opporutnityid { get; set; }
        public List<ProjectName> projectNames { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual string CalllCategoryName { get; set; }
        public virtual string Comment { get; set; }
        public virtual DateTime? NextActionDate { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string PersonName { get; set; }
        public virtual DateTime? ClosingDate { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual int MasterOpporutnityid { get; set; }
        public virtual int AssignUserId { get; set; }
        public virtual int FollowUpTypeId { get; set; }
        public virtual string FollowUpType { get; set; }
        
    }
    public class ProjectName
    {
        public string Name { get; set; }
    }
}
