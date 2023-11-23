using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.SmsSender
{
    public class TwilioSmsSenderConfigurationAppService : ITransientDependency
    {
        public string AccountSid => ConfigurationManager.AppSettings["AccountSid"];
        public string AuthToken => ConfigurationManager.AppSettings["AuthToken"];
        public string SenderNumber => ConfigurationManager.AppSettings["SenderNumber"];
    }
}
