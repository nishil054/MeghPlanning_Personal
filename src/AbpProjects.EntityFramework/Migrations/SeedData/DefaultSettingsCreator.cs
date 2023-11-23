using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using Abp.Timing;
using AbpProjects.EntityFramework;

namespace AbpProjects.Migrations.SeedData
{
    public class DefaultSettingsCreator
    {
        private readonly AbpProjectsDbContext _context;
        public DefaultSettingsCreator(AbpProjectsDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            int? tenantId = null;
            if (AbpProjectsConsts.MultiTenancyEnabled == false)
            {
                tenantId = 1;
            }
            else
            {
                tenantId = null;
            }
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "megherrorlog@gmail.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "ABP Projects", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "true", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "Google$2020", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "mailtest@meghtechnologies.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "587", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "smtp.gmail.com", tenantId);

            //vikas

            //AddSettingIfNotExists("text", "SSvikas.guptaw@meghtechnologies.com",tenantId);
            //AddSettingIfNotExists("Check", "DUw Projects", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "false", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "true", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "Megh1TechGoogale@2017$ErrorLog", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "vikas.guptaa@meghtechnologies.com", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "588", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "smtp.gmail.com", tenantId);

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage,"en");

            //Date
            AddSettingIfNotExists(TimingSettingNames.TimeZone, "India Standard Time");
            AddSettingIfNotExists("CDN Url", "https://www.google.com/", 1);
            AddSettingIfNotExists("CDN Folder Name", "CDNFolder", 1);
            AddSettingIfNotExists("CDN User Name", "Nitin Patel", 1);
            AddSettingIfNotExists("CDN Key", "Key123456", 1);
            AddSettingIfNotExists("CDN Container", "Container", 1);
            AddSettingIfNotExists("CDN Flag", "false", 1);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}