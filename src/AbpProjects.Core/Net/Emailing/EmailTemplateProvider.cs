﻿using Abp.Dependency;
using Abp.Extensions;
using Abp.IO.Extensions;
using AbpProjects.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Net.Emailing
{
    public class EmailTemplateProvider : IEmailTemplateProvider, ITransientDependency
    {
        private readonly IWebUrlService _webUrlService;

        public EmailTemplateProvider(
        IWebUrlService webUrlService)
        {
            _webUrlService = webUrlService;
        }

        public string GetDefaultTemplate(int? tenantId)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AbpProjects.Net.Emailing.EmailTemplates.default.html"))
            {
                var bytes = stream.GetAllBytes();
                var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
                return template.Replace("{EMAIL_LOGO_URL}", GetTenantLogoUrl(tenantId));
            }
        }

        private string GetTenantLogoUrl(int? tenantId)
        {
            if (!tenantId.HasValue)
            {
                return _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo";
            }

            return _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?tenantId=" + tenantId.Value;
        }
    }
}
