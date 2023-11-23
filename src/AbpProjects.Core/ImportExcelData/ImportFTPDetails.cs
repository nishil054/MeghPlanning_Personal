using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportExcelData
{
    [Table("ImportFTPDetails")]
    public class ImportFTPDetails : FullAuditedEntity
    {
        public virtual string DomainName { get; set; }
        public virtual string HostName { get; set; }
        public virtual string FTPUserName { get; set; }
        public virtual string FTPPassword { get; set; }
        public virtual string DBType { get; set; }
        public virtual string OnlineManager { get; set; }
        public virtual string OnlineManagerHostName { get; set; }
        public virtual string DatabaseName { get; set; }
        public virtual string DataBaseUserName { get; set; }
        public virtual string DataBasePassword { get; set; }
        public virtual string Storagecontainer { get; set; }
        public virtual string MailProvider_Host { get; set; }
        public virtual string MailProvider_User { get; set; }
        public virtual string MailProvider_Password { get; set; }

        public virtual string HostingProvider { get; set; }

    }
}
