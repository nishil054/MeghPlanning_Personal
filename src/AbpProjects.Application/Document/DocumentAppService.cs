using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AbpProjects.Document.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Extensions;
using Abp.Authorization;
using AbpProjects.Authorization;
using AbpProjects.FileUploadByDirective;

namespace AbpProjects.Document
{
    [AbpAuthorize(PermissionNames.Pages_DataVault, PermissionNames.Pages_Documents)]
    //[AbpAuthorize]
    public class DocumentAppService : AbpProjectsApplicationModule, IDocumentAppService
    {
        private readonly IRepository<document> _documentRepository;
        private readonly IRepository<Documentmaster> _documentmasterRepository;
        private readonly IRepository<Documentchild> _documentchildRepository;
        public DocumentAppService(IRepository<document> documentRepository, IRepository<Documentmaster> documentmasterRepository, IRepository<Documentchild> documentchildRepository)
        {
            _documentRepository = documentRepository;
            _documentmasterRepository = documentmasterRepository;
            _documentchildRepository = documentchildRepository;
        }
        public async Task<int> CreateDocument(CreateDocumentDto input)
        {
            var documentcreate = input.MapTo<document>();
            int Id = await _documentRepository.InsertAndGetIdAsync(documentcreate);
            return Id;
        }

        public async Task DeleteDocument(EntityDto input)
        {
            await _documentRepository.DeleteAsync(input.Id);
        }

        public bool DocumentExsistence(DocumentDto input)
        {
            return _documentRepository.GetAll().Where(e => e.Title == input.Title).Any();
        }

        public bool DocumentExsistenceById(DocumentDto input)
        {
            return _documentRepository.GetAll().Where(e => e.Title == input.Title && e.Id != input.Id).Any();
        }

        public async Task<DocumentDto> GetDataById(EntityDto input)
        {
            var sname = (await _documentRepository.GetAsync(input.Id)).MapTo<DocumentDto>();
            return sname;
        }

        public async Task<PagedResultDto<DocumentDto>> GetDocumentData(GetDocumentDto input)
        {
            var Query = _documentRepository.GetAll();
            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<DocumentDto>(userCount, userData.MapTo<List<DocumentDto>>());
        }

        public async Task<PagedResultDto<DocumentDto>> GetDocumentList(GetDocumentDto input)
        {
            var cc = _documentRepository.GetAll()
              .WhereIf(!input.Title.IsNullOrEmpty(), p => p.Title.ToLower().Contains(input.Title.ToLower())
             );
            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            //return await Task.FromResult(cc.ToList());
            return new PagedResultDto<DocumentDto>(ccCount, ccData.MapTo<List<DocumentDto>>());
        }

        public async Task UpdateDocument(EditDocumentDto input)
        {
            var document = await _documentRepository.GetAsync(input.Id);
            document.Title = input.Title;
            document.Attachment = input.Attachment;
            await _documentRepository.UpdateAsync(document);
        }

        public async Task<int> CreateFileUploadDocument(CreateFileUploadDto input)
        {
            var document = input.MapTo<Documentmaster>();
            int Id = await _documentmasterRepository.InsertAndGetIdAsync(document);
            return Id;
        }

        public async Task FileUploadDocument(FileUploadDto input)
        {
            if (input.FileName != null && input.FileName.Count() != 0)
            {
                for (int i = 0; i < input.FileName.Count(); i++)
                {
                    Documentchild doc = new Documentchild();
                    doc.FileName = input.FileName[i];
                    doc.DocumentId = input.Id;
                    await _documentchildRepository.InsertAsync(doc);
                }
            }
        }
    }
}
