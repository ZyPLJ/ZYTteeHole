using Microsoft.EntityFrameworkCore;
using ZYTreeHole_Models;
using ZYTreeHole_Services.Services;
using ZYTreeHole_Services.Services.User;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<MyDbContext>(opt =>
{
    string connStr = @"Server=DESKTOP-U64RN7B\SQLSERVER;Database=TreeHole;User Id=Sa;Password=zyplj1314999;Encrypt=True;TrustServerCertificate=True;";
    opt.UseSqlServer(connStr);
});
// Add services to the container.
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddSingleton<TempFilterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }