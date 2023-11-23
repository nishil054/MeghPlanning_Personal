using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace AbpProjects.Authorization.Roles
{
    public static class AppRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            //Static host roles for Support
            roleManagementConfig.StaticRoles.Add(
        new StaticRoleDefinition(
            StaticRoleNames.Tenants.Support,
            MultiTenancySides.Tenant
           )
        {
            GrantedPermissions = {
                "Pages.Holiday",
                    "Pages.Documents",
                    "Pages.SupportManageService",
                    "Pages.Support.VPS",
                    "Pages.VPS",
                    "Pages.Employee.Holiday",
                    "Pages.Employee.Documents"
                }
        }
        );
            //Static host roles for Employee
            roleManagementConfig.StaticRoles.Add(
                    new StaticRoleDefinition(
                        StaticRoleNames.Tenants.Employee,
                        MultiTenancySides.Tenant
                       )
                    {
                        GrantedPermissions = {
                    "Pages.Employee.Documents",
                    "Pages_MissingTimesheet_Datewise",
                    "Pages.Employee.Holiday",
                    "Pages.TimeSheet"
                            }
                    }
                    );


            roleManagementConfig.StaticRoles.Add(
                   new StaticRoleDefinition(
                       StaticRoleNames.Tenants.HR,
                       MultiTenancySides.Tenant
                      )
                   {
                       GrantedPermissions = {
                    "Pages.SupportManageService",
                       "Pages.TimeSheet"
                       ,
                       //"Pages.Admin.Holiday",
                       "Pages.DataVault.Admin.Holiday",
                       "Pages.Employee.Documents",
                       //"Pages.Admin.Leaves",
                       "Pages.DataVault.Admin.Leaves",
                       "Pages.Project"

                           }
                   }
                   );

            roleManagementConfig.StaticRoles.Add(
                   new StaticRoleDefinition(
                       StaticRoleNames.Tenants.Marketing_Leader,
                       MultiTenancySides.Tenant
                      )
                   {
                       GrantedPermissions = {

                       "Pages.TimeSheet",
                       "Pages.Employee.Holiday",
                        "Pages.Employee.Documents",
                        "Pages.Project"
                           }
                   }
                   );

            roleManagementConfig.StaticRoles.Add(
                  new StaticRoleDefinition(
                      StaticRoleNames.Tenants.Supervisor,
                      MultiTenancySides.Tenant
                     )
                  {
                      GrantedPermissions = {

                       "Pages.TimeSheet",
                       "Pages_MissingTimesheetEmployee_Count",
                       "Pages_MissingTimesheet_Datewise",
                       "Pages.Employee.Holiday",
                        "Pages.Employee.Documents"
                          }
                  }
                  );



            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Admin,
                    MultiTenancySides.Host)
                {
                    GrantedPermissions =
                    {
                        "Section_Renewal",
                        "Pages_MissingTimesheetEmployee_Count",
                        "Pages_MissingTimesheet_Datewise",
                        "Pages.TimeSheet",
                        "Pages.Documents",
                       //"Pages.Admin.Leaves",
                       "Pages.DataVault.Admin.Leaves",
                       //"Pages.Admin.Documents",
                       //"Pages.Project",
                       //"Pages.Admin.VPS",
                       //"Pages.Holiday",
                       //"Pages.Admin.Holiday"

                    }

                });
        }
    }
}
