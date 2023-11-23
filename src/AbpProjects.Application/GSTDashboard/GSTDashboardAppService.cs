using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AbpProjects.Authorization;
using AbpProjects.Bills;
using AbpProjects.Company;
using AbpProjects.Company.Dto;
using AbpProjects.FinancialYear;
using AbpProjects.gstdashboard;
using AbpProjects.GSTDashboard.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.GSTDashboard
{
    [AbpAuthorize(PermissionNames.Pages_GSTDashboard)]
    public class GSTDashboardAppService : AbpProjectsApplicationModule, IGSTDashboardAppService
    {
        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<financialYear> _financialRepository;
        private readonly IRepository<gstDashboard> _gstdataRepository;
        private readonly IRepository<company> _companyRepository;
        public GSTDashboardAppService(IRepository<Bill> billRepository, IRepository<financialYear> financialRepository, IRepository<gstDashboard> gstdataRepository,IRepository<company> companyRepository)
        {
            _billRepository = billRepository;
            _financialRepository = financialRepository;
            _gstdataRepository = gstdataRepository;
            _companyRepository = companyRepository;
        }

        public async Task CreateService(CreateGstDataDto input)
        {
            //var data = _financialRepository.GetAll().Where(x => x.Id == input.FinancialyearId).Select(X=>X.StartYear).FirstOrDefault();
            //var endyear = _financialRepository.GetAll().Where(x => x.Id == input.FinancialyearId).Select(X => X.EndYear).FirstOrDefault();
            var gstdata = input.MapTo<gstDashboard>();
            
            gstdata.Month = input.MonthName;
            gstdata.OutputGST = input.OutputGST;
            gstdata.TotalPayableGST = input.TotalPayableGST;
            gstdata.TotalPendingPayment = input.TotalPendingPayment;
            await _gstdataRepository.InsertAndGetIdAsync((gstDashboard)gstdata);


        }

        public async Task UpdateStatuslist(UpdateStatus input)
        {
            if (input.UpdateStatusId != null)
            {
               
                var data = _gstdataRepository.Get(input.Id);
                data.Status = input.UpdateStatusId;
                if (data.Status==1)
                {
                    data.TotalPendingPayment = 0;
                }
                else
                {
                    data.Status = input.UpdateStatusId;
                    

                }
                _gstdataRepository.Update(data);
               
            }
        }

        public List<GetGstDataListDto> GetGstDataList(GetGstDataDto input)
        {
            try
            {
                var data = _financialRepository.GetAll().Where(x => x.Id == input.FinancialyearId).FirstOrDefault();
                var billrepo = _billRepository.GetAll();
                var result = (from a in _gstdataRepository.GetAll()

                              select new GetGstDataListDto
                              {
                                  Id = a.Id,
                                  Mid = a.CreationTime.Month,
                                  Month = a.Month,
                                  MonthName = "",
                                  OutputGST = a.OutputGST,
                                  InputGST = a.InputGST,
                                  TotalPayableGST = a.TotalPayableGST,
                                  TotalPendingPayment = a.TotalPendingPayment,
                                  Status = a.Status,
                                  CompanyId = a.CompanyId,
                                  FinancialyearId = a.FinancialyearId,
                                  CreationTime = a.CreationTime,
                                  MonthId = a.MonthId,
                                  InvoiceDisable=false,
                                  EndYear=data.EndYear

                              })
                              .OrderBy(x => x.Id)
                              .Where(x=>x.CompanyId==input.CompanyId && x.FinancialyearId==input.FinancialyearId)
                              //.Where(x => (x.CreationTime.Year == data.StartYear && x.CreationTime.Month >= 4 && x.CompanyId == input.CompanyId) || (x.CreationTime.Year == data.EndYear && x.CreationTime.Month <= 3 && x.CompanyId == input.CompanyId))
                              .ToList();

                foreach (var item in result)
                {
                   
                    item.MonthName = getMonthName((int)item.MonthId);
                   
                }

               
                return result;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public decimal GetSum(int? month, int? company, int? finacialyearid)
        {
            try
            {
                var data = _financialRepository.GetAll().Where(x => x.Id == finacialyearid).FirstOrDefault();
                var billrepo = _billRepository.GetAll();
                var totalgst = 0.0M;
                
                var Cgst = billrepo.Where(x => (x.companyid == company && x.BillDate.Value.Year == data.StartYear && x.BillDate.Value.Month == month) || (x.companyid == company && x.BillDate.Value.Year == data.EndYear && x.BillDate.Value.Month == month)).Select(x => x.cgst).Sum();
                
                var Sgst = billrepo.Where(x => (x.companyid == company && x.BillDate.Value.Year == data.StartYear && x.BillDate.Value.Month == month) || (x.companyid == company && x.BillDate.Value.Year == data.EndYear && x.BillDate.Value.Month == month)).Select(x => x.sgst).Sum();
               
                var Igst = billrepo.Where(x => (x.companyid == company && x.BillDate.Value.Year == data.StartYear && x.BillDate.Value.Month == month) || (x.companyid == company && x.BillDate.Value.Year == data.EndYear && x.BillDate.Value.Month == month)).Select(x => x.igst).Sum();
               
                totalgst = (decimal)((Cgst == null ? 0.0M : Cgst) + (Sgst == null ? 0.0M : Sgst ) + (Igst == null ? 0.0M : Igst)) ;

                return totalgst;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public int GetEndYear(int? finacialyearid)
        {
            try
            {
                var data = _financialRepository.GetAll().Where(x => x.Id == finacialyearid).FirstOrDefault();
                var endyr = 0;
                endyr = data.EndYear;
                return endyr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetStartYear(int? finacialyearid)
        {
            
            try
            {
                var data = _financialRepository.GetAll().Where(x => x.Id == finacialyearid).FirstOrDefault();
                var yr="";
                var startyr1 = 0;
                var endyr1 = 0;
                startyr1 = data.StartYear;
                endyr1= data.EndYear;

                return yr= startyr1+ "-"  + endyr1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private string getMonthName(int monthId)
        {
            List<GetMonthNameDto> MonthList = new List<GetMonthNameDto>();
            for (int i = 1; i <= 12; i++)
            {
                MonthList.Add(new GetMonthNameDto { Id = i, MonthName = getFullName(i) });
            }
            string monthname = "";
                var data = MonthList.Where(X => X.Id == monthId).FirstOrDefault();
            if (data != null)
            {
                monthname = data.MonthName;
            }
            return monthname;
        }

        public List<GetMonthNameDto> GetMonth()
        {
            List<GetMonthNameDto> MonthList = new List<GetMonthNameDto>();
            for (int i = 1; i <= 12; i++)
            {
                MonthList.Add(new GetMonthNameDto { Id = i, MonthName = getFullName(i) });
            }
            return MonthList;
        }

        static string getFullName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat
                .GetAbbreviatedMonthName(month);
        }

        public async Task<GetGstDataListDto> GetDataById(EntityDto input)
        {
            var c = (await _gstdataRepository.GetAsync(input.Id)).MapTo<GetGstDataListDto>();
            return c;
        }

        public async Task UpdateGstData(EditGstDataDto input)
        {
            var Tests = await _gstdataRepository.GetAsync(input.Id);
            
            Tests.Id = input.Id;
            Tests.FinancialyearId = input.FinancialyearId;
            Tests.CompanyId = input.CompanyId;
            Tests.Month = input.Month;
            Tests.MonthId = input.MonthId;
            Tests.OutputGST = input.OutputGST;
            Tests.InputGST = input.InputGST;
            Tests.TotalPayableGST = input.TotalPayableGST;
            Tests.TotalPendingPayment = input.TotalPendingPayment;
            Tests.Status = input.Status;
            await _gstdataRepository.UpdateAsync(Tests);
        }

        public async Task DeleteGstData(EntityDto input)
        {
            await _gstdataRepository.DeleteAsync(input.Id);
        }

        public List<CompanyDto> GetCompany()
        {
            var result = (from a in _companyRepository.GetAll()
                          .Where(a => a.CompanyId > 0)
                          select new CompanyDto
                          {
                              Id = a.Id,
                              CompanyId = a.CompanyId,
                              Beneficial_Company_Name = a.Beneficial_Company_Name,
                          }).OrderBy(x=>x.Beneficial_Company_Name).ToList();
            return result;
        }
    }
}
