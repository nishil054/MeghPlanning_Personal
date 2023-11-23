using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.Project.Dto;
using AbpProjects.Reports.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports
{
  public interface IReportsAppService : IApplicationService
    {
        List<AllUserDto> GetUser();
        List<MonthDto> GetMonth();
        List<YearDto> GetYear();
        List<Reports.Dto.ProjectDto> GetProjects(GetInputDto input);
        List<SalesReport> GetSalesReport(int year);
        List<SalesReport> GetSalesReport_Service(int year);
        List<SalesReport> GetCollectionReport(int year);
        List<SalesReport> GetInvoiceReport(int year);
        List<SalesReport> GetProductionReport(int year);
        List<GSTData> GetGSTDataReport(int year);
        PagedResultDto<OutStandingInvoice> GetOutStandingInvoice(OutStandingInvoice mOutStandingInvoice);
        PagedResultDto<OutStandingInvoice> GetOutStandingClient(OutStandingInvoice mOutStandingInvoice);

        DataTable GetAllData(GetInputDto input);
        List<StatusDto> GetStatus();
        List<StatsAmountDto> ExportProjectStateAmount(GetInputDto input);
        PagedResultDto<StatsAmountDto> GetProjectStatsAmount(GetInputDto input);
        List<ProjectWiseTimesheetDto> GetProjectWiseTimesheetReport(GetInputDto input);

        //DataTable ExportReportToExcel(GetInputDto input);
        PagedResultDto<LoginLogoutReportDto> GetLoginLogoutReportData(InputLoginLogoutReportDto input);
        List<GetYearDto> GetUniqueYear();
        List<GetMonth> GetUniqueMonth();
        PagedResultDto<EmployeeInOutReportDetailsDto> GetInOutDetailsReportData(EmployeeInOutDetails input);
        List<ExpenseReportDto> GetExpenseEntryReport(int year);
        List<subcategorydetaillist> GetExpenseEntryReportTotal(int year);
        List<OutStandingInvoice> ExportToExcelGetOutStandingClient(ImportOutStandingInvoiceDto input);
        string GetFinYear();
        ListResultDto<GetDailyActivityReportDto> GetDailyActivityReport(GetDailyActivityReportInputDto input);

    }
}
