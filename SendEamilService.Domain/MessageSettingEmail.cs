using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class MessageSettingEmail
    {
        public int ID { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string QYEmail { get; set; }
    }
}
