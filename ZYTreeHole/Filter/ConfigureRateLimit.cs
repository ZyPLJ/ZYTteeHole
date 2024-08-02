using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using StackExchange.Redis;

namespace ZYTreeHole.Filter;

public static class ConfigureRateLimit
{
    public static void AddRateLimit(this IServiceCollection services,IConfiguration conf)
    {
        services.Configure<IpRateLimitOptions>(conf.GetSection("IpRateLimiting"));
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();
    }
    public static IApplicationBuilder UseRateLimit(this IApplicationBuilder app)
    {
        app.UseIpRateLimiting();
        return app;
    }
}