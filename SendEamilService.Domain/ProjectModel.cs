using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class ProjectModel
    {
        [Key]
        public Guid BUGUID { get; set; }
        public string? City { get; set; }
     //   public ICollection<ProjectModel>? children { get; set; }

    }
}
