using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Document.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Document
{
  public interface IDocumentAppService : IApplicationService
    {
        //Task<int> CreateTaxDocument(CreateTaxDocumentDto input);
        //Task<TaxDocumentListDto> GetTaxDocumentEdit(EntityDto input);
        //Task UpdateTaxDocument(EditTaxDocumentDto input);
        //Task DeleteTaxDocument(EntityDto inputs);
        Task<int> CreateDocument(CreateDocumentDto input);
        Task<DocumentDto> GetDataById(EntityDto input);
        Task UpdateDocument(EditDocumentDto input);
        Task DeleteDocument(EntityDto input);
        bool DocumentExsistence(DocumentDto input);
        bool DocumentExsistenceById(DocumentDto input);
        //Task<PagedResultDto<TaxDocumentListDto>> GetTaxDocumentList(TaxDocumentMasterInput input);
        //Task<PagedResultDto<TaxDocumentListDto>> GetTaxDocumentMaster(TaxDocumentMasterInput input);
        Task<PagedResultDto<DocumentDto>> GetDocumentList(GetDocumentDto input);
        Task<PagedResultDto<DocumentDto>> GetDocumentData(GetDocumentDto input);

        //File Upload by Directive
        Task<int> CreateFileUploadDocument(CreateFileUploadDto input);
        Task FileUploadDocument(FileUploadDto input);
    }
}
