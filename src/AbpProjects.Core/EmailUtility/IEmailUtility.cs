using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.EmailUtility
{
    public interface IEmailUtility
    {
        void Send(string to, string cc, string subject, string body);
    }
}
