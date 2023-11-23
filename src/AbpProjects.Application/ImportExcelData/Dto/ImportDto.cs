using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportExcelData.Dto
{
    [AutoMapFrom(typeof(ImportFTPDetails))]
    public class ImportDto : EntityDto
    {
        
        public virtual string DomainName { get; set; }
        public virtual string HostName { get; set; }
        public virtual string FtpUserName { get; set; }
        public virtual string FtpPassword { get; set; }
        public virtual string DbType { get; set; }
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
