using AbpProjects.Opportunities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.BulkData.Dto
{
  public class InsertBulkData
    {
        public List<bulkmaster> BulkItemsData { get; set; }
    }
    public class InsertOpportunityBulkData
    {
        public List<Opportunity> BulkOpportunityItemsData { get; set; }
    }

    public class ExcelValidationList
    {
        public virtual bool isnullcolumn { get; set; }
        public List<string> rowcount { get; set; }
        public int successrowcount { get; set; }
        public List<string> errorresult { get; set; }
        public string nullresult { get; set; }
        //public string notexsistresult { get; set; }
        //public string documentExsist { get; set; }
    }
}
