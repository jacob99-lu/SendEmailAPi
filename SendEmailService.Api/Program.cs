using SendEamilService.Infrastructure;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
//var sqlstr = "Data Source=10.36.10.167;Initial Catalog=dotnet_erp60;User ID=sa;Password=1qaz@WSX;Encrypt=True;TrustServerCertificate=True;";
//var sqlstr = new ConfigurationBuilder()
//                 .SetBasePath(Directory.GetCurrentDirectory())
//                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                 .Build()
//                 .GetConnectionString("dbStr");
////链接数据库方法一：采用帮助类实现链接数据库
//SqlHelper.Constr = sqlstr;
//注入
//builder.Services.AddDbContext<MyDbContext>(x => x.UseSqlServer(sqlstr));

//链接数据库方法二：ef链接数据库
//添加Dbcontext服务
builder.Services.AddDbContext<MyDbContext>(options =>
{
    var constr = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .Build()
                 .GetConnectionString("dbStr");
    options.UseSqlServer(constr);
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
