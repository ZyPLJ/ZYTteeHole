using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ZYTreeHole API", Version = "v1" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
    c.IncludeXmlComments(filePath, true);
});
builder.Services.AddOptions();
builder.Services.AddSignalR();
builder.WebHost.UseUrls("http://*:44323");


builder.Services.AddDbContext<MyDbContext>(options =>  
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));  

// Add services to the container.
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddSingleton<TempFilterService>();
builder.Services.AddSingleton<ChatHub>();
//注册服务
builder.Services.AddRateLimit(builder.Configuration);
//注册jwt配置
builder.Services.AddAuth(builder.Configuration);
builder.Services.Configure<Auth>(builder.Configuration.GetSection(nameof(Auth)));
builder.Services.AddScoped<AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", opt =>
    {
        opt.WithOrigins("http://localhost:5173", "http://tree.pljzy.top") // 指定允许的来源
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() // 允许使用凭据（如cookies）
            .WithExposedHeaders("http://localhost:5173/", "http://tree.pljzy.top/");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZYTreeHole API V1");
    });
}
app.UseRateLimit();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
});

app.MapControllers();

app.MapHub<ChatHub>("/ChatHub");

app.Run();

public partial class Program { }