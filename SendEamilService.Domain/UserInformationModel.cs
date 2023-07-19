using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class UserInformationModel
    {
        [Key]
        public int ID { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
