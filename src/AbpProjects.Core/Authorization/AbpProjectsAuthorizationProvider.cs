using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace AbpProjects.Authorization
{

    public class AbpProjectsAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var adminuser = L("admin");
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"), description: adminuser);
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

           

            var User = context.CreatePermission(PermissionNames.Pages_Users, L("Users"), description: adminuser);
            //User.CreateChildPermission(PermissionNames.Pages_Userschild, L("userchild"));

            //Document Permission

            var document = context.CreatePermission(PermissionNames.Pages_Documents, L("Documents"));
            //document.CreateChildPermission(PermissionNames.Pages_Admin_Documents, L("Admin_Documents"));
            document.CreateChildPermission(PermissionNames.Pages_Employee_Documents, L("Employee_Documents"));

            //VPS Permission
            var vps = context.CreatePermission(PermissionNames.Pages_VPS, L("VPS"));
            vps.CreateChildPermission(PermissionNames.Pages_Employee_VPS, L("Employee_VPS"));
            vps.CreateChildPermission(PermissionNames.Pages_Support_VPS, L("Support_VPS"));

            //TimeSheet Permission
            var timesheet = context.CreatePermission(PermissionNames.Pages_TimeSheet, L("TimeSheet"));
            timesheet.CreateChildPermission(PermissionNames.Pages_Employee_MissingTimeSheet, L("Employee_MissingTimeSheet"));


            //invoice request Permission
            var invoice = context.CreatePermission(PermissionNames.Pages_Invoice_Request, L("InvoiceRequest"));
            invoice.CreateChildPermission(PermissionNames.Pages_Invoice_Request_Project, L("InvoiceRequestProject"));
            invoice.CreateChildPermission(PermissionNames.Pages_Invoice_Request_Service, L("InvoiceRequestService"));
            //Opportunity

            var opportunity = context.CreatePermission(PermissionNames.Pages_Opportunity, L("Opportunity"));
            opportunity.CreateChildPermission(PermissionNames.Pages_Opportunity_Admin, L("Opportunity_Admin"));
            opportunity.CreateChildPermission(PermissionNames.Pages_Opportunity_User, L("Opportunity_Users"));
            opportunity.CreateChildPermission(PermissionNames.Pages_GeneralOpportunity, L("GeneralOpportunity"));
            opportunity.CreateChildPermission(PermissionNames.Pages_MyOpportunity, L("MyOpportunity"));
            opportunity.CreateChildPermission(PermissionNames.Page_Opportunity_Leader, L("OpportunityLeader"));
            opportunity.CreateChildPermission(PermissionNames.Page_Sales_Import, L("SalesImport"));

            context.CreatePermission(PermissionNames.Pages_Opportunity_Report, L("Opportunity_Report"));
            //Project Permission
            var projectPer = context.CreatePermission(PermissionNames.Pages_Project, L("Project"));
            projectPer.CreateChildPermission(PermissionNames.Pages_Project_TimeSheetEnableDisable, L("projectEnableDisable"));
            projectPer.CreateChildPermission(PermissionNames.Pages_Project_Admin, L("Project_Admin"));
            projectPer.CreateChildPermission(PermissionNames.Pages_Project_Employee, L("Project_Employee"));
            projectPer.CreateChildPermission(PermissionNames.Pages_Projects_Generate_Invoice_Request, L("Projects_Generate_Invoice_Request"));
            projectPer.CreateChildPermission(PermissionNames.Pages_UserStory, L("UserStory"));
            projectPer.CreateChildPermission(PermissionNames.Pages_Update_Project_Priority, L("UpdatePriority"));
            projectPer.CreateChildPermission(PermissionNames.Pages_ActiveProjects, L("ActiveProjects"));
            projectPer.CreateChildPermission(PermissionNames.Pages_AMCProjects, L("AMCProjects"));
            projectPer.CreateChildPermission(PermissionNames.Pages_OnGoingProjects, L("OnGoingProjects"));
            projectPer.CreateChildPermission(PermissionNames.Pages_InvoiceCollectionProjects, L("InvoiceCollectionProjects"));
            projectPer.CreateChildPermission(PermissionNames.Pages_CompletedProjects, L("CompletedProjects"));
            projectPer.CreateChildPermission(PermissionNames.Pages_OnHoldProjects, L("OnHoldProjects"));
            projectPer.CreateChildPermission(PermissionNames.Pages_AllProjects, L("allProjects"));
            //Projects Without Client
            projectPer.CreateChildPermission(PermissionNames.Pages_ProjectsWithoutClient, L("ProjectsWithoutClient"));
            //Type Column Permission in Project for Marketing Leader
            projectPer.CreateChildPermission(PermissionNames.Pages_Project_Type, L("Type"));
            //Permission for Marketing Person Permission
            projectPer.CreateChildPermission(PermissionNames.Pages_Project_Marketing, L("Project_Marketing"));

            context.CreatePermission(PermissionNames.Pages_Clients, L("Clients"));
            // DataVault Permission
            var DataVault = context.CreatePermission(PermissionNames.Pages_DataVault, L("Datavault"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_Company, L("DataVault_Company"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_ProjectType, L("DataVault_ProjectType"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_WorkType, L("DataVault_WorkType"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_TypeName, L("DataVault_TypeName"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_Category, L("DataVault_Category"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_Admin_Leaves, L("AdminLeaves"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_Admin_Holiday, L("Admin_Holiday"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_Admin_Documents, L("Admin_Documents"));
            DataVault.CreateChildPermission(PermissionNames.Pages_DataVault_Financialyear, L("Financialyear"));


            //Employee renewal reminderlist for admin user login
            //context.CreatePermission(PermissionNames.Section_Renewal, L("Admin_Renewal"));

            //Manageservice permission
            //context.CreatePermission(PermissionNames.Pages_SupportManageService, L("SupportManageService"));


            //Holiday Permission
            var Holiday = context.CreatePermission(PermissionNames.Pages_Holiday, L("Holiday"));
            Holiday.CreateChildPermission(PermissionNames.Pages_Employee_Holiday, L("Employee_Holiday"));
            //Holiday.CreateChildPermission(PermissionNames.Pages_Admin_Holiday, L("Admin_Holiday"));

            //Import Permission
            var FTPImport = context.CreatePermission(PermissionNames.Pages_Import, L("FTPImport"));
            FTPImport.CreateChildPermission(PermissionNames.Pages_ImportExcel, L("FTPImportExcel"));
            FTPImport.CreateChildPermission(PermissionNames.Pages_ImportList, L("FTPDetails"));

            //Manage Leaves Permission
            //context.CreatePermission(PermissionNames.Pages_Admin_Leaves, L("AdminLeaves"));
            //context.CreatePermission(PermissionNames.Pages_Employee_Leaves, L("EmployeeLeaves"));

            //Knowledge Center Permission
            var KnowledgeCenter = context.CreatePermission(PermissionNames.Pages_KnowledgeCenter, L("KnowledgeCenter"));
            KnowledgeCenter.CreateChildPermission(PermissionNames.Pages_KnowledgeCenterCrud, L("KnowledgeCenterCrud"));
            KnowledgeCenter.CreateChildPermission(PermissionNames.Pages_KnowledgeCenterList, L("KnowledgeCenterList"));

            //Missing timesheet count employeewise for supervisor login
            //context.CreatePermission(PermissionNames.Pages_MissingTimesheetEmployee_Count, L("MissingEmployeeTimesheet_Count"));

            //Missing timesheet datewise for current user login
            //context.CreatePermission(PermissionNames.Pages_MissingTimesheet_Datewise, L("MissingEmployeeTimesheet_Datewise"));

            //Employeewise User Story Report
            context.CreatePermission(PermissionNames.Pages_Employeewise_UserStory, L("Pages_Employeewise_UserStory"));
            //Reports Permission
            var Reports = context.CreatePermission(PermissionNames.Pages_Reports, L("Reports"));
            Reports.CreateChildPermission(PermissionNames.Pages_Reports_User_Timesheet, L("Reports_User_Timesheet"));
            Reports.CreateChildPermission(PermissionNames.Pages_Reports_ProjectWiseTimesheet, L("Reports_ProjectWiseTimesheet"));
            Reports.CreateChildPermission(PermissionNames.Pages_Project_Report, L("Pages_Project_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_Service_Report, L("Pages_Service_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_Reports_InvoiceReport, L("Pages_Reports_InvoiceReport"));
            Reports.CreateChildPermission(PermissionNames.Pages_Reports_ProductionReport, L("Pages_Reports_ProductionReport"));

            Reports.CreateChildPermission(PermissionNames.Pages_ProjectStats_Hour_Report, L("Pages_ProjectStats_Hour_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_ProjectStats_Amount_Report, L("Pages_ProjectStats_Amount_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_Reports_Sales, L("Pages_Reports_Sales"));
            Reports.CreateChildPermission(PermissionNames.Pages_UserStory_Report, L("UserStoryReports"));
            Reports.CreateChildPermission(PermissionNames.Pages_Reports_CollectionReport, L("Pages_Reports_CollectionReport"));
            Reports.CreateChildPermission(PermissionNames.Pages_OutstandingInvoice_Report, L("Pages_OutstandingInvoice_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_OutstandingClient_Report, L("Pages_OutstandingClient_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_GSTData_Report, L("Pages_GSTData_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_Report_EmployeeInOutReport, L("EmployeeInOutReport"));
            Reports.CreateChildPermission(PermissionNames.Pages_Reports_Expense, L("ExpenseReport"));
            Reports.CreateChildPermission(PermissionNames.Pages_LeaveApplicationReport, L("LeaveApplicationReport"));
            Reports.CreateChildPermission(PermissionNames.Pages_DailySalesActivityReport, L("DailySalesActivityReport"));
            Reports.CreateChildPermission(PermissionNames.Pages_Opportunity_Closing_Report, L("Opportunity_Closing_Report"));
            Reports.CreateChildPermission(PermissionNames.Pages_DailyActivityReport, L("DailyActivityReport"));
            //UserStory Permission

            //UserStoryReports Permission
            //context.CreatePermission(PermissionNames.Pages_UserStory_Report, L("UserStoryReports"));
            //UserStoryReports Permission
            //context.CreatePermission(PermissionNames.Pages_UserStoryList, L("UserStoryList"));
            //Last 7Days Sales
            //context.CreatePermission(PermissionNames.Pages_Last_SevenDays_sales, L("LastSevenDaysSales"));
            //Monthly sales
            //context.CreatePermission(PermissionNames.Pages_Monthly_sales, L("MonthlySales"));
            //ProjectStats Hours
            //context.CreatePermission(PermissionNames.Pages_ProjectStats_Hour, L("ProjectStatsHours"));
            //ProjectStats Amount
            //context.CreatePermission(PermissionNames.Pages_ProjectStats_Amount, L("ProjectStatsAmount"));
            //Dashboard Permission
            var Dashboard = context.CreatePermission(PermissionNames.Pages_Dashboard, L("Dashboard"));
            //ProjectStats Hours
            Dashboard.CreateChildPermission(PermissionNames.Pages_ProjectStats_Hour, L("ProjectStatsHours"));
            //ProjectStats Amount
            Dashboard.CreateChildPermission(PermissionNames.Pages_ProjectStats_Amount, L("ProjectStatsAmount"));
            //Monthly sales
            Dashboard.CreateChildPermission(PermissionNames.Pages_Monthly_sales, L("MonthlySales"));
            //Last 7Days Sales
            Dashboard.CreateChildPermission(PermissionNames.Pages_Last_SevenDays_sales, L("LastSevenDaysSales"));
            //Manageservice permission
            Dashboard.CreateChildPermission(PermissionNames.Pages_SupportManageService, L("SupportManageService"));
            //Missing timesheet count employeewise for supervisor login
            Dashboard.CreateChildPermission(PermissionNames.Pages_MissingTimesheetEmployee_Count, L("MissingEmployeeTimesheet_Count"));
            //Userstory assign to employee show in particular user login
            Dashboard.CreateChildPermission(PermissionNames.Pages_AssignToUserstory_Employee, L("Pages_AssignToUserstory_Employee"));
            //Missing timesheet datewise for current user login
            Dashboard.CreateChildPermission(PermissionNames.Pages_MissingTimesheet_Datewise, L("MissingEmployeeTimesheet_Datewise"));
            //Employee renewal reminderlist for admin user login
            Dashboard.CreateChildPermission(PermissionNames.Section_Renewal, L("Admin_Renewal"));
            Dashboard.CreateChildPermission(PermissionNames.Pages_SalesTargetChart, L("SalesTargetChart_Dashboard"));
            Dashboard.CreateChildPermission(PermissionNames.Pages_ServiceWithoutClient, L("ServiceWithoutClient"));
            //total outstanding
            Dashboard.CreateChildPermission(PermissionNames.Pages_TotalOutstanding, L("TotalOutstanding"));
            //GSTDashboard Permission
            context.CreatePermission(PermissionNames.Pages_GSTDashboard, L("GST_Dashboard"));
            //Expense Category
            var ExpenseCategoryPermission = context.CreatePermission(PermissionNames.Page_Expense, L("Page_Expense"));
            ExpenseCategoryPermission.CreateChildPermission(PermissionNames.Page_Expense_Category, L("Page_Expense_Category"));
            ExpenseCategoryPermission.CreateChildPermission(PermissionNames.Page_Expense_SubCategory, L("Page_Expense_SubCategory"));
            ExpenseCategoryPermission.CreateChildPermission(PermissionNames.Page_Expense_Entry, L("Page_Expense_Entry"));
            // context.CreatePermission(PermissionNames.Page_Expense_Category, L("Page_Expense_Category"));
            //Support
            var SupportPermission = context.CreatePermission(PermissionNames.Page_Support, L("Support"));
            SupportPermission.CreateChildPermission(PermissionNames.Page_Support_Admin, L("Support_Admin"));
            SupportPermission.CreateChildPermission(PermissionNames.Page_Support_Employee, L("Support_Employee"));
            SupportPermission.CreateChildPermission(PermissionNames.Pages_Service_Generate_Invoice_Request, L("Service_Generate_Invoice_Request"));
            var AuditLogs = context.CreatePermission(PermissionNames.Pages_Administration_AuditLogs, L("AuditLogs"));
            

            //Settings
            context.CreatePermission(PermissionNames.Pages_Settings, L("Settings"));
            context.CreatePermission(PermissionNames.Pages_ManageCategory, L("ManageCategory"));

            //Utility
            context.CreatePermission(PermissionNames.Pages_Utility, L("Utility"));

            //Notification
            context.CreatePermission(PermissionNames.Pages_Notification, L("Notification"));

            //Hangfire Dashboard Permission
            context.CreatePermission(PermissionNames.Pages_Admin_HangfireDashboard, L("HangfireDashboard"));

            // Leave Application Permission
            var LeaveApplicationPermission = context.CreatePermission(PermissionNames.Pages_LeaveApplication, L("LeaveApplication"));
            LeaveApplicationPermission.CreateChildPermission(PermissionNames.Pages_Admin_LeaveApplication, L("LeaveApplication_Admin"));
            LeaveApplicationPermission.CreateChildPermission(PermissionNames.Pages_User_LeaveApplication, L("LeaveApplication_User"));

            var LeaveApplicationToDoPermission = context.CreatePermission(PermissionNames.Pages_LeaveApplicationToDo, L("LeaveApplicationToDo"));
            LeaveApplicationToDoPermission.CreateChildPermission(PermissionNames.Pages_Admin_LeaveApplicationToDo, L("LeaveApplicationToDo_Admin"));
            LeaveApplicationToDoPermission.CreateChildPermission(PermissionNames.Pages_User_LeaveApplicationToDo, L("LeaveApplicationToDo_User"));


            //var LeaveApplicationReportPermission = context.CreatePermission(PermissionNames.Pages_LeaveApplicationReport, L("LeaveApplicationReport"));
            //LeaveApplicationReportPermission.CreateChildPermission(PermissionNames.Pages_Admin_LeaveApplicationReport, L("LeaveApplicationReport_Admin"));
            //LeaveApplicationReportPermission.CreateChildPermission(PermissionNames.Pages_User_LeaveApplicationReport, L("LeaveApplicationReport_User"));
        }


        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpProjectsConsts.LocalizationSourceName);
        }
    }
}
