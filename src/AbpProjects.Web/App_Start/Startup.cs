using System;
using System.Configuration;
using Abp.Hangfire;
using Abp.Owin;
using AbpProjects.Api.Controllers;
using AbpProjects.Web;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using AbpProjects.Authorization;

[assembly: OwinStartup(typeof(Startup))]
namespace AbpProjects.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                // evaluate for Persistent cookies (IsPermanent == true). Defaults to 14 days when not set.
                ExpireTimeSpan = new TimeSpan(int.Parse(ConfigurationManager.AppSettings["AuthSession.ExpireTimeInDays.WhenPersistent"] ?? "14"), 0, 0, 0),
                SlidingExpiration = true //bool.Parse(ConfigurationManager.AppSettings["AuthSession.SlidingExpirationEnabled"] ?? bool.FalseString)
            }) ;

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.MapSignalR();

            //Enable it to use HangFire dashboard(uncomment only if it's enabled in TechnoFormsWebModule)
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AbpHangfireAuthorizationFilter(PermissionNames.Pages_Admin_HangfireDashboard) }
                //Authorization = new[] { new AbpHangfireAuthorizationFilter() }  //access hangfire dashboard  for all users
            });
        }
    }
}
