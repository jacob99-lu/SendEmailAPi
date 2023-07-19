using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class MessageSettingEmailModel
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string QYEmail { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? modificationTim { get; set; }
        public string? Password { get; set; }
    }
}
