using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class UnitMapping
    {
        //myUserBusinessUnitMapping
       [Key]
        public Guid myUserBusinessUnitMappingId { get; set; }
      //  public DateTime VersionNumber { get; set; }
        public Guid UserId { get; set; }
        public Guid BUGUID { get; set; }
        public string ModifiedName { get; set; }
        public Guid CreatedGUID { get; set; }
        public string CreatedName { get; set; }
        public DateTime CreatedTime { get; set; } 
        public Guid ModifiedGUID { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
