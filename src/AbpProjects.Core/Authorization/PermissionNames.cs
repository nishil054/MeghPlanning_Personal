namespace AbpProjects.Authorization
{
    public static class PermissionNames
    {
        public const string Pages_Tenants = "Pages.Tenants";

        public const string Pages_Users = "Pages.Users";

        //public const string Pages_Userschild = "Pages.Userschild";

        public const string Pages_Roles = "Pages.Roles";
       
        //Document Permission
        public const string Pages_Documents = "Pages.Documents";
        public const string Pages_Employee_Documents = "Pages.Employee.Documents";
        //public const string Pages_Admin_Documents = "Pages.Admin.Documents";

        //VPS Permission
        public const string Pages_VPS = "Pages.VPS";
        public const string Pages_Support_VPS = "Pages.Support.VPS";
        public const string Pages_Employee_VPS = "Pages.Employee.VPS";

        //TimeSheet Permission
        public const string Pages_TimeSheet = "Pages.TimeSheet";
        public const string Pages_Employee_MissingTimeSheet = "Pages.Employee.MissingTimeSheet";

        //DataVault Permission
        public const string Pages_DataVault = "Pages.DataVault";
        public const string Pages_DataVault_Company = "Pages.DataVault.Company";
        public const string Pages_DataVault_ProjectType = "Pages.DataVault.ProjectType";
        public const string Pages_DataVault_WorkType = "Pages.DataVault.WorkType";
        public const string Pages_DataVault_TypeName = "Pages.DataVault.TypeName";
        public const string Pages_DataVault_Category = "Pages.DataVault.Category";
        public const string Pages_DataVault_Financialyear = "Pages.DataVault.Financialyear";
        public const string Pages_DataVault_Admin_Leaves = "Pages.DataVault.Admin.Leaves";
        public const string Pages_DataVault_Admin_Holiday = "Pages.DataVault.Admin.Holiday";
        public const string Pages_DataVault_Admin_Documents = "Pages.DataVault.Admin.Documents";

        //Opportunity Permission
        public const string Pages_Opportunity = "Pages.Opportunity";
        public const string Pages_GeneralOpportunity = "Pages.GeneralOpportunity";
        public const string Pages_MyOpportunity = "Pages.MyOpportunity";
        public const string Pages_Opportunity_Admin = "Pages.Opportunity.Admin";
        public const string Pages_Opportunity_User = "Pages.Opportunity.User";
        public const string Pages_Opportunity_Report = "Pages.Opportunity.Report";
        public const string Page_Opportunity_Leader = "Pages.Opportunity_Leader";
        public const string Page_Sales_Import = "Pages.Sales_Import";
        public const string Pages_Opportunity_Closing_Report = "Pages.Opportunity.Closing_Report";

        //Project  Permission
        public const string Pages_Project = "Pages.Project";
        public const string Pages_ActiveProjects = "Pages.ActiveProjects";
        public const string Pages_AMCProjects = "Pages.AMCProjects";
        public const string Pages_OnGoingProjects = "Pages.OnGoingProjects";
        public const string Pages_InvoiceCollectionProjects = "Pages.InvoiceCollectionProjects";
        public const string Pages_CompletedProjects = "Pages.CompletedProjects";
        public const string Pages_OnHoldProjects = "Pages.OnHoldProjects";
        public const string Pages_AllProjects = "Pages.AllProjects";

        //Projects Without Client
        public const string Pages_ProjectsWithoutClient = "Pages.ProjectsWithoutClient";
        public const string Pages_Project_TimeSheetEnableDisable = "Pages.Project.TimeSheetEnableDisable";
        public const string Pages_Clients = "Pages.Clients";
        public const string Pages_Update_Project_Priority = "Pages.Update.Project.Priority";
        public const string Pages_Projects_Generate_Invoice_Request = "Projects.Generate.Invoice.Request";
        public const string Pages_Project_Admin = "Pages.Project.Admin";
        public const string Pages_Project_Employee = "Pages.Project.Employee";
        //Permission for Marketing Person Permission
        public const string Pages_Project_Marketing = "Pages.Project.Marketing";

        //Type Column Permission in Project for Marketing Leader
        public const string Pages_Project_Type = "Pages.Project.Type";

        //Holiday Permission
        public const string Pages_Holiday = "Pages.Holiday";
        public const string Pages_Employee_Holiday = "Pages.Employee.Holiday";
        //public const string Pages_Admin_Holiday = "Pages.Admin.Holiday";

        //Import Page Permission
        public const string Pages_Import = "Pages.Import";
        public const string Pages_ImportExcel = "Pages.ImportExcel";
        public const string Pages_ImportList = "Pages.ImportList";

        //InvoiceRequest Permission
        public const string Pages_Invoice_Request = "Pages.InvoiceRequest";
        public const string Pages_Invoice_Request_Project = "Pages.Project.InvoiceRequest";
        public const string Pages_Invoice_Request_Service = "Pages.Service.InvoiceRequest";

        //Manage Leaves Page Permission
        //public const string Pages_Admin_Leaves = "Pages.Admin.Leaves";
        //public const string Pages_Employee_Leaves = "Pages.Employee.Leaves";

        //Knowledge Center Page Permission
        public const string Pages_KnowledgeCenter = "Pages.KnowledgeCenter";
        public const string Pages_KnowledgeCenterCrud = "Pages.KnowledgeCenterCrud";
        public const string Pages_KnowledgeCenterList = "Pages.KnowledgeCenterList";

        //Reports Permission
        public const string Pages_Reports = "Pages.Reports";
        public const string Pages_Reports_User_Timesheet = "Pages.Reports.UserTimesheet";
        public const string Pages_Reports_ProjectWiseTimesheet = "Pages.Reports.ProjectWiseTimesheet";
        public const string Pages_Reports_Sales = "Pages.Reports.Sales";
        public const string Pages_Project_Report = "Pages.ProjectReport";
        public const string Pages_Service_Report = "Pages.Reports.Service";

        public const string Pages_Reports_InvoiceReport = "Pages.Reports.InvoiceReport";
        public const string Pages_Report_EmployeeInOutReport = "Pages.Reports.EmployeeInOutReport";

        public const string Pages_Reports_ProductionReport = "Pages.Reports.ProductionReport";
        public const string Pages_ProjectStats_Hour_Report = "Pages.ProjectStatsHour.Report";
        public const string Pages_ProjectStats_Amount_Report = "Pages.ProjectStatsAmount.Report";
        public const string Pages_UserStory_Report = "Pages.UserStoryReport";
        public const string Pages_OutstandingInvoice_Report = "Pages.OutstandingInvoice.Report";
        public const string Pages_OutstandingClient_Report = "Pages.OutstandingClient.Report";
        public const string Pages_GSTData_Report = "Pages.GSTData.Report";
        public const string Pages_Employeewise_UserStory = "Pages.Employeewise.UserStory";

        public const string Pages_Reports_Expense = "Pages.Reports.Expense";
        public const string Pages_LeaveApplicationReport = "Pages.Reports.LeaveApplicationReport";
        public const string Pages_DailySalesActivityReport = "Pages.Reports.DailySalesActivityReport";
        public const string Pages_DailyActivityReport = "Pages.Reports.DailyActivityReport";
        //Import Page Permission
        public const string Pages_UserStory = "Pages.UserStory";
        //public const string Pages_UserStoryList = "Pages.UserStoryList";


        //Dashboard Permission
        public const string Pages_Dashboard = "Pages.Dashboard";
        //Renewal reminder list for admin user
        public const string Section_Renewal = "Section.Renewal";
        //Last 7Days Sales
        public const string Pages_Last_SevenDays_sales = "Pages.SevenDays.Sales";
        //Monthly sales
        public const string Pages_Monthly_sales = "Pages.Monthly.Sales";
        //ProjectStats Hours
        public const string Pages_ProjectStats_Hour = "Pages.ProjectStats.Hour";
        //ProjectStats Amount
        public const string Pages_ProjectStats_Amount = "Pages.ProjectStats.Amount";
        //Missing timesheet count employeewise for supervisor login
        public const string Pages_MissingTimesheetEmployee_Count = "Pages.MissingTimesheetEmployee.Count";
        //Missing timesheet datewise for current user login
        public const string Pages_MissingTimesheet_Datewise = "Pages.MissingTimesheet.Datewise";
        //Manage service permission
        public const string Pages_SupportManageService = "Pages.SupportManageService";
        public const string Pages_Reports_CollectionReport = "Pages.Reports.CollectionReport";
        //Assigned Userstory show in dashboard employeewise login
        public const string Pages_AssignToUserstory_Employee = "Pages.AssignToUserstory.Employee";

        //GSTDashboard Permission

        public const string Pages_GSTDashboard = "Pages.GSTDashboard";
        public const string Pages_SalesTargetChart = "Pages.SalesTargetChart";
        public const string Pages_ServiceWithoutClient = "Pages.ServiceWithoutClient";

        // support permission
        public const string Page_Support = "Pages.Support";
        public const string Page_Support_Admin = "Pages.Support.Admin";
        public const string Page_Support_Employee = "Pages.Support.Employee";
        public const string Pages_Service_Generate_Invoice_Request = "Pages.Service.Generate.Invoice.Request";

        //expense Category
        public const string Page_Expense = "Pages.Expense";
        public const string Page_Expense_Category = "Pages.Expense.Category";
        public const string Page_Expense_SubCategory = "Pages.Expense.SubCategory";
        public const string Page_Expense_Entry = "Pages.Expense.Entry";

        //Total Outstanding
        
        public const string Pages_TotalOutstanding = "Pages.TotalOutstanding";


        //Settings
        public const string Pages_Settings = "Pages.Settings";

        public const string Pages_Administration_AuditLogs = "Pages.AdministrationAuditLogs";

        //Manage Category
        public const string Pages_ManageCategory = "Pages.ManageCategory";

        //Utility
        public const string Pages_Utility = "Pages.Utility";

        //Notification
        public const string Pages_Notification = "Pages.Notification";

        //Hangfire Dashboard Permission
        public const string Pages_Admin_HangfireDashboard = "Pages.Admin.HangfireDashboard";

        // Leave Application Permission
        public const string Pages_LeaveApplication = "Pages.LeaveApplication";
        public const string Pages_Admin_LeaveApplication = "Pages.Admin.LeaveApplication";
        public const string Pages_User_LeaveApplication = "Pages.User.LeaveApplication";
        public const string Pages_LeaveApplicationToDo = "Pages.LeaveApplicationToDo";
        public const string Pages_Admin_LeaveApplicationToDo = "Pages.Admin.LeaveApplicationToDo";
        public const string Pages_User_LeaveApplicationToDo = "Pages.User.LeaveApplicationToDo";

        

    }
}