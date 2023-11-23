using Abp.Authorization;
using AbpProjects.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Email
{
    [AbpAllowAnonymous]
    public class EmailAppService : AbpProjectsApplicationModule, IEmailAppService
    {
        private readonly IUserEmailer _userEmailer;


        public EmailAppService(
          IUserEmailer userEmailer
          )
        {
            _userEmailer = userEmailer;

        }
        public async Task sendmail()
        {
            await _userEmailer.SendMailtouser("sneha.patel@meghtechnologies.com");
        }
    }
}
