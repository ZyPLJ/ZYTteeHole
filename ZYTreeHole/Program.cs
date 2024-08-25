using Microsoft.EntityFrameworkCore;
using ZYTreeHole_Models;
using ZYTreeHole_Models.ViewModels.Config;
using ZYTreeHole_Services.Services;
using ZYTreeHole_Services.Services.User;
using ZYTreeHole.Extensions;
using ZYTreeHole.Filter;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.WebHost.UseUrls("http://*:44323");


builder.Services.AddDbContext<MyDbContext>(options =>  
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));  

// Add services to the container.
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddSingleton<TempFilterService>();
//注册服务
builder.Services.AddRateLimit(builder.Configuration);
//注册jwt配置
builder.Services.AddAuth(builder.Configuration);
builder.Services.Configure<Auth>(builder.Configuration.GetSection(nameof(Auth)));
builder.Services.AddScoped<AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        opt => opt.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("http://localhost:5173/"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
//添加中间件
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
});
app.UseRateLimit();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }