using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportExcelData.Dto
{
    public class ExcelValidationList
    {
        public virtual bool isnullcolumn { get; set; }
        public List<string> rowcount { get; set; }
        public int successrowcount { get; set; }
        //public List<string> nullcolumnscount { get; set; }
        //public List<string> notexsistcolumnscount { get; set; }
        public List<string> errorresult { get; set; }
        public string nullresult { get; set; }
        public string notexsistresult { get; set; }
        public string documentExsist { get; set; }
    }
}
