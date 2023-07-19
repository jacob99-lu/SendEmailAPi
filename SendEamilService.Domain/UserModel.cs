using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class UserModel
    {
        [Key]
        public Guid UserGUID { get; set; }
        public string ADAccount { get; set; }
        public string UserName { get; set; }
        public string? UserCode { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Guid BUGUID { get; set; }
        public int IsAdmin { get; set; }
        public string UserKind { get; set; }
        public string IsDisabeld { get; set; }
        public int IsFirstLogin { get; set; }
        public int IsModelingUser { get; set; }
        public int IsOldUser { get; set; }
        public int IsUserChangePWD { get; set; }
        public string? MobilePhone { get; set; }
        public DateTime PasswordModifyTime { get; set; }
        public Guid CreatedGUID { get; set; }
        public string CreatedName { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid ModifiedGUID { get; set; }
        public string ModifiedName { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
