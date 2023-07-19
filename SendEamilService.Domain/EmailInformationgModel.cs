using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class EmailInformationgModel
    {
        public string strTitle { get; set; }
        public string strCC { get; set; }
        public string strRecipient { get; set; }
        public string strContent { get; set; }
        
    }
}
