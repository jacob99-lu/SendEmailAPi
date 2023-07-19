using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    /// <summary>
    /// 定义 TreeObject类 
    /// </summary>
    public class TreeObject
    {
        public Guid BUGUID { get; set; }
        public string BUName { get; set; }

        public Guid ParentGUID { get; set; }
        public List<TreeObject> nodes = new List<TreeObject>();
        public void Addchildren(TreeObject node)
        {
            this.nodes.Add(node);
        }



    }
}
