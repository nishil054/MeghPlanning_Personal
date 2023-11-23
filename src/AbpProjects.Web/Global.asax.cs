using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using Abp.WebApi.Validation;
using System.Threading;
using Abp.Timing;

namespace AbpProjects.Web
{
    public class MvcApplication : AbpWebApplication<AbpProjectsWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {

            //Use UTC clock. Remove this to use local time for your application.
            Clock.Provider = ClockProviders.Utc;



#if DEBUG
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.config"))
            );
#else
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.Production.config"))
            );
#endif
            base.Application_Start(sender, e);


        }
    }
}
