using Microsoft.EntityFrameworkCore;
using SendEamilService.Domain;

namespace SendEamilService.Infrastructure
{
    public class MyDbContext : DbContext
    {
		
		/// <summary>
		/// 默认构造函数 使用方法与原来一样
		/// </summary>
		public MyDbContext() : base() { }
		/// <summary>
		/// 通过IOC
		/// </summary>
		/// <param name="options"></param>
		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{ }
		

		/// <summary>
		/// 公司组织
		/// </summary>
		public DbSet<myBusinessUnitModel> myBusinessUnit { get; set; }
		/// <summary>
		/// 项目
		/// </summary>
		public DbSet<ProjectModel> p_Project { get; set; }
		/// <summary>
		/// 用户表
		/// </summary>
		public DbSet<UserModel>	myUser { get; set; }
		/// <summary>
		/// 组织映射表
		/// </summary>
		public DbSet<UnitMapping> myUserBusinessUnitMapping { get; set; }
		/// <summary>
		///邮箱配置表
		/// </summary>
		public DbSet<MessageSettingEmailModel> messageSettingEmail { get; set; }
	}
}