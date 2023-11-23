using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using AbpProjects.Api;
using AbpProjects.BackgroundJob;
using AbpProjects.Email;
using Hangfire;

namespace AbpProjects.Web
{
    [DependsOn(
        typeof(AbpProjectsDataModule),
        typeof(AbpProjectsApplicationModule),
        typeof(AbpProjectsWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpHangfireModule), //- ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule))]
    public class AbpProjectsWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<AbpProjectsNavigationProvider>();

            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            Configuration.BackgroundJobs.UseHangfire(configuration =>
            {
                configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
             RecurringJob.RemoveIfExists("ProjectEffortsNotification");
             var effortsNotification = IocManager.Resolve<IBackgroundAppService>();
             RecurringJob.AddOrUpdate<IBackgroundAppService>("ProjectEffortsNotification", t => t.ProjectNotification(),"00 10 * * 1,5",TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));//1-Monday,5-Friday
        }
    }
}
