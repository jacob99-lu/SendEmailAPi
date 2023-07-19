using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    public class ADUserModel
    {
      public string GsName { get; set; }
      public string ADName { get; set; }
      public string ADId { get; set; }
      public string ADGUID { get; set; }
      public string? email { get; set; }
      public string systype { get; set; }
      public Guid gsGUID { get; set; }
      public string status { get; set; }
      public Boolean remark { get; set; }
      public Boolean createUser { get; set; }
    }
   
}
