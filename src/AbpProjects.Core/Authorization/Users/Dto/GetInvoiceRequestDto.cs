using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Authorization.Users.Dto
{
  public class GetInvoiceRequestDto
    {
        public virtual int UId { get; set; }
        public virtual string toEmail { get; set; }
        public virtual string FromEmail { get; set; }
        public virtual string EmailTitle { get; set; }
        public virtual string Emailsubtitle { get; set; }
        public virtual string EmailSubject { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string DomainName { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Comment { get; set; }
        public StringBuilder MailBody { get; set; }
    }
}
