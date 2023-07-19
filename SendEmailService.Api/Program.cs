using SendEamilService.Infrastructure;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
//var sqlstr = "Data Source=10.36.10.167;Initial Catalog=dotnet_erp60;User ID=sa;Password=1qaz@WSX;Encrypt=True;TrustServerCertificate=True;";
//var sqlstr = new ConfigurationBuilder()
//                 .SetBasePath(Directory.GetCurrentDirectory())
//                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                 .Build()
//                 .GetConnectionString("dbStr");
////�������ݿⷽ��һ�����ð�����ʵ���������ݿ�
//SqlHelper.Constr = sqlstr;
//ע��
//builder.Services.AddDbContext<MyDbContext>(x => x.UseSqlServer(sqlstr));

//�������ݿⷽ������ef�������ݿ�
//���Dbcontext����
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
