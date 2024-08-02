using Microsoft.EntityFrameworkCore;
using ZYTreeHole_Models;
using ZYTreeHole_Services.Services;
using ZYTreeHole_Services.Services.User;
using ZYTreeHole.Filter;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();



builder.Services.AddDbContext<MyDbContext>(opt =>
{
    string connStr = @"Server=DESKTOP-U64RN7B\SQLSERVER;Database=TreeHole;User Id=Sa;Password=zyplj1314999;Encrypt=True;TrustServerCertificate=True;";
    opt.UseSqlServer(connStr);
});
// Add services to the container.
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddSingleton<TempFilterService>();
//注册服务
builder.Services.AddRateLimit(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//添加中间件
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
});
app.UseRateLimit();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }