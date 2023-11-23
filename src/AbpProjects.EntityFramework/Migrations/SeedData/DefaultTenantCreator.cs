using System.Linq;
using AbpProjects.EntityFramework;
using AbpProjects.MultiTenancy;
using AbpProjects.Team;

namespace AbpProjects.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly AbpProjectsDbContext _context;

        public DefaultTenantCreator(AbpProjectsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
            CreateDefaultTeam();
            CreateProjectStatus();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
        public void CreateDefaultTeam()
        {
            //default Team
            var DeafultTeam1 = _context.Teams.FirstOrDefault(p => p.TeamName == "Dot Net");
            if (DeafultTeam1 == null)
            {
                _context.Teams.Add(
                    new Team.team
                    {
                        Id = 1,
                        TeamName = "Dot Net"

                    });
            }
            var DeafultTeam2 = _context.Teams.FirstOrDefault(p => p.TeamName == "PHP");
            if (DeafultTeam2 == null)
            {
                _context.Teams.Add(
                    new Team.team
                    {
                        Id = 2,
                        TeamName = "PHP"

                    });
            }
            var DeafultTeam3 = _context.Teams.FirstOrDefault(p => p.TeamName == "Design");
            if (DeafultTeam3 == null)
            {
                _context.Teams.Add(
                    new Team.team
                    {
                        Id = 3,
                        TeamName = "Design"

                    });
            }
            var DeafultTeam4 = _context.Teams.FirstOrDefault(p => p.TeamName == "Marketing");
            if (DeafultTeam4 == null)
            {
                _context.Teams.Add(
                    new Team.team
                    {
                        Id = 4,
                        TeamName = "Marketing"

                    });
            }
            var DeafultTeam5 = _context.Teams.FirstOrDefault(p => p.TeamName == "Accounts & HR");
            if (DeafultTeam5 == null)
            {
                _context.Teams.Add(
                    new Team.team
                    {
                        Id = 5,
                        TeamName = "Accounts & HR"
                    });
            }


        }

        public void CreateProjectStatus()
        {
            //default Project Status
            var DeafultTeam1 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "Invoice / Collecction");
            if (DeafultTeam1 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 1,
                        Status = "Invoice / Collecction",
                        sortorder=7

                    });
            }
            var DeafultTeam2 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "Completed Project");
            if (DeafultTeam2 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 2,
                        Status = "Completed Project",
                        sortorder = 8

                    });
            }
            var DeafultTeam3 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "On Hold");
            if (DeafultTeam3 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 3,
                        Status = "On Hold",
                        sortorder = 6

                    });
            }
            var DeafultTeam4 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "Planning");
            if (DeafultTeam4 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 4,
                        Status = "Planning",
                        sortorder = 1

                    });
            }
            var DeafultTeam5 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "Design");
            if (DeafultTeam5 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 5,
                        Status = "Design",
                        sortorder = 2
                    });
            }
            var DeafultTeam6 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "Working");
            if (DeafultTeam6 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 6,
                        Status = "Working",
                        sortorder = 3
                    });
            }
            var DeafultTeam7 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "Review");
            if (DeafultTeam7 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 7,
                        Status = "Review",
                        sortorder = 4
                    });
            }
            var DeafultTeam8 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "On Going");
            if (DeafultTeam8 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 8,
                        Status = "On Going",
                        sortorder = 5
                    });
            }
            var DeafultTeam9 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "AMC");
            if (DeafultTeam9 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 9,
                        Status = "AMC",
                        sortorder = 9
                    });
            }
            var DeafultTeam10 = _context.ProjectStatuses.FirstOrDefault(p => p.Status == "Pending");
            if (DeafultTeam10 == null)
            {
                _context.ProjectStatuses.Add(
                    new Project.ProjectStatus
                    {
                        Id = 10,
                        Status = "Pending",
                        sortorder = 0
                    });
            }
        }
    }
}
