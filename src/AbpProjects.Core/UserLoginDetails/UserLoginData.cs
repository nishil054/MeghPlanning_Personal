using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.UserLoginDetails
{
    [Table("tbl_UserLogIn_LogOut")]
    public class UserLoginData : FullAuditedEntity
    {
        public const int maxLength = 100;
        public string returndate;

        public virtual int UserId { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual DateTime LoggedIn { get; set; }
        public virtual DateTime? LoggedOut { get; set; }
        public virtual int? TimesheetComplete { get; set; }
    }
}
