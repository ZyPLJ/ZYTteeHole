using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ZYTreeHole_Models.ViewModels.Config;
using ZYTreeHole_Services.Services;

namespace ZYTreeHole.Extensions;


public static class ConfigureAuth {
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<AuthService>();
        services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                var authSetting = configuration.GetSection(nameof(Auth)).Get<Auth>();
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authSetting.Jwt.Issuer,
                    ValidAudience = authSetting.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSetting.Jwt.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}

