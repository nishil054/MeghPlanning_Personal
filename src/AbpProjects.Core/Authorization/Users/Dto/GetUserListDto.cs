using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Authorization.Users.Dto
{
   public class GetUserListDto
    {
        public virtual int UserId { get; set; }
        public virtual string toEmail { get; set; }
        public virtual string FromEmail { get; set; }
        public virtual string CC { get; set; }
        public virtual string EmailTitle { get; set; }
        public virtual string Emailsubtitle { get; set; }
        public virtual string EmailSubject { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FromDate { get; set; }
        public virtual string Reason { get; set; }

        public virtual string LeaveType { get; set; }
        public virtual string ToDate { get; set; }
        public StringBuilder MailBody { get; set; }
    }
}
