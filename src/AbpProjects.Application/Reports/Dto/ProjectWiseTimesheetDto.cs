using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    public class ProjectWiseTimesheetDto
    {

        public virtual DateTime Date { get; set; }
        public virtual List<Getuserlist> UserList { get; set; }
        public virtual List<GetTmesheetData> TimesheetData { get; set; }


    }

    public class Getuserlist
    {
        public virtual int Userid { get; set; }
        public virtual string UserName { get; set; }

    }

    public class GetTmesheetData
    {
        public virtual decimal hours { get; set; }
        public virtual List<string> Description { get; set; }
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
    }

    public class GetUserdateCombo
    {
        public virtual int userid { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }
    }


}
