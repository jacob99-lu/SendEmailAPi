using System.ComponentModel.DataAnnotations;

namespace SendEamilService.Domain
{
    public class myBusinessUnitModel
    {
        /// <summary>
        /// 层级
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 公司编码
        /// </summary>
        [Key]
        public string? BUCode { get; set; }
        /// <summary>
        /// 公司父级GUID
        /// </summary>
        public Guid? ParentGUID { get; set; }
        /// <summary>
        /// 公司GUID
        /// </summary>
        public Guid? BUGUID { get; set; }
        ///// <summary>
        ///// 公司名称
        ///// </summary>
        public string? BUName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string? OrderHierarchyCode { get; set; }


    }
}