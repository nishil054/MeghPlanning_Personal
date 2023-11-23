using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AbpProjects.Authorization;
using AbpProjects.FinancialYear.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace AbpProjects.FinancialYear
{      
    [AbpAuthorize(PermissionNames.Pages_DataVault, PermissionNames.Pages_DataVault_Financialyear)]
    public class FinancialYearAppService : AbpProjectsApplicationModule, IFinancialYearAppService
    {
        private readonly IRepository<financialYear> _financialRepository;
        public FinancialYearAppService(IRepository<financialYear> financialRepository)
        {
            _financialRepository = financialRepository;
        }
        public async Task CreateFinancialYear(CreateFinancialyearDto input)
        {
            var result = input.MapTo<financialYear>();
            await _financialRepository.InsertAsync(result);
        }

        public async Task DeleteFinancialYear(EntityDto input)
        {
            await _financialRepository.DeleteAsync(input.Id);
        }

        public bool FinancialYearExsistence(GetFinancialYearDto input)
        {
            return _financialRepository.GetAll().Where(e => e.Title == input.Title && e.StartYear == input.StartYear && e.EndYear == input.EndYear).Any();
        }
        public bool FinancialYearExsistenceByid(GetFinancialYearDto input)
        {
            return _financialRepository.GetAll().Where(e => e.Title == input.Title && e.StartYear == input.StartYear && e.EndYear == input.EndYear && e.Id!=input.Id).Any();
        }

        public async Task<FinancialYearListDto> GetFinancialDataById(EntityDto input)
        {
            var c = (await _financialRepository.GetAsync(input.Id)).MapTo<FinancialYearListDto>();
            return c;
        }

        public List<FinancialYearDto> GetFinancialYear()
        {
            List<FinancialYearDto> years = new List<FinancialYearDto>();
            int i = 1;
            for (DateTime date = DateTime.Now.AddYears(-5); date <= DateTime.Now.AddYears(5); date = date.AddYears(1))
            {

                years.Add(new FinancialYearDto { Id = date.Year, Year = date.Year.ToString() });
                i++;
            };

            var data = years.ToList();
            return years;

            
        }

        public PagedResultDto<FinancialYearListDto> GetFinancialYearList(GetFinanceYearDto Input)
        {
            var result = (from a in _financialRepository.GetAll()
                          select new FinancialYearListDto
                          {
                              Id = a.Id,
                              StartYear = a.StartYear,
                              EndYear = a.EndYear,
                              Title = a.Title,

                          });
            //.OrderByDescending(x => x.Id)
            //.ToList();
            var userData = result.OrderBy(Input.Sorting).PageBy(Input).ToList();
            var userCount = result.Count();
            return new PagedResultDto<FinancialYearListDto>(userCount, userData.MapTo<List<FinancialYearListDto>>());
        }

        public async Task UpdateFinancialYear(EditFinancialYearDto input)
        {
            var Tests = await _financialRepository.GetAsync(input.Id);

            
            Tests.StartYear = input.StartYear;
            Tests.EndYear = input.EndYear;
            Tests.Title = input.Title;


            await _financialRepository.UpdateAsync(Tests);
        }
    }
}
