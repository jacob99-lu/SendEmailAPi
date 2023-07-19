using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain
{
    /// <summary>
    /// 定义递归方法 将列表的中节点 以树形存储数据结构
    /// </summary>
    public class TreeJsonExtension
    {
        /// <summary>
        ///  递归生成json树
        /// </summary>
        /// <param name="menuEntities"> 列表数据   </param>
        /// <param name="id"> 树根编号  </param>
        /// <returns> 递归后的Node  </returns>
        public static TreeObject BindTree(List<TreeObject> List, Guid id)
        {

           
            TreeObject node = List.Where(t => t.BUGUID == id).First();
            try
            {
                List<TreeObject> childTreeNodes = List.Where(t => t.ParentGUID == id).ToList();
                foreach (var child in childTreeNodes)
                {
                    TreeObject n = BindTree(List, child.BUGUID);
                    node.nodes.Add(n);
                }
            }
            catch { }
            return node;
        }
    }
}
