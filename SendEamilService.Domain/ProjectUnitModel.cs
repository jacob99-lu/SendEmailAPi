using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class ProjectUnitModel : myBusinessUnitModel
    {
        public Guid? Id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }

        public List<ProjectUnitModel> children = new List<ProjectUnitModel>();
    }
}
