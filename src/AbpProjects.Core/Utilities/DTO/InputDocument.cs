using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Utilities.DTO
{
    public class InputDocument
    {
        public string CDNUrl { get; set; }
        public string CDNFolderName { get; set; }
        public string CDNUserName { get; set; }
        public string CDNKey { get; set; }
        public string CDNContainer { get; set; }
        public bool CDNFlag { get; set; }
    }
}
